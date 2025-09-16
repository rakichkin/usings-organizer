using System.Collections.Generic;

namespace UsingsOrganizer.Core.UsingsExtraction;

public interface IUsingsSectionExtractor
{
	IEnumerable<string> Extract(string sourceCodeText);
}
