namespace UsingsOrganizer;

public enum LinesDelimiters
{
	CR,
	LF,
	CRLF
}

public record UsingsOrganizerOptions
{
	public LinesDelimiters LinesDelimiters { get; set; } = LinesDelimiters.CRLF;

	public UsingsOrganizerOptions()
	{

	}

	public UsingsOrganizerOptions(LinesDelimiters linesDelimiter)
	{
		LinesDelimiters = linesDelimiter;
	}
}
