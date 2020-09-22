using UnityEditor;
using System;
using UITools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(FlexibleUIButton))]
public class FlexibleUIButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        //base.OnInspectorGUI();
        var myScript = target as FlexibleUIButton;
        var allSelectedScripts = targets;
        
        
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", myScript, typeof(FlexibleUIButton), false);
        GUI.enabled = true;

        EditorGUILayout.LabelField("Button Config", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        var boolValue = EditorGUILayout.Toggle("Interactable", myScript.interactable);
        if (EditorGUI.EndChangeCheck())
        {
            foreach (var script in allSelectedScripts)
            {
                ((FlexibleUIButton) script).interactable = boolValue;
            }
        }
        
        EditorGUI.BeginChangeCheck();
        var typeValue =  (FlexibleUIData.BUTTON_TYPES) EditorGUILayout.EnumPopup("Button Type", myScript.buttonTypes);
        if (EditorGUI.EndChangeCheck())
        {
            foreach (var script in allSelectedScripts)
            {
                ((FlexibleUIButton) script).buttonTypes = typeValue;
            }
        }
        
        if (myScript.buttonTypes != FlexibleUIData.BUTTON_TYPES.None)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            var modeValue = (ButtonMode) EditorGUILayout.EnumPopup("Button Mode", myScript.buttonMode);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var script in allSelectedScripts)
                {
                    ((FlexibleUIButton) script).buttonMode = modeValue;
                }
            }
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.LabelField("Button Tween", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        boolValue = EditorGUILayout.Toggle("Has Press Tween", myScript.hasPressTween);
        if (EditorGUI.EndChangeCheck())
        {
            foreach (var script in allSelectedScripts)
            {
                ((FlexibleUIButton) script).hasPressTween = boolValue;
            }
        }
        
        if (myScript.hasPressTween)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            var vectorValue = EditorGUILayout.Vector3Field("Pressed Scale", myScript.pressed);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var script in allSelectedScripts)
                {
                    ((FlexibleUIButton) script).pressed = vectorValue;
                }
            }
            
            EditorGUI.BeginChangeCheck();
            var floatValue = EditorGUILayout.FloatField("Tween Duration", myScript.duration);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var script in allSelectedScripts)
                {
                    ((FlexibleUIButton) script).duration = floatValue;
                }
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        EditorGUILayout.LabelField("Button Sound", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        boolValue = EditorGUILayout.Toggle("Has Sound", myScript.hasSound);
        if (EditorGUI.EndChangeCheck())
        {
            foreach (var script in allSelectedScripts)
            {
                ((FlexibleUIButton) script).hasSound = boolValue;
            }
        }
        
        if (myScript.hasSound)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            var soundValue = (SoundTapType) EditorGUILayout.EnumPopup("Sound Type", myScript.soundTapType);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var script in allSelectedScripts)
                {
                    ((FlexibleUIButton) script).soundTapType = soundValue;
                }
            }
            EditorGUI.indentLevel--;
            
        }
        
        EditorGUILayout.Space();

        SerializedProperty onClick = serializedObject.FindProperty("m_OnClickEvent");
        EditorGUILayout.PropertyField(onClick);
        serializedObject.ApplyModifiedProperties();

#if UNITY_EDITOR
        if (!GUI.changed) return;
        myScript.OnValidate();
        if(!Application.isPlaying) EditorSceneManager.MarkSceneDirty(myScript.gameObject.scene);
#endif
    }
}
