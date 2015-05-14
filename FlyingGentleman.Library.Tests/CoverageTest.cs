using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FlyingGentleman.Library.Tests
{
	[TestFixture]
	public class CoverageTest
	{
		[Test]
		public void Test()
		{
			var e = new LogEvent("category", "action", "message");
		}
	}
}
