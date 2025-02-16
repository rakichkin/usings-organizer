#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingsOrganizer;

/// <summary>Органайзер секции подключенных пространств имён.</summary>
public class UsingsOrganizer(IComparer<string> usingStringComparer)
{
	/// <summary>Организовывает (делит на секции и сортирует) пространства имён в строке <paramref name="rawUsingsText"/>.</summary>
	/// <param name="rawUsingsText">Секция из .cs-файла с подключенными пространствами имён.</param>
	/// <returns>Секция с организованными пространствами имён.</returns>
	public string Organize(string rawUsingsText)
	{
		var newlineSymbol = FindMostFrequentLineSeparator(rawUsingsText);
		if(newlineSymbol == null) return rawUsingsText;

		var lines = rawUsingsText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();

		CheckNotSupportedUsings(lines);

		lines.Sort(usingStringComparer);
		var usingGroups = SplitByUsingGroups(lines);

		return ConcatenateUsingsString(usingGroups, newlineSymbol);
	}

	/// <summary>Ищет и возвращает наиболее часто встречаемый символ перехода на новую строку в <paramref name="text"/>.</summary>
	/// <param name="text">Строка, в которой необходимо найти символ.</param>
	/// <returns>Наиболее часто встречаемый символ перехода на новую строку, либо <see langword="null"/>, если такого символа в тексте нет.</returns>
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

	/// <summary>
	/// Проверяет, содержит ли список <paramref name="lines"/> неподдерживаемые на данный момент пространства имён, и выбрасывает исключение, если таковые имеются.
	/// </summary>
	/// <param name="lines">Список подключенных пространств имён.</param>
	private void CheckNotSupportedUsings(List<string> lines)
	{
		foreach(var line in lines)
		{
			if(line.Contains("=")) throw new NotImplementedException("Working with namespace aliases is not yet supported");
			if(line.Contains("global")) throw new NotImplementedException("Working with global namespaces is not yet supported");
		}
	}

	/// <summary>Разделяет множество строк <paramref name="usingLines"/>, представляющих подключенные пространства имён, на группы.</summary>
	/// <remarks>
	/// Отличительным признаком группы пространств имён (и ключом возвращаемого словаря, в данном случае) является её первое подпрострнаство. 
	/// К примеру, для пространства имён `Microsoft.AspNetCore.Hosting` отличительным признаком будет `Microsoft`.
	/// </remarks>
	/// <param name="usingLines">Множество подключенных пространств имён в формате `using <![CDATA[<]]>Example.Namespace<![CDATA[>]]>;.</param>
	/// <returns>Подключенные пространства имён, разделенные по группам.</returns>
	private static Dictionary<string, List<string>> SplitByUsingGroups(IEnumerable<string> usingLines)
	{
		var usingGroups = new Dictionary<string, List<string>>();
		foreach(var line in usingLines)
		{
			if(!line.Contains("using")) continue;
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

	/// <summary>Склеивает группы using-ов из <paramref name="usingGroups"/> в единую строку, используя разделитель <paramref name="newlineSymbol"/>.</summary>
	/// <param name="usingGroups">Группы пространств имён</param>
	/// <param name="newlineSymbol">Символ перехода на новую строку.</param>
	/// <returns>Строка, представляющая организованную секцию подключенных пространств имён.</returns>
	private static string ConcatenateUsingsString(IReadOnlyDictionary<string, List<string>> usingGroups, string newlineSymbol)
	{
		if(usingGroups.Count == 0) return string.Empty;

		var groups = usingGroups.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

		var sb = new StringBuilder();
		if(groups.TryGetValue("System", out var systemGroup))
		{
			foreach(var @using in systemGroup)
			{
				sb.Append(@using + newlineSymbol);
			}
		}
		groups.Remove("System");

		var hasMallenomGroup = groups.TryGetValue("Mallenom", out var mallenomGroup);
		var hasEyecontGroup = groups.TryGetValue("EyeCont", out var eyecontGroup);
		groups.Remove("Mallenom");
		groups.Remove("EyeCont");

		foreach(var group in groups)
		{
			if(sb.Length != 0) sb.Append(newlineSymbol);
			foreach(var @using in group.Value)
			{
				sb.Append(@using + newlineSymbol);
			}
		}

		if(hasMallenomGroup)
		{
			if(sb.Length != 0) sb.Append(newlineSymbol);
			foreach(var @using in mallenomGroup)
			{
				sb.Append(@using + newlineSymbol);
			}
		}

		if(hasEyecontGroup)
		{
			if(sb.Length != 0) sb.Append(newlineSymbol);
			foreach(var @using in eyecontGroup)
			{
				sb.Append(@using + newlineSymbol);
			}
		}

		sb.Append(newlineSymbol);
		var result = sb.ToString();
		return result;
	}
}
