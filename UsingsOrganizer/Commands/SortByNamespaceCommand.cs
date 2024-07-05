using System.Linq;

namespace UsingsOrganizer.Commands;

[Command(PackageIds.SortByNamespaceCommand)]
internal sealed class SortByNamespaceCommand : BaseCommand<SortByNamespaceCommand>
{
	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
	{
		await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
		var docView = await VS.Documents.GetActiveDocumentViewAsync();
		if(docView.TextView == null || !docView.FilePath.EndsWith(".cs")) return;

		var edit = docView.TextBuffer.CreateEdit();
		var startUsingsBlockPosition = edit.Snapshot.Lines.First(l => l.GetText().Contains("using")).Start.Position;
		var endUsingsBlockPosition = edit.Snapshot.Lines.First(l => l.GetText().Contains("namespace")).Start.Position - 1;
		var usingsBlockText = edit.Snapshot.GetText(startUsingsBlockPosition, endUsingsBlockPosition - startUsingsBlockPosition).Trim();

		var sortedUsingsText = UsingsOrganizer.GetFormattedUsingsBlockText(usingsBlockText);
		edit.Replace(startUsingsBlockPosition, endUsingsBlockPosition - startUsingsBlockPosition, sortedUsingsText);
	}
}
