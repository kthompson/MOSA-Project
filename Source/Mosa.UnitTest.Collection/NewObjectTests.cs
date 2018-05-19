﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public class NewObjectTests
	{
		[MosaUnitTest]
		public int Test()
		{
			return 5;
		}

		[MosaUnitTest]
		public static bool Create()
		{
			NewObjectTests d = new NewObjectTests();
			return d != null;
		}

		[MosaUnitTest]
		public static bool CreateAndCallMethod()
		{
			NewObjectTests d = new NewObjectTests();
			return d.Test() == 5;
		}
	}
}
