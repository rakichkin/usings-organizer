#nullable enable

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace UsingsOrganizer;

public static class UsingsOrganizer
{
	public static string GetFormattedUsingsBlockText(string rawUsingsText, UsingsOrganizerOptions options = null)
	{
		if(options == null) options = new UsingsOrganizerOptions();

		var separator = FindMostFrequentLineSeparator(rawUsingsText);
		if(separator == null) return rawUsingsText;

		var lines = rawUsingsText.Split([separator], StringSplitOptions.RemoveEmptyEntries).ToList();

		var systemUsings = GetSortedUsingsByName("System", lines);
		var mallenomUsings = GetSortedUsingsByName("Mallenom", lines);
		var eyecontUsings = GetSortedUsingsByName("EyeCont", lines);

		lines.Sort(StringComparer.InvariantCulture);
		var usingsWithEquation = lines.GetAndRemove(l => l.Contains("="));
		usingsWithEquation.Sort(StringComparer.InvariantCulture);
		lines = [.. lines, .. usingsWithEquation];

		var resultSb = new StringBuilder();

		if(systemUsings.Count > 0)
		{
			resultSb.Append(string.Join(separator, systemUsings) + separator + separator);
		}
		if(lines.Count > 0)
		{
			resultSb.Append(string.Join(separator, lines) + separator + separator);
		}
		if(mallenomUsings.Count > 0)
		{
			resultSb.Append(string.Join(separator, mallenomUsings) + separator + separator);
		}
		if(eyecontUsings.Count > 0)
		{
			resultSb.Append(string.Join(separator, eyecontUsings) + separator + separator);
		}

		return resultSb.ToString();
	}

	private static List<string> GetSortedUsingsByName(string name, List<string> usingLines)
	{
		var targetUsings = usingLines.GetAndRemove(l => l.StartsWith($"using {name}") || l.Contains($"= {name}"));
		targetUsings.Sort(StringComparer.InvariantCulture);

		var usingsWithEquation = targetUsings.GetAndRemove(l => l.Contains("="));
		usingsWithEquation.Sort(CompareUsings);

		return targetUsings.Concat(usingsWithEquation).ToList();
	}

	private static int CompareUsings(string u1, string u2)
		=> string.Compare(u1, u2, false, CultureInfo.GetCultureInfo("en-US"));

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
