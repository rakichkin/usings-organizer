using System.Collections.Generic;

namespace UsingsOrganizer;

public class UsingComparer : IComparer<string>
{
	public int Compare(string x, string y)
	{
		var xNamespace = ExtractNamespace(x);
		var yNamespace = ExtractNamespace(y);
		return string.Compare(xNamespace, yNamespace);
	}

	private string ExtractNamespace(string usingStatement)
		=> usingStatement.Replace("using ", "").Replace(";", "").Trim();
}
