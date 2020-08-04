using UnityEditor;
using System;
using UITools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(FlexibleUIButton))]
public class FlexibleUIButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var myScript = target as FlexibleUIButton;
        
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", myScript, typeof(FlexibleUIButton), false);
        GUI.enabled = true;
        
        EditorGUILayout.LabelField("Button Config", EditorStyles.boldLabel);

        myScript.buttonTypes = (FlexibleUIData.BUTTON_TYPES) EditorGUILayout.EnumPopup("Button Type", myScript.buttonTypes);
/*
        bool paint = false;
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.buttonTypes == FlexibleUIData.BUTTON_TYPES.None)))
        {
            if (group.visible == false)
            {
                paint = true;
            }
        }
    */
        if (myScript.buttonTypes != FlexibleUIData.BUTTON_TYPES.None)
        {
            EditorGUI.indentLevel++;
            myScript.buttonMode = (ButtonMode) EditorGUILayout.EnumPopup("Button Mode", myScript.buttonMode);
            myScript.buttonTransition = (Selectable.Transition) EditorGUILayout.EnumPopup("Button Transition", myScript.buttonTransition);
            myScript.imageByColor = EditorGUILayout.Toggle("Image By Color", myScript.imageByColor);
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.LabelField("Button Tween", EditorStyles.boldLabel);

        myScript.hasPressTween = EditorGUILayout.Toggle("Has Press Tween", myScript.hasPressTween);

        if (myScript.hasPressTween)
        {
            EditorGUI.indentLevel++;
            myScript.pressed = EditorGUILayout.Vector3Field("Pressed Scale", myScript.pressed);
            myScript.duration = EditorGUILayout.FloatField("Tween Duration", myScript.duration);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        EditorGUILayout.LabelField("Button Sound", EditorStyles.boldLabel);


        myScript.hasSound = EditorGUILayout.Toggle("Has Sound", myScript.hasSound);

        if (myScript.hasSound)
        {
            EditorGUI.indentLevel++;
            myScript.soundTapType = (SoundTapType) EditorGUILayout.EnumPopup("Sound Type", myScript.soundTapType);
            EditorGUI.indentLevel--;
            
        }
        
        EditorGUILayout.Space();

        SerializedProperty onClick = serializedObject.FindProperty("m_OnClickEvent");
        EditorGUILayout.PropertyField(onClick);
        serializedObject.ApplyModifiedProperties();

        
        if (!GUI.changed) return;
        
        myScript.OnValidate();
        EditorSceneManager.MarkSceneDirty(myScript.gameObject.scene);
    }
}
