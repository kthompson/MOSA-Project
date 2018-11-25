// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// SetByteIfUnsignedLessThan
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class SetByteIfUnsignedLessThan : X64Instruction
	{
		public override int ID { get { return 553; } }

		internal SetByteIfUnsignedLessThan()
			: base(1, 0)
		{
		}

		public override string AlternativeName { get { return "SetB"; } }

		public override bool IsCarryFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X64.SetByteIfUnsignedGreaterOrEqual;
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 0);

			emitter.OpcodeEncoder.AppendByte(0x0F);
			emitter.OpcodeEncoder.AppendByte(0x92);
			emitter.OpcodeEncoder.Append2Bits(0b11);
			emitter.OpcodeEncoder.Append3Bits(0b000);
			emitter.OpcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
		}
	}
}
