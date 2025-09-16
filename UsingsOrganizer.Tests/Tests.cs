using UsingsOrganizer.Core.UsingsExtraction;

namespace UsingsOrganizer.Tests;

public class Tests
{
	private UsingsOrganizer.Core.UsingsOrganizer _organizer;

	[SetUp]
    public void Setup()
    {
		_organizer = new UsingsOrganizer.Core.UsingsOrganizer();
    }

    [Test]
	public void DefaultUsingsFormattingTest()
	{
		var rawUsings = File.ReadAllText(Path.Combine("TestData", "DefaultUsingsFormattingTest", "raw_usings.txt"));
		var formattedUsingsActual = _organizer.Organize(rawUsings);
		var formattedUsingsExpected = File.ReadAllText(
			Path.Combine("TestData", "DefaultUsingsFormattingTest", "organized_usings.txt"));

		Assert.That(formattedUsingsActual, Is.EqualTo(formattedUsingsExpected));
	}
}
