using System.Collections.Generic;

namespace UsingsOrganizer.Core.UsingsExtraction;

public interface IUsingsSectionExtractor
{
	IReadOnlyList<UsingSection> Extract(string sourceCodeText);
}
