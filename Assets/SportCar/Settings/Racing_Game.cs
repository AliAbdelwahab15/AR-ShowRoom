#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup
{
    static Startup()    
    {
        EditorPrefs.SetInt("showCounts_sportcarf123", EditorPrefs.GetInt("showCounts_sportcarf123") + 1);

        if (EditorPrefs.GetInt("showCounts_sportcarf123") == 1)       
        {
            Application.OpenURL("https://u3d.as/3xgn");
            // System.IO.File.Delete("Assets/SportCar/Racing_Game.cs");
        }
    }     
}
#endif
