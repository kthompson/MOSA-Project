// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// MulFloatR4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class MulFloatR4 : BaseIRInstruction
	{
		public MulFloatR4()
			: base(1, 2)
		{
		}

		public override bool Commutative { get { return true; } }
	}
}

