using UnityEngine;
using System.Collections;

public class XMLocalizationExampleGUI : MonoBehaviour
{
	float width = 300;
	float height = 400;
	
	void OnGUI ()
	{
		GUI.Window (1, new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height), windowFunc, "XMLocalization Example");
	}
			
	void windowFunc (int id)
	{
		if (id == 1) {
			string currentLang = Localization.Instance.getCurrentLoadedLanguage ();
			GUI.Label (new Rect (5, 20, width, 30), "Current active language: " + currentLang);
			
			GUI.Label (new Rect (5, 60, width, 30), "Switch language: ");
			
			if (GUI.Button (new Rect (5, 90, 80, 40), "English")) {
				Localization.Instance.switchLanguage ("English");
			}
			
			if (GUI.Button (new Rect (90, 90, 80, 40), "German")) {
				Localization.Instance.switchLanguage ("German");
			}
			
			if (GUI.Button (new Rect (175, 90, 80, 40), "Italian")) {
				Localization.Instance.switchLanguage ("Italian");
			}
			
			GUI.Label (new Rect (5, 150, width, 30), "Usages:");
			GUI.Label (new Rect (5, 185, width, 30), "'points': " + Localization.Instance.s("points"));
			GUI.Label (new Rect (5, 220, width, 30), "'coins': " + Localization.Instance.s("coins"));
			GUI.Label (new Rect (5, 255, width - 10, 50), "'tutorial_step_1': " + Localization.Instance.s("tutorial_step_1"));
			GUI.Label (new Rect (5, 290, width - 10, 50), "'only_english_string_no_fallback': " + Localization.Instance.s("only_english_string_no_fallback"));
			GUI.Label (new Rect (5, 330, width - 10, 50), "'unknown_string_but_fallback': " + Localization.Instance.s("unknown_string_but_fallback", "Custom Fallback"));
			GUI.Label (new Rect (5, 355, width - 10, 50), "'unknown_string_no_fallback': " + Localization.Instance.s("unknown_string_no_fallback"));
		}
	}
}
