namespace UsingsOrganizer.Commands;

/// <summary>Команда для выполнения сортировки и организации секции подключенных пространств имён в .cs-файле.</summary>
[Command(PackageIds.OrganizeUsingsCommand)]
internal sealed class OrganizeUsingsCommand : BaseCommand<OrganizeUsingsCommand>
{
	private readonly UsingComparer _usingStringComparer = new();

	/// <inheritdoc/>
	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
	{
		await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

		var activeDocument = await VS.Documents.GetActiveDocumentViewAsync();
		if(activeDocument.TextView == null || !activeDocument.TextBuffer.ContentType.IsOfType("CSharp")) return;

		var textBuffer = activeDocument.TextBuffer;
		var textSnapshot = textBuffer.CurrentSnapshot;
		var text = textSnapshot.GetText();
		var usingsSectionStart = text.IndexOf("using");
		var usingsSectionEnd = text.IndexOf("namespace") - 1;
		var usingsTextBlock = text[usingsSectionStart.. usingsSectionEnd];
		using var edit = textBuffer.CreateEdit();
		var organizer = new UsingsOrganizer(_usingStringComparer);
		try
		{
			string organizedUsingsBlock = organizer.Organize(usingsTextBlock);
			var textWithOrganizedUsings = text.Replace(usingsTextBlock, organizedUsingsBlock);
			edit.Replace(0, text.Length, textWithOrganizedUsings);
		}
		catch(Exception ex)
		{
			await VS.MessageBox.ShowErrorAsync(
				"An error occurred while organizing usings. See details in the Output Window.");
			await ex.LogAsync();
			edit.Cancel();
			return;
		}
		edit.Apply();
	}
}
