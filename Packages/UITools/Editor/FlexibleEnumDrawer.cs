using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UITools
{
    [CustomPropertyDrawer(typeof(FlexibleEnum))]
    public class FlexibleEnumDrawer : PropertyDrawer
    {
        private string[] _enumChoices;

        private string[] EnumChoices
        {
            get
            {
                if(_enumChoices == null)
                {
                    var list = FindEnum();
                    list.Insert(0, "None");
                    _enumChoices = list.ToArray();
                }

                return _enumChoices;
            }
        }

        private int _enumValueIndex;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            _enumValueIndex = property.FindPropertyRelative("flexibleEnumType").enumValueIndex;

            var propertyString = property.FindPropertyRelative("enumSelected").stringValue;
            var choiceIndex = Array.IndexOf(EnumChoices, propertyString ?? "None");

            if (choiceIndex < 0) choiceIndex = 0;

            EditorGUI.BeginProperty(position, label, property);
            
            choiceIndex = EditorGUI.Popup(position,"Flexible Type", choiceIndex, EnumChoices);
            property.FindPropertyRelative("enumSelected").stringValue = EnumChoices[choiceIndex];
            property.serializedObject.ApplyModifiedProperties();
            
            EditorGUI.EndProperty();
        }

        private List<string> FindEnum()
        {
            var flexibleData = Resources.Load("FlexibleUIData") as FlexibleUIData;

            var enumChoices = new List<string>();
            
            switch ((FlexibleEnum.FlexibleEnumTypes)_enumValueIndex)
            {
                case FlexibleEnum.FlexibleEnumTypes.Text:
                    foreach (var t in flexibleData.flexibleUIText)
                    {
                        enumChoices.Add(t.name);
                    }
                    break;
                case FlexibleEnum.FlexibleEnumTypes.TextMesh:
                    foreach (var t in flexibleData.flexibleUITextMesh)
                    {
                        enumChoices.Add(t.name);
                    }
                    break;
                case FlexibleEnum.FlexibleEnumTypes.Button:
                    foreach (var t in flexibleData.flexibleUIButtons)
                    {
                        enumChoices.Add(t.name);
                    }
                    break;
                case FlexibleEnum.FlexibleEnumTypes.Toggle:
                    foreach (var t in flexibleData.flexibleUIToggles)
                    {
                        enumChoices.Add(t.name);
                    }
                    break;
            }

            return enumChoices;
        }
    }
}