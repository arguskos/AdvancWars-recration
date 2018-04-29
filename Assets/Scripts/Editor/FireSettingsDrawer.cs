using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// IngredientDrawer
[CustomPropertyDrawer(typeof(DamageInfo))]
public class DamageInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect rect = position;
        rect.width /= 2;
        EditorGUI.LabelField(rect, property.FindPropertyRelative("Name").stringValue);
        rect.position += new Vector2(rect.width, 0);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("BaseDamage"), GUIContent.none);

        //EditorGUI.PropertyField(rect, property.FindPropertyRelative("Name"));
        // EditorGUI.PropertyField(property.FindPropertyRelative("BaseDamage"), new GUIContent("Base Damage", ""));

    }
}

// IngredientDrawer
[CustomPropertyDrawer(typeof(CanFire.FireSettings))]
public class FireSettingsDrawer : PropertyDrawer
{


    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        EditorGUILayout.Space();
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Range");
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Range"), GUIContent.none);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ammo");
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Ammo"), GUIContent.none);
        EditorGUILayout.EndHorizontal();

        SerializedProperty s = property.FindPropertyRelative("DamageToBaseUnitType");
        EditorGUILayout.PropertyField(s, new GUIContent("Damages", ""), true);

        if (GUILayout.Button("Update"))
        {
            Update(property);
        }



        EditorGUI.indentLevel = 0;
        // Calculate rects
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    /// <summary>
    /// This is all super hacky but the point is 
    /// 1. Get Scriptable level object
    /// 2. Get CanFire Setting of every class that has fire settings 
    /// 3. Get list of all unit types 
    /// 4. if unit type does not exist add it 
    /// </summary>
    public void Update(SerializedProperty property)
    {
        SerializedProperty s = property.FindPropertyRelative("DamageToBaseUnitType");
        List<Type> unitsTypes = null;

        foreach (var fieldInScriptableLevelInstaller in s.serializedObject.targetObject.GetType().GetFields())
        {
            var obj = fieldInScriptableLevelInstaller.GetValue(s.serializedObject.targetObject);
            foreach (var fieldofField in fieldInScriptableLevelInstaller.FieldType.GetFields())
            {
                if (fieldInfo == fieldofField)
                {
                    List<DamageInfo> damageInfos = (fieldofField.GetValue(obj) as CanFire.FireSettings).DamageToBaseUnitType;
                    if (unitsTypes == null)
                    {
                       unitsTypes =  GetAllDerrivedClasssesofBaseUnit();
                    }

                    foreach (var unitType in unitsTypes)
                    {
                        bool found = false;
                        foreach (var df in damageInfos)
                        {
                            if (df.Name == unitType.Name)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            damageInfos.Add(new DamageInfo() { Name = unitType.Name });
                        }

                    }
                }
            }
        }
    }

    private  List<Type> GetAllDerrivedClasssesofBaseUnit()
    {
        var assembly = Assembly.GetAssembly(typeof(BaseUnit));
        var derivedType = typeof(BaseUnit);
        var unitsList = assembly.GetTypes()
            .Where(t =>
                t != derivedType &&
                derivedType.IsAssignableFrom(t)
                ).ToList();
        return unitsList;
    }
}