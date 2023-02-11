# SoraEditorBindings
This is a C# binding of https://github.com/Rosemoe/sora-editor enabling the use of Sora Editor in .NET for Android.
No modifications to the base code of the code editor have been made. Currently the bindings include the Editor and TextMate modules.
Theare are only a few custom additions for specific use cases. They are optional and "off" by default.

Refer to the original repository and its "app" module for more detailed guides on using the editor.<br>
Here is a quick tutorial for easily setting it up with TextMate:<br>
Firstly create a folder structure like this in your Android Assets. 
The languages folder here contains a [languages.json](https://github.com/Rosemoe/sora-editor/blob/adef78d89894790d7717cf4c45b879fe2b82a3e7/app/src/main/assets/textmate/languages.json) file.<br>
**Assets/sora_editor/themes<br>**
**Assets/sora_editor/languages<br>**
**Assets/sora_editor/languages/lang_name_here**  (this one might not matter if you use a languages.json, that's where the paths are important) <br>

```csharp
void Init(CodeEditor editor)
{
  FileProviderRegistry.Instance.AddFileProvider(new AssetsFileResolver(editor.Context.Assets));
  InitEditorLanguages();
  InitEditorThemes();
  editor.ColorScheme = TextMateColorScheme.Create(ThemeRegistry.Instance);
  editor.EditorLanguage = TextMateLanguage.Create("source.cs", true);
  ...
}


//Not related to the next 2 methods themselves but a Visual Studio bug.
//In case it isn't fixed yet add something like this in your ".csproj".
//Otherwise the debug builds can't seem to find default methods in Java interfaces and the runtime crashes.
//<SupportedOSPlatformVersion Condition=" '$(Configuration)' == 'Debug' ">24</SupportedOSPlatformVersion>
//<SupportedOSPlatformVersion Condition=" '$(Configuration)' == 'Release' ">21</SupportedOSPlatformVersion>

void InitEditorThemes() 
{
  var assetsRoot = "sora_editor/themes";
  var themes = new string[]
  {
    $"{assetsRoot}/darcula.json" //List your themes here
  };
  
  for (int i=0; i<themes.Length; i++)
  {
    var theme = themes[i];
    var src = IThemeSource.FromInputStream(FileProviderRegistry.Instance.TryGetInputStream(theme), theme, null);
    var model = new ThemeModel(src, theme);
    ThemeRegistry.Instance.LoadTheme(model, i == 0); //First theme is loaded as default
  }
}

void InitEditorLanguages()
{
  GrammarRegistry.Instance.LoadGrammars("sora_editor/languages/languages.json");
}


//The scope here can be for example "source.cs" or "source.java" but some grammars use scopes like "text.xml".
//You might want to write a method which handles that depending on your requirments.
void SetLanguage(CodeEditor editor, string scope)
{
  editor.EditorLanguage.JavaCast<TextMateLanguage>().UpdateLanguage(scope);
}

void SetTheme(CodeEditor editor, string theme)
{ 
  var assetsRoot = "sora_editor/themes";
  if (theme.StartsWith(assetsRoot)) //Enforce full path to the theme. You could change that from the ThemeModel creation.
  {
    theme = $"{assetsRoot}/theme"
  }
  ThemeRegistry.Instance.SetTheme(theme);
}

