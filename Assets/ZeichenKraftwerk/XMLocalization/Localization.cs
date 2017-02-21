using UnityEngine;
using System.Collections.Generic;
#if UNITY_WP8 || UNITY_METRO
using System.Xml.Linq;
#else
using System.Xml; // Webplayer uses subset that doesn't know System.Xml.Linq
#endif
using System.IO;
using System;

public class Localization : MonoBehaviour
{
	/// <summary>
	/// The default language.
	/// </summary>
	public string defaultLanguage = "English";
	
	/// <summary>
	/// The directory in resources, where the localization files can be found
	/// </summary>
	public string directoryInResources = "localization";
	
	/// <summary>
	/// Should the current language be remembered?
	/// If set to TRUE and one switches the language, it'll be also loaded next time.
	/// If set to FALSE and one switches the language, then the next time the system language will be loaded.
	/// </summary>
	public bool rememberCurrentLanguage = true;
	
	/// <summary>
	/// String to return if now fallback was provided and localization key can't befound
	/// </summary>
	public string unknownKeyErrorString = "ERR:unknown_string";
	
	/// <summary>
	/// The default localization.
	/// </summary>
	private Dictionary<string, string> defaultLocalizationStrings;
	
	/// <summary>
	/// The current localization.
	/// </summary>
	private Dictionary<string, string> currentLocalizationStrings;
	
	/// <summary>
	/// The current loaded language.
	/// </summary>
	private string currentLanguage = "none";
	
	private static Localization _instance;
	public static Localization Instance {
		get {
			if(_instance == null) {
				_instance = ((Localization)FindObjectOfType(typeof(Localization)));
				if(_instance == null) {
					_instance = new GameObject("XMLOCALIZATION").AddComponent<Localization>();
				}
			}
			return _instance;
		}
	}
	

	void Awake ()
	{
		if(_instance != null) {
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(Instance.gameObject);
		
		// Load default language strings
		defaultLocalizationStrings = loadLocalizedStrings (defaultLanguage);
		
		// Load strings for current language
		string systemLanguage = Enum.GetName (typeof(SystemLanguage), Application.systemLanguage);
		string languageToLoad = systemLanguage;
		if(rememberCurrentLanguage) {
			string langLoadedLastTime = getLanguageLoadedLastTime();
			if(!langLoadedLastTime.Equals("none")) {
				languageToLoad = langLoadedLastTime;
			}
		}
		
		if(!switchLanguage(languageToLoad)) {
			switchLanguage(defaultLanguage);
		}
	}
	
	/// <summary>
	/// Switchs the language.
	/// </summary>
	/// <returns>
	/// TRUE if language is available and switch worked
	/// </returns>
	/// <param name='language'>
	/// Name of language to load (e.g. English)
	/// </param>
	public bool switchLanguage(string language) {
		Dictionary<string, string> newStrings = loadLocalizedStrings(language);
		
		if(newStrings == null) {
			return false;
		} 
		
		// Language found? -> take over to current localization+
		currentLanguage = language;
		
		if(rememberCurrentLanguage) {
			setLanguageLoadedLastTime(language);
		}
		
		currentLocalizationStrings = newStrings;
		
		return true;
	}
	
	/// <summary>
	/// Loads the localized strings.
	/// </summary>
	/// <param name='language'>
	/// Language.
	/// </param>
	private Dictionary<string, string> loadLocalizedStrings (string language)
	{
		if(language.Equals(defaultLanguage) && defaultLocalizationStrings != null) {
			return defaultLocalizationStrings;
		}
		
		// Try to open the file
		TextAsset text = (TextAsset)Resources.Load (directoryInResources + "/" + language, typeof(TextAsset));
		
		// File found?
		if (text != null) {
			// Create target dictionary
			Dictionary<string, string> dict = new Dictionary<string, string> ();
			
			// Read XML
#if UNITY_WP8 || UNITY_METRO
            XDocument languageXml = XDocument.Parse(text.text);

            // Read string
            foreach (XElement n in languageXml.Descendants("string"))
            {
				dict.Add(n.Attribute("name").Value, n.Value);
			}
#else
            // WEBPLAYER doesn't know System.Xml.Linq
            XmlTextReader xmlReader = new XmlTextReader (new StringReader (text.text));
			XmlDocument doc = new XmlDocument ();
			doc.Load (xmlReader);

            // Read string
            foreach (XmlNode n in doc.GetElementsByTagName("string"))
            {
                dict.Add(n.Attributes["name"].Value, n.InnerText);
            }

            // Close reader
            xmlReader.Close ();
#endif

			return dict;
		}
		
		return null;
	}
	
	/// <summary>
	/// Gets the localized string for the specified key.
	/// If the key can't be found in current language dict or default language dict, it'll return content of {unknown}
	/// </summary>
	/// <returns>
	/// The localized string or {unknown}
	/// </returns>
	/// <param name='key'>
	/// Localization key
	/// </param>
	public string getString(string key) {
		return getString(key, unknownKeyErrorString);
	}
	
	/// <summary>
	/// Shortcut method for getString(...)
	/// </summary>
	/// <param name='key'>
	///  Localization key
	/// </param>
	public string s(string key) {
		return getString(key);
	}
	
	
	/// <summary>
	/// Gets the localized string for the specified key.
	/// If the key can't be found in current language dict or default language dict, it'll return content of {fallback}
	/// </summary>
	/// <returns>
	/// The localized string or {unknown}
	/// </returns>
	/// <param name='key'>
	/// Localization key
	/// </param>
	/// <param name='fallback'>
	/// String to return, if localization key can't befound
	/// </param>
	public string getString(string key, string fallback) {
		Dictionary<string, string> workingDict = currentLocalizationStrings;
		
		// If there is no xml file for current language, load default one
		if(workingDict == null || workingDict.Count == 0) {
			workingDict = defaultLocalizationStrings;
		}
		
		// If also default one couldn't be found
		if(workingDict == null) {
			return fallback;
		}
		
		// Does current langugae contain the requested key?
		if(workingDict.ContainsKey(key)) {
			return workingDict[key];
		} else if(workingDict != defaultLocalizationStrings && defaultLocalizationStrings.ContainsKey(key)) { // Does default language contain the requested key?
			return defaultLocalizationStrings[key];
		} else {
			return fallback;
		}
		
	}
	
	/// <summary>
	/// Shortcut method for getString(...)
	/// </summary>
	/// <param name='key'>
	///  Localization key
	/// </param>
	/// <param name='fallback'>
	/// String to return, if localization key can't befound
	/// </param>
	public string s(string key, string fallback) {
		return getString(key, fallback);
	}
	
	/// <summary>
	/// Gets the current loaded / active language.
	/// </summary>
	/// <returns>
	/// Returns the current loaded / active language.
	/// </returns>
	public string getCurrentLoadedLanguage() {
		return currentLanguage;
	}
	
	/// <summary>
	/// Gets the language loaded last time.
	/// </summary>
	/// <returns>
	/// The language loaded last time.
	/// </returns>
	private string getLanguageLoadedLastTime() {
		if(PlayerPrefs.HasKey("xmlocalization_language")) {
			return PlayerPrefs.GetString("xmlocalization_language");
		}
		
		return "none";
	}
	
	private void setLanguageLoadedLastTime(string l) {
		PlayerPrefs.SetString("xmlocalization_language", l);
	}
}

