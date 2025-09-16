using System;
using System.Collections.Generic;

namespace UsingsOrganizer.Core.UsingsExtraction;

[Obsolete("This extractor doesn't support top level files. Use RegexBasedUsingsSectionExtractor instead.")]
internal class DefaultUsingsExtractor : IUsingsSectionExtractor
{
	public IEnumerable<string> Extract(string sourceCodeText)
	{
		var usingsSectionStart = sourceCodeText.IndexOf("using");
		var usingsSectionEnd = sourceCodeText.IndexOf("namespace");
		if(usingsSectionEnd == -1)
		{
			throw new NotSupportedException("Top-level files are not supported yet.");
		}

		var usingsTextBlock = sourceCodeText[usingsSectionStart..usingsSectionEnd];
		return usingsTextBlock.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
	}
}
