using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIToolEditor : Editor
{

    [MenuItem("GameObject/UITools/Flexible UI/Button", priority = 0)]
    public static void AddButton()
    {
        Create("Button", "UITools/FlexibleUI");
    }
    
    [MenuItem("GameObject/UITools/Flexible UI/Text Label", priority = 1)]
    public static void AddText()
    {
        Create("Text", "UITools/FlexibleUI");
    }
    
    [MenuItem("GameObject/UITools/Flexible UI/Text Mesh", priority = 2)]
    public static void AddTextMesh()
    {
        Create("TextMesh", "UITools/FlexibleUI");
    }
    
    [MenuItem("GameObject/UITools/Layout/AutoFlow Layout", priority = 0)]
    public static void AddAutoFlowLayout()
    {
        Create("AutoFlowLayout", "UITools/Layout");
    }
    
    [MenuItem("GameObject/UITools/Layout/Horizontal Scroll Snap", priority = 1)]
    public static void AddHorizontalScrollSnap()
    {
        Create("HSS", "UITools/Layout");
    }
    
    [MenuItem("GameObject/UITools/Layout/Vertical Scroll Snap", priority = 2)]
    public static void AddVerticalScrollSnap()
    {
        Create("VSS", "UITools/Layout");
    }

    [MenuItem("GameObject/UITools/Recyclable Scroll View", priority = 3)]
    private static void CreateRecyclableScrollView()
    {
        Create("Recyclable Scroll View", "UITools/Layout");
    }
    
    private static GameObject Create(string objectName, string folder)
    {
        GameObject selected = Selection.activeGameObject;

        if (!selected || selected.transform.GetType() != typeof(RectTransform))
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            
            selected = canvas ? canvas.gameObject : InstanciateCanvas();
            
        }

        if (!selected) return null;
        
        GameObject item = Instantiate(Resources.Load<GameObject>(folder + "/" + objectName));
        item.name = objectName;
        item.transform.SetParent(selected.transform, false);
        item.transform.localPosition = Vector3.zero;
        Selection.activeGameObject = item;
        Undo.RegisterCreatedObjectUndo(item, "Created UITool item");

        return item;
    }

    private static GameObject InstanciateCanvas()
    {
        GameObject c = new GameObject();
        Canvas canvas = c.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = c.AddComponent<CanvasScaler>();
        cs.scaleFactor = 1.0f;
        cs.dynamicPixelsPerUnit = 10f;
        GraphicRaycaster gr = c.AddComponent<GraphicRaycaster>();
        c.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
        c.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);
        c.name = "Canvas";

        if (!FindObjectOfType<EventSystem>())
        {
            GameObject ev = new GameObject();
            ev.AddComponent<EventSystem>();
            ev.AddComponent<StandaloneInputModule>();
            ev.name = "EventSystem";
        }
        
        return c;
    }
}
