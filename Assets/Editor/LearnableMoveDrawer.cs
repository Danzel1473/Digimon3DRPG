using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LearnableMove))]
public class LearnableMoveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var moveIDProperty = property.FindPropertyRelative("moveID");

        string[] moveNames = MoveTable.Instance.Moves.Select(move => move.MoveName).ToArray();
        int[] moveIDs = MoveTable.Instance.Moves.Select(move => move.MoveID).ToArray();

        int selectedIndex = Mathf.Max(0, Array.IndexOf(moveIDs, moveIDProperty.intValue));
        
        selectedIndex = EditorGUI.Popup(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            "Move", selectedIndex, moveNames
        );

        moveIDProperty.intValue = moveIDs[selectedIndex];

        EditorGUI.EndProperty();
    }
}