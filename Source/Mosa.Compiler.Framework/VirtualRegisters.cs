/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class VirtualRegisters : IEnumerable<Operand>
	{

		#region Data members

		private List<Operand> virtualRegisters = new List<Operand>();

		//private int sequenceStart;

		#endregion // Data members

		#region Properties

		public int Count { get { return virtualRegisters.Count; } }

		public Operand this[int index] { get { return virtualRegisters[index]; } }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualRegisters"/> class.
		/// </summary>
		public VirtualRegisters(IArchitecture architecture)
		{
			//foreach (var register in architecture.RegisterSet)
			//	if (register.Index > startVirtualSequence)
			//		startVirtualSequence = register.Index;
			//sequenceStart = architecture.RegisterSet[architecture.RegisterSet.Length - 1].Index;
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand Allocate(SigType type)
		{
			int index = virtualRegisters.Count + 1;
			//int sequence = index + sequenceStart;
			Operand virtualRegister = Operand.CreateVirtualRegister(type, index);

			virtualRegisters.Add(virtualRegister);

			return virtualRegister;
		}

		public void SplitLongOperand(Operand longOperand, int highOffset, int lowOffset)
		{
			Debug.Assert(longOperand.StackType == StackTypeCode.Int64);

			if (longOperand.Low == null && longOperand.High == null)
			{
				virtualRegisters.Add(Operand.CreateHighSplitForLong(longOperand, highOffset, virtualRegisters.Count + 1));
				virtualRegisters.Add(Operand.CreateLowSplitForLong(longOperand, lowOffset, virtualRegisters.Count + 1));
			}
		}

		public IEnumerator<Operand> GetEnumerator()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				yield return virtualRegister;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}
