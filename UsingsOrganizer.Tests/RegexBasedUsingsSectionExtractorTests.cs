using System.Text;
using NUnit.Framework;
using UsingsOrganizer.Core.UsingsExtraction;

namespace UsingsOrganizer.Tests;

[TestFixture]
public class RegexBasedUsingsSectionExtractorTests
{
    private RegexBasedUsingsSectionExtractor _extractor = null!;

    [SetUp]
    public void SetUp()
    {
        _extractor = new RegexBasedUsingsSectionExtractor();
    }

    [Test]
    public void Extract_SimpleUsings_ShouldReturnAll()
    {
        const string code = "using System;\r\nusing System.IO;\r\nnamespace A { class B {} }";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"using System;","using System.IO;"}));
    }

    [Test]
    public void Extract_IndentedUsings_ShouldMatch()
    {
        const string code = "    using System;\n\tusing System.Text;\nclass C {}";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"using System;","using System.Text;"}));
    }

    [Test]
    public void Extract_GlobalUsing_ShouldMatch()
    {
        const string code = "global using System;\nnamespace N { }";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"global using System;"}));
    }

    [Test]
    public void Extract_UsingAlias_ShouldMatch()
    {
        const string code = "using IO = System.IO;\nusing G = global::System;";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"using IO = System.IO;","using G = global::System;"}));
    }

    [Test]
    public void Extract_StaticUsing_ShouldMatch()
    {
        const string code = "using static System.Math;\nclass C{}";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result.Single(), Is.EqualTo("using static System.Math;"));
    }

    [Test]
    public void Extract_TrailingComment_ShouldMatch()
    {
        const string code = "using System; // core lib\nusing System.Text; // text";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"using System; // core lib","using System.Text; // text"}));
    }

    [Test]
    public void Extract_DoesNotMatchInsideStringLiteral()
    {
        const string code = "var s = \"using System;\";\n// using Commented.Out;\n/* using Also.Commented; */";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Extract_DoesNotMatchWithoutSemicolon()
    {
        const string code = "using System\nusing System.IO\nnamespace N {}";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Extract_EmptyInput_ShouldReturnEmpty()
    {
        var result = _extractor.Extract(string.Empty).ToList();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Extract_MixedValidAndInvalid_ShouldReturnOnlyValid()
    {
        const string code = "using System;\nusing Something\nusing Another.Valid;\nusing Bad Line;\nusing X; // ok\n";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"using System;","using Another.Valid;","using X; // ok"}));
    }

    [Test]
    public void Extract_GlobalStaticAliasVariants()
    {
        const string code = "global using static System.Math;\nusing X = System.Text.StringBuilder;\n";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.EqualTo(new[]{"global using static System.Math;","using X = System.Text.StringBuilder;"}));
    }

    [Test]
    public void Extract_DoesNotOverMatchBeyondLine()
    {
        const string code = "using System; extra text\nusing System.Text;\n";
        var result = _extractor.Extract(code).ToList();
        // First line invalid due to extra text after semicolon? Pattern allows trailing comment only, so it should NOT match.
        Assert.That(result, Is.EqualTo(new[]{"using System.Text;"}));
    }

    [Test]
    public void Extract_DoesNotMatchCommentedOutUsings()
    {
        const string code = @"// using System;\n/* using System.Text; */\n";
        var result = _extractor.Extract(code).ToList();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Extract_HandlesManyBlankLinesBetweenUsings()
    {
        var sb = new StringBuilder();
        sb.AppendLine("using A;");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("using B;");
        var result = _extractor.Extract(sb.ToString()).ToList();
        Assert.That(result, Is.EqualTo(new[]{"using A;","using B;"}));
    }
}
