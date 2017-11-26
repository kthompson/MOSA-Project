// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// ExceptionStart
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class ExceptionStart : BaseIRInstruction
	{
		public ExceptionStart()
			: base(1, 0)
		{
		}

		public override bool IgnoreDuringCodeGeneration { get { return true; } }
	}
}

