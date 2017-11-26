// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// IntrinsicMethodCall
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class IntrinsicMethodCall : BaseIRInstruction
	{
		public IntrinsicMethodCall()
			: base(0, 0)
		{
		}

		public override bool VariableOperandCount { get { return true; } }

		public override bool HasSideEffect { get { return true; } }
	}
}

