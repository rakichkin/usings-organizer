﻿global using System;

global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using Task = System.Threading.Tasks.Task;

using System.Runtime.InteropServices;
using System.Threading;

namespace UsingsOrganizer;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[Guid(PackageGuids.UsingsOrganizerString)]
public sealed class UsingsOrganizerPackage : ToolkitPackage
{
	protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
	{
		await this.RegisterCommandsAsync();
	}
}