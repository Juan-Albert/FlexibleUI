using System.Collections;
using System.Collections.Generic;
using UITools;
using UnityEngine;

public static class UIUtils 
{
    public static T SafeDestroy<T>(T obj) where T : Object
    {
        if (Application.isEditor)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);
     
        return null;
    }
    public static T SafeDestroyGameObject<T>(T component) where T : Component
    {
        if (component != null)
            SafeDestroy(component.gameObject);
        return null;
    }
    
    public static string ModifyText(string text, TextModifier modifier)
    {
        if (!string.IsNullOrEmpty(text))
        {
            if (modifier == TextModifier.None) return text;
            if (modifier == TextModifier.ToLowercase) return text.ToLower();
            if (modifier == TextModifier.ToUppercase) return text.ToUpper();
        }
        return text;
            
    }

    public static Color GetColorWithAlpha(Color currentColor, float alpha)
    {
        return new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}
