namespace UsingsOrganizer;

public static class UsingsOrganizer
{
	public static string GetFormattedUsingsBlockText(string rawUsingsText, UsingsOrganizerOptions options = null)
	{
		if(options == null) return SortUsingsDefault(rawUsingsText);

		throw new NotImplementedException();
	}

	private static string SortUsingsDefault(string rawUsingsText)
	{
		return string.Empty;
	}
}
