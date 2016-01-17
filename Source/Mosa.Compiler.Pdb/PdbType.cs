// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	///
	/// </summary>
	public abstract class PdbType
	{
		#region Data Members

		private int unknown1;
		private PdbSymbolRangeEx range;
		private short flag;
		private short stream;
		private int symbol_size;
		private int lineno_size;
		private int unknown2;
		private int nSrcFiles;
		private int attribute;
		private int reserved1;
		private int reserved2;
		private string name;
		private string unknown3;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PdbType"/> class.
		/// </summary>
		/// <param name="reader">The reader to initialize From.</param>
		protected PdbType(BinaryReader reader)
		{
			unknown1 = reader.ReadInt32();
			range = new PdbSymbolRangeEx(reader);
			flag = reader.ReadInt16();
			stream = reader.ReadInt16();
			symbol_size = reader.ReadInt32();
			lineno_size = reader.ReadInt32();
			unknown2 = reader.ReadInt32();
			nSrcFiles = reader.ReadInt32();
			attribute = reader.ReadInt32();
			reserved1 = reader.ReadInt32();
			reserved2 = reader.ReadInt32();
			name = CvUtil.ReadString(reader);
			unknown3 = CvUtil.ReadString(reader);
			CvUtil.PadToBoundary(reader, 4);
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the size of the line numbers in this type.
		/// </summary>
		/// <value>The size of the line numbers in this type.</value>
		protected int LineNoSize
		{
			get { return lineno_size; }
		}

		/// <summary>
		/// Gets the line numbers.
		/// </summary>
		/// <value>The line numbers.</value>
		public abstract IEnumerable<CvLine> LineNumbers { get; }

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		/// Retrieves the PDB stream, that describes this type.
		/// </summary>
		/// <value>The stream, which describes this type.</value>
		protected short Stream
		{
			get { return stream; }
		}

		/// <summary>
		/// Gets the symbols.
		/// </summary>
		/// <value>The symbols.</value>
		public abstract IEnumerable<CvSymbol> Symbols { get; }

		/// <summary>
		/// Gets the size of the symbol information.
		/// </summary>
		/// <value>The size of the symbol information.</value>
		protected int SymbolSize
		{
			get { return symbol_size; }
		}

		#endregion Properties
	}
}
