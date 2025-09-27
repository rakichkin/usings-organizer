using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UsingsOrganizer.Core.UsingsExtraction;

public class RegexBasedUsingSectionsExtractor : IUsingsSectionExtractor
{
	public static Regex UsingRegex { get; } = new(
		 @"(?<=^|;)\s*(?:global\s+)?using\s+(?:static\s+)?(?:(?<alias>[A-Za-z_]\w*)\s*=\s*)?(?<name>[^;/]+?)\s*;",
		RegexOptions.Compiled | RegexOptions.Multiline);

	public IReadOnlyList<UsingSection> Extract(string sourceCodeText)
	{
		var lines = sourceCodeText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

		List<UsingSection> sections = [];

		int? startSectionIdx = null;
		List<string>? usingsInSection = null;
		for(int i = 0; i < lines.Length; i++)
		{
			var match = UsingRegex.Match(lines[i]);
			if(match.Success)
			{
				usingsInSection ??= [];
				startSectionIdx ??= i;
				
				usingsInSection.Add(match.Value.Trim());

				while(match.NextMatch().Success) // на случай, если в одной строке несколько using-ов
				{
					match = match.NextMatch();
					usingsInSection.Add(match.Value.Trim());
				}
			}
			else
			{
				if(startSectionIdx != null)
				{
					sections.Add(new UsingSection
					{
						StartLinePosition = startSectionIdx.Value,
						EndLinePosition = i - 1,
						Usings = usingsInSection!
					});

					usingsInSection = null;
					startSectionIdx = null;
				}
			}
		}

		return sections;
	}
}
