using System.Linq;

namespace UsingsOrganizer.Commands;

/// <summary>Команда для выполнения сортировки и организации секции подключенных пространств имён в .cs-файле.</summary>
[Command(PackageIds.SortByNamespaceCommand)]
internal sealed class SortByNamespaceCommand : BaseCommand<SortByNamespaceCommand>
{
	private readonly UsingComparer _usingStringComparer = new();

	/// <inheritdoc/>
	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
	{
		await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
		var docView = await VS.Documents.GetActiveDocumentViewAsync();
		if(docView.TextView == null || !docView.FilePath.EndsWith(".cs")) return;

		var edit = docView.Document.TextBuffer.CreateEdit();
		var startUsingsBlockPosition = edit.Snapshot.Lines.First(l => l.GetText().Contains("using")).Start.Position;
		var endUsingsBlockPosition = edit.Snapshot.Lines.First(l => l.GetText().Contains("namespace")).Start.Position - 1;
		var usingsBlockText = edit.Snapshot.GetText(startUsingsBlockPosition, endUsingsBlockPosition - startUsingsBlockPosition).Trim();

		var organizer = new UsingsOrganizer(_usingStringComparer);
		var sortedUsingsText = organizer.Organize(usingsBlockText);
		edit.Replace(startUsingsBlockPosition, endUsingsBlockPosition - startUsingsBlockPosition, sortedUsingsText);
		edit.Apply();
	}
}
