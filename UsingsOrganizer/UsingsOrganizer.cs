#nullable enable

using System.Collections.Generic;
using System.Linq;


namespace UsingsOrganizer;

public static class UsingsOrganizer
{
	public static string GetFormattedUsingsBlockText(string rawUsingsText, UsingsOrganizerOptions options = null)
	{
		if(options == null) options = new UsingsOrganizerOptions();

		var separator = FindMostFrequentLineSeparator(rawUsingsText);
		if(separator == null) return rawUsingsText;

		var lines = rawUsingsText.Split([separator], StringSplitOptions.RemoveEmptyEntries).ToList();

		throw new NotImplementedException();
	}

	private static string? FindMostFrequentLineSeparator(string text)
	{
		var separatorCounts = new Dictionary<string, int>()
		{
			{ "\r\n", 0 },
			{ "\n", 0 },
			{ "\r", 0 }
		};
		for(int i = 0; i < text.Length; i++)
		{
			foreach(var sep in separatorCounts.Keys)
			{
				if(i + sep.Length <= text.Length && text.Substring(i, sep.Length) == sep)
				{
					separatorCounts[sep]++;
					i += sep.Length - 1; // перескакиваем через разделитель
					break;
				}
			}
		}

		if(separatorCounts.All(c => c.Value == 0)) return null;
		return separatorCounts.Where(kvp => kvp.Value == separatorCounts.Max(x => x.Value)).First().Key;
	}
}
