using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsingsOrganizer;

namespace Tests
{
	/// <summary>Модульные тесты для проверки работы расширения.</summary>
	[TestClass]
	public class UsingsOrganizerTests
	{
		private UsingsOrganizer.UsingsOrganizer _organizer;

		/// <summary>Подготавливает всё необходимое для проведения тестов.</summary>
		[TestInitialize]
		public void Initialize()
		{
			_organizer = new UsingsOrganizer.UsingsOrganizer(new UsingComparer());
		}

		/// <summary>Проверяет работу органайзера using-ов в стандартных условиях с дефолтными параметрами.</summary>
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
