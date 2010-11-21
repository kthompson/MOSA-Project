﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework.IR;

using Mosa.Platform.X86;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;

namespace Mosa.Tools.Compiler.Stages
{
	public class FakeSystemObjectGenerationStage : BaseMethodCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		private AssemblyCompiler compiler;

		private struct FakeEntry
		{
			public string Namespace { get; set; }
			public string TypeName { get; set; }
			public string Method { get; set; }
			public List<RuntimeParameter> Parameters { get; set; }
		}

		private static readonly List<FakeEntry> fakeEntries = new List<FakeEntry>
		{
			// System.Object
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@".ctor" },
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@"ToString" },
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@"GetHashCode" },
			new FakeEntry { Namespace=@"System", TypeName=@"Object", Method=@"Finalize" },

			// System.ValueType
			new FakeEntry { Namespace=@"System", TypeName=@"ValueType", Method=@"ToString" },
			new FakeEntry { Namespace=@"System", TypeName=@"ValueType", Method=@"GetHashCode" },
		};

		public void Run()
		{
			foreach (FakeEntry entry in fakeEntries)
			{
				RuntimeMethod method = this.GenerateMethod(entry.Namespace, entry.TypeName, entry.Method);
				this.GenerateInstructionSet();

				this.Compile(method);
			}

			// Special case for Object.Equals, ValueType.Equals :(
			this.CompileObjectEquals(@"Object");
			this.CompileObjectEquals(@"ValueType");
		}

		private void GenerateInstructionSet()
		{
			this.InstructionSet = new InstructionSet(1);

			Context ctx = this.CreateContext(-1);
			ctx.AppendInstruction(Mosa.Platform.X86.CPUx86.Instruction.RetInstruction);
		}

		private RuntimeMethod GenerateMethod(string nameSpace, string typeName, string methodName)
		{
			var type = new LinkerGeneratedType(moduleTypeSystem, nameSpace, typeName);

			// Create the method
			LinkerGeneratedMethod method = new LinkerGeneratedMethod(moduleTypeSystem, methodName, type);
			type.AddMethod(method);

			return method;
		}

		private void Compile(RuntimeMethod method)
		{
			LinkerMethodCompiler methodCompiler = new LinkerMethodCompiler(this.compiler, this.compiler.Pipeline.FindFirst<ICompilationSchedulerStage>(), method, this.InstructionSet);
			methodCompiler.Compile();
		}

		private void CompileObjectEquals(string typeName)
		{
			LinkerGeneratedType type = new LinkerGeneratedType(moduleTypeSystem, @"System", typeName);

			// Create the method
			LinkerGeneratedMethod method = new LinkerGeneratedMethod(moduleTypeSystem, @"Equals", type);
			method.Parameters.Add(new RuntimeParameter(null, @"obj", 0, ParameterAttributes.In));
			method.SetSignature(new MethodSignature(BuiltInSigType.Boolean, new SigType[] { BuiltInSigType.Object }));
			type.AddMethod(method);

			this.Compile(method);
		}

		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;
		}

		public string Name
		{
			get { return @"FakeCoreTypeGenerationStage"; }
		}
	}
}
