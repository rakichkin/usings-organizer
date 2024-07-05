namespace Tests;

public class UsingsOrganizerTests
{
	[Test]
	public void DefaultUsingsFormattingTest()
	{
		var rawUsings = File.ReadAllText(Path.Combine("TestData", "DefaultUsingsFormattingTest", "raw_usings.txt"));
		var formattedUsingsActual = UsingsOrganizer.UsingsOrganizer.GetFormattedUsingsBlockText(rawUsings);
		var formattedUsingsExpected = File.ReadAllText(
			Path.Combine("TestData", "DefaultUsingsFormattingTest", "sorted_and_formatted_usings.txt"));
		Assert.That(formattedUsingsActual, Is.EqualTo(formattedUsingsExpected));
	}
}