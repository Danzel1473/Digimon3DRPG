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
            new Rect(position.x, position.y, position.width * 0.6f, EditorGUIUtility.singleLineHeight),
            "Move", selectedIndex, moveNames
            );
        
        // 선택된 itemNum 업데이트
        moveIDProperty.intValue = moveIDs[selectedIndex];

        // quantity 속성
        var levelProperty = property.FindPropertyRelative("level");
        
        // 수량 표시
        levelProperty.intValue = EditorGUI.IntField(
            new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, EditorGUIUtility.singleLineHeight),
            "Level", levelProperty.intValue
        );

        EditorGUI.EndProperty();
    }
}