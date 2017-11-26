// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// AddSigned
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class AddSigned : BaseIRInstruction
	{
		public AddSigned()
			: base(1, 2)
		{
		}

		public override bool Commutative { get { return true; } }
	}
}

