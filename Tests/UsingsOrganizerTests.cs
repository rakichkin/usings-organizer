using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class UsingsOrganizerTests
	{
		private UsingsOrganizer.UsingsOrganizer _organizer;

		[TestInitialize]
		public void Initialize()
		{
			_organizer = new UsingsOrganizer.UsingsOrganizer();
		}

		[TestMethod]
		public void DefaultUsingsFormattingTest()
		{
			var rawUsings = File.ReadAllText(Path.Combine("TestData", "DefaultUsingsFormattingTest", "raw_usings.txt"));
			var formattedUsingsActual = _organizer.Organize(rawUsings);
			var formattedUsingsExpected = File.ReadAllText(
				Path.Combine("TestData", "DefaultUsingsFormattingTest", "sorted_and_formatted_usings.txt"));

			Assert.IsTrue(string.Equals(formattedUsingsActual, formattedUsingsExpected));
		}
	}
}
