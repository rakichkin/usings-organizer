using System.Collections.Generic;

namespace UsingsOrganizer;

/// <summary>Компаратор для сравнения строк, представляющих подключенные пространства имён в .cs-файле.</summary>
/// <remarks>
/// Извлекает из строки формата `using <![CDATA[<]]>Namespace.Name<![CDATA[>]]>;` пространство имён и подает его на сравнение. 
/// При сравнении используется <see cref="StringComparison.InvariantCulture"/>.
/// </remarks>
public class UsingComparer : IComparer<string>
{
	/// <inheritdoc/>
	public int Compare(string x, string y)
	{
		var xNamespace = ExtractNamespace(x);
		var yNamespace = ExtractNamespace(y);
		return string.Compare(xNamespace, yNamespace, StringComparison.InvariantCulture);
	}

	private string ExtractNamespace(string usingStatement)
		=> usingStatement.Replace("using ", "").Replace(";", "").Trim();
}
