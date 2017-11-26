// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// AddUnsigned
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class AddUnsigned : BaseIRInstruction
	{
		public AddUnsigned()
			: base(1, 2)
		{
		}

		public override bool Commutative { get { return true; } }
	}
}

