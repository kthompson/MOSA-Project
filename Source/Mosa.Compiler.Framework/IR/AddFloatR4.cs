// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// AddFloatR4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class AddFloatR4 : BaseIRInstruction
	{
		public AddFloatR4()
			: base(1, 2)
		{
		}

		public override bool Commutative { get { return true; } }
	}
}

