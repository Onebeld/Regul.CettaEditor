using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using Regul.Base;
using Regul.ModuleSystem;
using Regul.ModuleSystem.Models;
using System;
using System.Collections.Generic;

namespace Regul.CettaEditor
{
	public class CettaEditorModule : IModule
	{
		private IStyle _language;

		public IImage Icon { get; set; } = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri("avares://Regul.CettaEditor/icon.png")));
		public string Name => "Cetta Editor";
		public string Creator => "Onebeld";
		public string Description => "(ALPHA) Text file editor";
		public string Version => "0.1.0.0";

		public bool CorrectlyInitialized { get; set; }
		public bool ThereIsAnUpdate { get; set; }

		public const string CettaEditorId = "Onebeld_Editor_CettaEditor_8247GvAqwY5";

		public void Execute()
		{
			ModuleManager.Editors.Add(
				new Editor
				{
					Id = CettaEditorId,
					Name = "Cetta Editor",
					CreatingAnEditor = () => new Views.CettaEditor(),
					Icon = App.GetResource<PathGeometry>("CettaEditorIcon"),
					DialogFilters = new List<FileDialogFilter>
					{
						new FileDialogFilter {Name = "Text-files", Extensions = {"txt", "xml", "md", "cs", "axaml", "xaml", "csproj", "h", "cfg", "ini", "sgr", "py", "cpp"}}
					}
				});
		}

		public Language[] Languages { get; } = new Language[]
		{
			new Language("English", "en"),
			new Language("Russian", "ru"),
		};
		public IStyle LanguageStyle { get; set; }
		public string PathToLocalization { get; } = "Regul.CettaEditor/Localization/";
		public string LinkForCheckUpdates { get; }
	}
}
