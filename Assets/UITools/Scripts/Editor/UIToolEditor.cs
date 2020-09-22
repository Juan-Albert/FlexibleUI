using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    private static void createRecyclableScrollView()
    {
        Create("Recyclable Scroll View", "UITools/Layout");
    }
    
    private static GameObject Create(string objectName, string folder)
    {
        GameObject selected = Selection.activeGameObject;

        if (!selected || selected.transform.GetType() != typeof(RectTransform))
        {
            //todo en modo prefab que se ponga en la raiz del prefab
            selected = FindObjectOfType<Canvas>().gameObject;
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
}
