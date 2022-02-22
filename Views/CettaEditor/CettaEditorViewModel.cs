using System;
using System.IO;
using System.Linq;
using Avalonia.Collections;
using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.TextMate;
using AvaloniaEdit.TextMate.Grammars;
using Onebeld.Extensions;
using PleasantUI.Controls.Custom;
using Regul.ModuleSystem;

namespace Regul.CettaEditor.Views
{
	public class CettaEditorViewModel : ViewModelBase
	{
		private AvaloniaList<Language> _languages;
		private Language _selectedLanguage;

		private RegistryOptions _registryOptions;
		private TextMate.Installation _textMateInstallation;

		private TextDocument _textDocument;

		#region Properties

		public bool IsEdited
		{
			get => PleasantTabItem.IsEditedIndicator;
			set
			{
				PleasantTabItem.IsEditedIndicator = value;
				RaisePropertyChanged(nameof(IsEdited));
			}
		}

		private AvaloniaList<Language> Languages
		{
			get => _languages;
			set => RaiseAndSetIfChanged(ref _languages, value);
		}

		private TextDocument TextDocument
		{
			get => _textDocument;
			set => RaiseAndSetIfChanged(ref _textDocument, value);
		}

		private Language SelectedLanguage
		{
			get => _selectedLanguage;
			set
			{
				RaiseAndSetIfChanged(ref _selectedLanguage, value);

				if (SelectedLanguage == null)
					SelectedLanguage = Languages.First(x => x.Aliases[0] == "Ignore");

				CettaEditor.TextEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(value.Aliases[0]);
			}
		}

		private string Line => CettaEditor.TextEditor.TextArea.Caret.Line.ToString();
		private string Column => CettaEditor.TextEditor.TextArea.Caret.Column.ToString();

		public string PathToFile;

		#endregion
		
		public PleasantTabItem PleasantTabItem { get; set; }
		public CettaEditor CettaEditor { get; set; }

		public CettaEditorViewModel(PleasantTabItem pleasantTabItem, Editor editor, CettaEditor cettaEditor, string path = null)
		{
			PleasantTabItem = pleasantTabItem;
			CettaEditor = cettaEditor;
			CettaEditor.CurrentEditor = editor;

			PathToFile = path;
			
			TextDocument = new TextDocument(string.Empty);
			_registryOptions = new RegistryOptions(ThemeName.Light);

			Languages = new AvaloniaList<Language>(_registryOptions.GetAvailableLanguages());

			if (string.IsNullOrEmpty(PathToFile))
			{
				IsEdited = true;
			}
			else
			{
				TextDocument.Text = File.ReadAllText(PathToFile);
				SelectedLanguage = _registryOptions.GetLanguageByExtension(Path.GetExtension(path));
			}
			
			TextDocument.TextChanged += DocumentOnTextChanged;
			CettaEditor.TextEditor.TextArea.Caret.PositionChanged += CaretOnPositionChanged;
		}

		private void CaretOnPositionChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged(nameof(Line));
			RaisePropertyChanged(nameof(Column));
		}

		private void DocumentOnTextChanged(object sender, EventArgs e) => IsEdited = true;

		public CettaEditorViewModel(PleasantTabItem pleasantTabItem, Editor editor, CettaEditor cettaEditor, string path, Project project) : this(pleasantTabItem, editor, cettaEditor, path)
		{

		}

		public bool Save(string path)
		{
			try
			{
				PathToFile = path;

				SaveFile(path);

				IsEdited = false;

				return true;
			}
			catch
			{
				return false;
			}
		}

		public void GetCaretPosition()
		{
			RaisePropertyChanged(nameof(Line));
			RaisePropertyChanged(nameof(Column));
		}

		private bool SaveFile(string path)
		{
			string content = TextDocument.Text;
			
			File.WriteAllText(path, content);
			
			return true;
		}
		public void Release()
		{

		}
	}
}
