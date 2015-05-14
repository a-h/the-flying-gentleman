using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FlyingGentleman.Mocks.Tests
{
	[TestFixture]
	public class CoverageTest
	{
		[Test]
		public void Test()
		{
			var target = new MockTarget("test", (a) => Console.WriteLine(a));
		}
	}
}
