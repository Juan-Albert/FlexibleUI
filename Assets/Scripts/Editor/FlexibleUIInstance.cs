using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlexibleUIInstance : Editor
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
    
    [MenuItem("GameObject/UITools/Scroll View/Vertical", priority = 2)]
    public static void AddTopScrollView()
    {
        Create("VerticalScrollView", "UITools/ScrollView");
    }
    
    [MenuItem("GameObject/UITools/Scroll View/Horizontal", priority = 3)]
    public static void AddRightScrollView()
    {
        Create("HorizontalScrollView", "UITools/ScrollView");
    }
    
    [MenuItem("GameObject/UITools/Scroll View/Grid", priority = 4)]
    public static void AddGridScrollView()
    {
        Create("GridScrollView", "UITools/ScrollView");
    }
    
    [MenuItem("GameObject/UITools/Layout/AutoFlowLayout", priority = 5)]
    public static void AddAutoFlowLayout()
    {
        Create("AutoFlowLayout", "UITools/Layout");
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
