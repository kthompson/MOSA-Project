// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Push32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class Push32 : X86Instruction
	{
		public override int ID { get { return 300; } }

		internal Push32()
			: base(0, 1)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.OpcodeEncoder.AppendByte(0xFF);
			emitter.OpcodeEncoder.Append2Bits(0b11);
			emitter.OpcodeEncoder.Append3Bits(0b110);
			emitter.OpcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
		}
	}
}