using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class RateWindow : EditorWindow
{
    static RateWindow()
    {
        EditorApplication.update += RunOnce;
    }

    static void RunOnce()
    {
        EditorApplication.update -= RunOnce;

        bool dontShow = EditorPrefs.GetBool("DontShowRateWindow");

        if (!Application.isPlaying)
            if (!dontShow)
                Init();
    }

    [MenuItem("Help/Forest Sprite Pack/RateWindow")]
    public static RateWindow Init()
    {
        RateWindow window = EditorWindow.GetWindow<RateWindow>(true, "Thank you!", true);
        return window;
    }


    bool dontShowAgain = false;
    private void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle(EditorStyles.largeLabel);
        titleStyle.wordWrap = true;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperLeft;
        titleStyle.richText = true;

        GUIStyle bodyStyle = new GUIStyle(EditorStyles.label);
        bodyStyle.wordWrap = true;
        bodyStyle.alignment = TextAnchor.UpperLeft;
        bodyStyle.richText = true;

        string titleText = "Hey! Thanks for acquiring the Forest Sprite Pack!";
        string bodyText = "Just wanted to ask you to spare a few seconds to give a rating to the Forest Sprite Pack on the store. It will help tremendously to support further development of the package.\n\n" +
                          "Thanks again! \n";

        GUILayout.Label(titleText, titleStyle);
        GUILayout.Label(bodyText, bodyStyle);


        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();

        Color col = GUI.color;
        GUI.backgroundColor *= Color.green;
        if (GUILayout.Button("Rate Forest Sprite Pack!", GUILayout.Height(30), GUILayout.MaxWidth(300)))
        {
            Application.OpenURL("http://u3d.as/M3U");
        }
        GUI.backgroundColor = col;

        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(50);

        dontShowAgain = GUILayout.Toggle(dontShowAgain, "Do not show this again.");
    }

    public void OnDestroy()
    {
        SavePreference();
    }

    void SavePreference()
    {
        EditorPrefs.SetBool("DontShowRateWindow", dontShowAgain);
    }

}
