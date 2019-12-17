using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlexibleUIInstance : Editor
{
    [MenuItem("GameObject/Flexible UI/Button", priority = 0)]
    public static void AddButton()
    {
        Create("Button", "FlexibleUIPrefabs");
    }
    
    [MenuItem("GameObject/Flexible UI/Text Label", priority = 1)]
    public static void AddText()
    {
        Create("Text", "FlexibleUIPrefabs");
    }
    
    [MenuItem("GameObject/Flexible UI/Panel", priority = 2)]
    public static void AddPanel()
    {
        Create("Panel", "FlexibleUIPrefabs");
    }
    
    [MenuItem("GameObject/Flexible UI/Scroll View/Vertical", priority = 3)]
    public static void AddTopScrollView()
    {
        Create("VerticalScrollView", "FlexibleUIPrefabs");
    }
    
    [MenuItem("GameObject/Flexible UI/Scroll View/Horizontal", priority = 4)]
    public static void AddRightScrollView()
    {
        Create("HorizontalScrollView", "FlexibleUIPrefabs");
    }
    
    [MenuItem("GameObject/Flexible UI/Scroll View/Grid", priority = 5)]
    public static void AddGridScrollView()
    {
        Create("GridScrollView", "FlexibleUIPrefabs");
    }
    
    private static GameObject clickedObject;

    private static GameObject Create(string objectName, string folder)
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>(folder + "/" + objectName));
        instance.name = objectName;
        clickedObject = UnityEditor.Selection.activeObject as GameObject;

        if (clickedObject != null)
        {
            instance.transform.SetParent(clickedObject.transform, false);
        }
        //PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        return instance;
    }
}
