using System.Collections.Generic;

namespace UsingsOrganizer.Core;

public sealed record UsingSection
{
	public required int StartLinePosition { get; init; }

	public required int EndLinePosition { get; init; }

	public required IReadOnlyList<string> Usings { get; init; }
}
