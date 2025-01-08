#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingsOrganizer;

public class UsingsOrganizer
{
	public string Organize(string rawUsingsText)
	{
		var separator = FindMostFrequentLineSeparator(rawUsingsText);
		if(separator == null) return rawUsingsText;

		var lines = rawUsingsText.Split([separator], StringSplitOptions.RemoveEmptyEntries).ToList();

		var usingGroups = GetUsingsGroups(lines);
		foreach(var group in usingGroups.Values) group.Sort(new UsingComparer());

		return ConcatenateUsingsString(usingGroups);
	}

	private static string ConcatenateUsingsString(IReadOnlyDictionary<string, List<string>> usingGroups)
	{
		var groups = usingGroups.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

		var sb = new StringBuilder();
		if(groups.TryGetValue("System", out var systemGroup))
		{
			foreach(var @using in systemGroup)
			{
				sb.AppendLine(@using);
			}
			sb.AppendLine();
		}
		groups.Remove("System");

		var hasMallenomGroup = groups.TryGetValue("Mallenom", out var mallenomGroup);
		var hasEyecontGroup = groups.TryGetValue("EyeCont", out var eyecontGroup);
		groups.Remove("Mallenom");
		groups.Remove("EyeCont");

		foreach(var group in groups)
		{
			foreach(var @using in group.Value)
			{
				sb.AppendLine(@using);
			}
			sb.AppendLine();
		}

		if(hasMallenomGroup)
		{
			foreach(var @using in mallenomGroup)
			{
				sb.AppendLine(@using);
			}
			sb.AppendLine();
		}

		if(hasEyecontGroup)
		{
			foreach(var @using in eyecontGroup)
			{
				sb.AppendLine(@using);
			}
		}

		var result = sb.ToString();
		return result;
	}

	private static Dictionary<string, List<string>> GetUsingsGroups(IEnumerable<string> codeTextLines)
	{
		var usingGroups = new Dictionary<string, List<string>>();
		foreach(var line in codeTextLines)
		{
			var trimmedLine = line.Trim();

			var spaceIndex = trimmedLine.LastIndexOf(" ");
			var dotIndex = trimmedLine.IndexOf(".");

			string groupName;
			if(dotIndex == -1) groupName = trimmedLine[(spaceIndex + 1)..(trimmedLine.Length - 1)];
			else groupName = trimmedLine[(spaceIndex + 1)..(dotIndex)];

			if(!usingGroups.ContainsKey(groupName)) usingGroups[groupName] = [];
			usingGroups[groupName].Add(line);
		}

		return usingGroups;
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
