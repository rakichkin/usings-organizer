using System;
using System.Collections.Generic;

namespace UsingsOrganizer.Core;

/// <summary>Компаратор для сравнения строк, представляющих подключенные пространства имён в .cs-файле.</summary>
/// <remarks>
/// Извлекает из строки формата `using <![CDATA[<]]>Namespace.Name<![CDATA[>]]>;` пространство имён и подает его на сравнение. 
/// </remarks>
public class UsingsComparer : IComparer<string>
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
