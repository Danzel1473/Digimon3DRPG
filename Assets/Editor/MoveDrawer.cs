using System.Linq;
using UnityEditor;
using UnityEngine;

// [CustomPropertyDrawer(typeof(Move))]
// public class MoveDrawer : PropertyDrawer
// {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         EditorGUI.BeginProperty(position, label, property);

//         var moveIDProperty = property.FindPropertyRelative("moveBase.moveID");

//         string[] moveNames = MoveTable.Instance.Moves.Select(m => m.MoveName).ToArray();
//         int[] moveIDs = MoveTable.Instance.Moves.Select(m => m.MoveID).ToArray();

//         int selectedIndex = Mathf.Max(0, System.Array.IndexOf(moveIDs, moveIDProperty.intValue));

//         selectedIndex = EditorGUI.Popup(
//             new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
//             "Move", selectedIndex, moveNames
//         );

//         // 선택된 moveID 업데이트
//         moveIDProperty.intValue = moveIDs[selectedIndex];

//         EditorGUI.EndProperty();
//     }
// }

[CustomPropertyDrawer(typeof(Move))]
public class MoveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // moveNum 속성
        var moveNumProperty = property.FindPropertyRelative("moveNum");

        // MoveTable에서 Move 목록 가져오기
        string[] moveNames = MoveTable.Instance.Moves.Select(m => m.MoveName).ToArray();
        int[] moveIDs = MoveTable.Instance.Moves.Select(m => m.MoveID).ToArray();

        // 현재 moveNum에 맞는 선택된 인덱스 찾기
        int selectedIndex = Mathf.Max(0, System.Array.IndexOf(moveIDs, moveNumProperty.intValue));

        // 드롭다운 표시
        selectedIndex = EditorGUI.Popup(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            "Move", selectedIndex, moveNames
        );

        // 선택된 moveNum 업데이트
        moveNumProperty.intValue = moveIDs[selectedIndex];

        EditorGUI.EndProperty();
    }
}