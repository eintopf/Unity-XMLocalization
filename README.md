# XMLocalization
XMLocalization is a simple localization scripting tool. We decided to make it Open Source (it is also available on the AssetStore - buy it there if you want to support us).

## How to use?
The following steps will take you quickly to your translated project.

### Preparation of localization files
Just drop your localization XML files in the resources/localization folder.

You can add a new localization string by adding a line like this to your language files:
`<string name="localization_key">Your localized string</string>`

Example: 
- Put `<string name="points">Points</string>` in English.xml
- Put `<string name="points">Punkte</string>` in German.xml
- Put `<string name="points">Punti</string>` in Italian.xml

Of course, any number of other languages ​​can be created.
Please use the values ​​from the system language table for the appropriate languages​​:
http://docs.unity3d.com/Documentation/ScriptReference/SystemLanguage.html

### Get localized strings
To get the localized strings, just use `Localization.Instance.s("localization_key")`.
E.g. `Localization.Instance.s("points")`.
It'll return the "points"-string according to the current language.
If a language is loaded which doesn't contain a definition for "points", it'll return the localization of your defaultLanguage (default English).

You can also fit a fallback for the case that the entry in the default language also doesn't exist. 
`Localization.Instance.s("localization_key", "Fallback")`
e.g. `Localization.Instance.s("mainmenu", "Main Menu")`

### Switch language
When you start your project the first time, it is always attempted to load the current system language (http://docs.unity3d.com/Documentation/ScriptReference/SystemLanguage.html).
Sometimes you might want to switch the language (e.g. if you offer a GUI for choosing the language).
Then just call `Localization.Instance.switchLanguage(newLanguage)`
e.g. `Localization.Instance.switchLanguage("German")`

### Advanced settings
XMLocalization runs out of the box.
But if you want to fine-tune some settings just add the XMLocalization-Prefab to your FIRST scene (e.g. Main Menu). 
Then you can set the following settings: 

Default Language: 
Your default language (the script will fallback to this langauge, if the localization key can't be found for the current language) 

Directory in Resources: 
Your XML files are default placed in Resources/localization, you can define another folder for localization, e.g. text/language (-> Resources/text/language)  

Remember current language: 
If you use the switchLanguage-method, then the script will remember the selected language for the next start of the project. 

Unknown Key Error String: 
If a translation key in either the current language file, nor in the default language file can be found, and no fallback was specified in the s-method, then this string is returned.
