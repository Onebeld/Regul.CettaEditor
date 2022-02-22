using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaEdit;
using PleasantUI.Controls.Custom;
using Regul.ModuleSystem;

namespace Regul.CettaEditor.Views
{
	public class CettaEditor : UserControl, IEditor
	{
		private CettaEditorViewModel _viewModel;

		public TextEditor TextEditor { get; set; }

		public CettaEditor()
		{
			AvaloniaXamlLoader.Load(this);

			TextEditor = this.FindControl<TextEditor>("PART_AvaloniaEdit");
			TextEditor.Background = Brushes.Transparent;
			TextEditor.ShowLineNumbers = true;
			
			TextEditor.TextArea.RightClickMovesCaret = true;

			AddHandler(PointerWheelChangedEvent, (o, i) =>
			{
				if (i.KeyModifiers != Avalonia.Input.KeyModifiers.Control) return;
				if (i.Delta.Y > 0) TextEditor.FontSize++;
				else TextEditor.FontSize = TextEditor.FontSize > 1 ? TextEditor.FontSize - 1 : 1;
			}, Avalonia.Interactivity.RoutingStrategies.Bubble, true);
		}

		private void DocumentOnTextChanged(object sender, EventArgs e) => _viewModel.IsEdited = true;

		private void CaretOnPositionChanged(object sender, EventArgs e) => _viewModel.GetCaretPosition();

		public string FileToPath { get; set; }
		public Editor CurrentEditor { get; set; }
		public Project CurrentProject { get; set; }
		public string Id { get; set; }

		public void Load(string path, Project project, PleasantTabItem pleasantTabItem, Editor editor)
		{
			CettaEditorViewModel viewModel = path != null
				? new CettaEditorViewModel(pleasantTabItem, editor, this, path, project)
				: new CettaEditorViewModel(pleasantTabItem, editor, this);

			Id = editor.Id;

			DataContext = _viewModel = viewModel;
		}

		public void Release() => _viewModel.Release();

		public bool Save(string path) => _viewModel.Save(path);
	}
}
