using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UsingsOrganizer.Core.UsingsExtraction;

public class RegexBasedUsingsSectionExtractor : IUsingsSectionExtractor
{
	public static Regex UsingSectionRegex { get; } = new(
		@"^(?:global\s+)?using\s+(?:static\s+)?[\w\.]+(?:\s*=\s*[\w\.]+)?;\s*$",
		RegexOptions.Compiled | RegexOptions.Multiline);

	//public static Regex UsingSectionRegex { get; } = new(
	//	@"^\s*(?:global\s+)?using\b[^\r\n]*?;(?:\s*//.*)?\s*$",
	//	RegexOptions.Compiled | RegexOptions.Multiline);

	public IEnumerable<string> Extract(string sourceCodeText)
	{
		var regexMatches = UsingSectionRegex.Matches(sourceCodeText);
		foreach(Match match in regexMatches)
		{
			yield return match.Value.Trim();
		}
	}
}
