#nullable enable

namespace UsingsOrganizer;

public class VsExtensionsOutputWindowPaneLogger()
{
	private static string _paneTitle = "Extensions";
	private static Guid _paneGuid = new("1780E60C-EE25-482B-AC77-CBA91891C420");

	private OutputWindowPane? _pane;

	public async Task LogAsync(string message)
	{
		await EnsurePaneAsync();
		await _pane!.WriteLineAsync(message);
	}

	private async Task EnsurePaneAsync()
	{
		_pane ??= await VS.Windows.GetOutputWindowPaneAsync(_paneGuid); 
		_pane ??= await VS.Windows.CreateOutputWindowPaneAsync(_paneTitle);
	}
}
