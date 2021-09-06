using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.Serializable]
public class OffsetSortingOrderWindow : EditorWindow
{
    [SerializeField]
    Transform selectedObject;

    [SerializeField]
    Transform[] children;
    [SerializeField]
    List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    [SerializeField]
    static int offsetValue;

    [SerializeField]
    static Color colorPicker = Color.white;

    [SerializeField]
    static Material materialPicker;

    int selectedTab = 0;

    [MenuItem("Window/Custom Tools/Sprite Renderer Tools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(OffsetSortingOrderWindow));
    }

    private void OnInspectorUpdate()
    {
        GetSelectedObject();
        Repaint();
    }

    private void OnGUI()
    {
        CreateWindowTabs();
        UpdateWindowInfo();
    }

    void CreateWindowTabs()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, new string[] { "Offset Sorting", "Tint Renderers", "Change Material" });
    }

    void GetSelectedObject()
    {
        if (renderers != null)
            renderers.Clear();

        selectedObject = Selection.activeTransform;

        if (selectedObject != null)
        {
            if (selectedObject.childCount > 0)
            {
               children = new Transform[selectedObject.childCount];
               int i = 0;
               foreach (Transform child in selectedObject)
               {
                   children[i] = child;
                   i++;
               }
            }
        }
    }

    void UpdateWindowInfo()
    {
        if (selectedObject != null)
        {
            GUILayout.Space(5);
            GUI.enabled = false;
            GameObject obj = (GameObject)EditorGUILayout.ObjectField(selectedObject.gameObject, typeof(GameObject), true);
            GUI.enabled = true;
            GUILayout.Space(5);
            if (selectedObject.childCount == 0)
            {
                EditorGUILayout.HelpBox("This object doesn't have children.", MessageType.Info);
            }
            else
            {
                if (!GetSpriteRenderers())
                {
                    EditorGUILayout.HelpBox("There are no Sprite Renderers under this Parent", MessageType.Info);
                }
                else
                {
                    DrawFunctionality();
                }

            }
        }
        else
        {
            EditorGUILayout.HelpBox("Select an object in the scene.", MessageType.Info);
        }
    }

    bool GetSpriteRenderers()
    {
        foreach (Transform child in children)
        {
            SpriteRenderer spr = child.GetComponent<SpriteRenderer>();
            if (spr != null)
            {
                if (renderers.Count == 0)
                    renderers.Add(spr);
                else if (!renderers.Contains(spr))
                    renderers.Add(spr);
            }
        }

        // if (renderers == null)
        //     return false;
        // else
        return renderers.Count > 0 ? true : false;
    }

    void DrawFunctionality()
    {

        if (selectedTab == 0)
        { //offset sorting window
            DrawSortingWindow();
        }
        else
        if (selectedTab == 1)
        { //tint renderers window
            DrawTintWindow();
        }
        else
        if (selectedTab == 2)
        { //materials window
            DrawMaterialWindow();
        }
    }

    Vector2 scrollPos;
    void DrawSortingWindow()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        GUIStyle styleLeft = new GUIStyle();
        styleLeft.normal.textColor = Color.gray;
        styleLeft.fontSize = 10;

        GUIStyle styleRight = new GUIStyle(styleLeft);
        styleRight.alignment = TextAnchor.MiddleRight;
        styleRight.normal.textColor = Color.black;

        EditorGUILayout.LabelField("Renderers", styleLeft, GUILayout.MaxWidth(130));
        GUILayout.Space(5);
        foreach (SpriteRenderer r in renderers)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(r.name, styleLeft, GUILayout.MinWidth(50), GUILayout.MaxWidth(Screen.width * 0.75f));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(r.sortingOrder.ToString(), styleRight, GUILayout.MaxWidth(Screen.width * 0.25f)); //, GUILayout.MaxWidth(20)
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        EditorGUILayout.EndScrollView();
        GUILayout.Space(5);

        offsetValue = EditorGUILayout.IntField("Order Offset: ", offsetValue);

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply"))
        {
            ApplyOffset(offsetValue);
        }

        if (GUILayout.Button("Reorder Siblings"))
        {
            ReorderSiblings();
        }
        EditorGUILayout.EndHorizontal();
    }

    void ReorderSiblings()
    {
        renderers.Sort(SortBySortingOrder);

        int i = 0;
        foreach (SpriteRenderer r in renderers)
        {
            r.transform.SetSiblingIndex(i);
            i++;
        }
    }

    static int SortBySortingOrder(SpriteRenderer spr1, SpriteRenderer spr2)
    {
        return spr1.sortingOrder.CompareTo(spr2.sortingOrder);
    }

    void DrawTintWindow()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        GUIStyle styleLeft = new GUIStyle();
        styleLeft.normal.textColor = Color.gray;
        styleLeft.fontSize = 10;

        GUIStyle styleRight = new GUIStyle(styleLeft);
        styleRight.alignment = TextAnchor.MiddleRight;
        styleRight.normal.textColor = Color.black;

        EditorGUILayout.LabelField("Renderers", styleLeft, GUILayout.MaxWidth(130));
        GUILayout.Space(5);

        foreach (SpriteRenderer r in renderers)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(r.name, styleLeft, GUILayout.MinWidth(50), GUILayout.MaxWidth(Screen.width * 0.75f));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUI.enabled = false;
            EditorGUILayout.ColorField(r.color, GUILayout.MaxWidth(Screen.width * 0.25f)); //, GUILayout.MaxWidth(20)
            GUI.enabled = true;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        EditorGUILayout.EndScrollView();
        GUILayout.Space(5);

        colorPicker = EditorGUILayout.ColorField("Color: ", colorPicker);

        if (GUILayout.Button("Apply"))
        {
            ApplyTint(colorPicker);
        }
    }

    void DrawMaterialWindow()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        GUIStyle styleLeft = new GUIStyle();
        styleLeft.normal.textColor = Color.gray;
        styleLeft.fontSize = 10;

        GUIStyle styleRight = new GUIStyle(styleLeft);
        styleRight.alignment = TextAnchor.MiddleRight;
        styleRight.normal.textColor = Color.black;

        EditorGUILayout.LabelField("Renderers", styleLeft, GUILayout.MaxWidth(130));
        GUILayout.Space(5);

        foreach (SpriteRenderer r in renderers)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField(r.name, styleLeft, GUILayout.MinWidth(50), GUILayout.MaxWidth(Screen.width * 0.75f));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUI.enabled = false;
            EditorGUILayout.ObjectField(r.sharedMaterial, typeof(Material), true); //, GUILayout.MaxWidth(20)
            GUI.enabled = true;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        EditorGUILayout.EndScrollView();
        GUILayout.Space(5);

        materialPicker = (Material)EditorGUILayout.ObjectField(materialPicker, typeof(Material),false);

        if (GUILayout.Button("Apply"))
        {
            ApplyMaterials(materialPicker);
        }
    }

    void ApplyOffset(int value)
    {
        foreach (SpriteRenderer r in renderers)
        {
            r.sortingOrder += value;
            EditorUtility.SetDirty(r);
        }
    }

    void ApplyTint(Color value)
    {
        foreach (SpriteRenderer r in renderers)
        {
            r.color = value;
            EditorUtility.SetDirty(r);
        }
    }

    void ApplyMaterials(Material value)
    {
        foreach (SpriteRenderer r in renderers)
        {
            r.material = value;
            EditorUtility.SetDirty(r);
        }
    }

}
