﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.Elf;
using Mosa.Compiler.LinkerFormat.Elf32;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Linker.Elf32
{
	public class Elf32 : BaseLinker
	{
		#region Constants

		#endregion Constants

		#region Data members

		protected Header elfheader = new Header();
		protected List<byte> stringTable = new List<byte>();
		protected SectionHeader stringSection = new SectionHeader();

		#endregion Data members

		public Elf32()
		{
			SectionAlignment = 0x1000;

			AddSection(new LinkerSection(SectionKind.Text, ".text", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, ".data", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, ".rodata", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, ".bss", SectionAlignment));
		}

		/// <summary>
		/// Emits the implementation.
		/// </summary>
		/// <param name="stream">The stream.</param>
		protected override void EmitImplementation(Stream stream)
		{
			var writer = new EndianAwareBinaryWriter(stream, Encoding.Unicode, Endianness);

			// Write ELF header
			WriteElfHeader(writer);

			// determine offsets
			uint sectionOffset = (uint)(elfheader.SectionHeaderOffset + (Header.SectionHeaderEntrySize * elfheader.SectionHeaderNumber));
			stringSection.Offset = sectionOffset + Alignment.AlignUp(GetSection(SectionKind.ROData).Offset + GetSection(SectionKind.ROData).Size, SectionAlignment);

			// Write program headers
			WriteProgramHeaders(writer, sectionOffset);

			// Write section headers
			WriteSectionHeaders(writer, sectionOffset);

			// Write sections
			foreach (var section in Sections)
			{
				stream.Position = (long)sectionOffset;
				section.WriteTo(stream);
			}

			WriteStringSection(writer);
		}

		private void WriteElfHeader(EndianAwareBinaryWriter writer)
		{
			elfheader.Type = FileType.Executable;
			elfheader.Machine = (MachineType)MachineID;
			elfheader.EntryAddress = (uint)EntryPoint.VirtualAddress;
			elfheader.CreateIdent(IdentClass.Class32, Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB, null);
			elfheader.ProgramHeaderOffset = Header.ElfHeaderSize;	// immediately after ELF header
			elfheader.ProgramHeaderNumber = (ushort)CountNonEmptySections();
			elfheader.SectionHeaderNumber = (ushort)(Sections.Length + 2);
			elfheader.SectionHeaderOffset = (uint)(elfheader.ProgramHeaderOffset + (Header.ProgramHeaderEntrySize * elfheader.ProgramHeaderNumber)); // immediately after program header
			elfheader.SectionHeaderStringIndex = 1;
			elfheader.Write(writer);
		}

		private void WriteProgramHeaders(EndianAwareBinaryWriter writer, uint sectionOffset)
		{
			writer.Position = elfheader.ProgramHeaderOffset;

			foreach (var section in Sections)
			{
				if (section.SectionKind == SectionKind.BSS || section.Size == 0)
					continue;

				var pheader = new ProgramHeader
				{
					Alignment = 0,
					FileSize = Alignment.AlignUp(section.Size, SectionAlignment),
					MemorySize = Alignment.AlignUp(section.Size, SectionAlignment),
					Offset = section.Offset + sectionOffset,
					VirtualAddress = (uint)section.VirtualAddress,
					PhysicalAddress = (uint)section.VirtualAddress,
					Type = ProgramHeaderType.Load,
					Flags =
						(section.SectionKind == SectionKind.Text) ? ProgramHeaderFlags.Read | ProgramHeaderFlags.Execute :
						(section.SectionKind == SectionKind.ROData) ? ProgramHeaderFlags.Read : ProgramHeaderFlags.Read | ProgramHeaderFlags.Write
				};

				pheader.Write(writer);
			}
		}

		private void WriteSectionHeaders(EndianAwareBinaryWriter writer, uint sectionOffset)
		{
			writer.Position = elfheader.SectionHeaderOffset;

			WriteNullHeaderSection(writer);

			foreach (var section in Sections)
			{
				var sheader = new SectionHeader();

				sheader.Name = AddToStringTable(section.Name);

				switch (section.SectionKind)
				{
					case SectionKind.Text: sheader.Type = SectionType.ProgBits; sheader.Flags = SectionAttribute.AllocExecute; break;
					case SectionKind.Data: sheader.Type = SectionType.ProgBits; sheader.Flags = SectionAttribute.Alloc | SectionAttribute.Write; break;
					case SectionKind.ROData: sheader.Type = SectionType.ProgBits; sheader.Flags = SectionAttribute.Alloc; break;
					case SectionKind.BSS: sheader.Type = SectionType.NoBits; sheader.Flags = SectionAttribute.Alloc | SectionAttribute.Write; break;
				}

				sheader.Address = (uint)section.VirtualAddress;
				sheader.Offset = (uint)section.Offset + sectionOffset;
				sheader.Size = (uint)Alignment.AlignUp(section.Size, SectionAlignment);
				sheader.Size = 0;
				sheader.Link = 0;
				sheader.Info = 0;
				sheader.AddressAlignment = 0;
				sheader.EntrySize = 0;
				sheader.Write(writer);
			}

			WriteStringHeaderSection(writer);
		}

		private static void WriteNullHeaderSection(EndianAwareBinaryWriter writer)
		{
			var nullsection = new SectionHeader();
			nullsection.Name = 0;
			nullsection.Type = SectionType.Null;
			nullsection.Flags = 0;
			nullsection.Address = 0;
			nullsection.Offset = 0;
			nullsection.Size = 0;
			nullsection.Link = 0;
			nullsection.Info = 0;
			nullsection.AddressAlignment = 0;
			nullsection.EntrySize = 0;
			nullsection.Write(writer);
		}

		private void WriteStringHeaderSection(EndianAwareBinaryWriter writer)
		{
			stringSection.Name = AddToStringTable(".shstrtab");
			stringSection.Type = SectionType.StringTable;
			stringSection.Flags = (SectionAttribute)0;
			stringSection.Address = 0;
			stringSection.Offset = Alignment.AlignUp(GetSection(SectionKind.ROData).Size, SectionAlignment);
			stringSection.Size = (uint)stringTable.Count;
			stringSection.Link = 0;
			stringSection.Info = 0;
			stringSection.AddressAlignment = 0;
			stringSection.EntrySize = 0;
			stringSection.Write(writer);
		}

		private void WriteStringSection(BinaryWriter writer)
		{
			writer.BaseStream.Position = stringSection.Offset;
			writer.Write((byte)'\0');
			writer.Write(stringTable.ToArray());
		}

		#region Internals

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		private int CountNonEmptySections()
		{
			int sections = 0;

			foreach (var section in Sections)
			{
				if (section.Size > 0 && section.SectionKind != SectionKind.BSS)
				{
					sections++;
				}
			}

			return sections;
		}

		public uint AddToStringTable(string text)
		{
			if (text.Length == 0)
				return (uint)(stringTable.Count + 1);

			uint index = (uint)stringTable.Count;

			foreach (char c in text)
			{
				stringTable.Add((byte)c);
			}

			stringTable.Add((byte)'\0');

			return index + 1;
		}

		#endregion Internals
	}
}