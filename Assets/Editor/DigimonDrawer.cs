using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Digimon))]
public class DigimonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight + 2;

        //DigimonBase 드롭다운
        var digimonIDProperty = property.FindPropertyRelative("digimonID");
        string[] digimonNames = DigimonTable.Instance.Digimons.Select(d => d.DigimonName).ToArray();
        int[] digimonIDs = DigimonTable.Instance.Digimons.Select(d => d.DigimonNum).ToArray();
        int selectedIndex = Mathf.Max(0, System.Array.IndexOf(digimonIDs, digimonIDProperty.intValue));

        selectedIndex = EditorGUI.Popup(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            "Digimon Base", selectedIndex, digimonNames);

        digimonIDProperty.intValue = digimonIDs[selectedIndex];

        //그외
        position.y += lineHeight;

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), property.FindPropertyRelative("digimonName"));
        position.y += lineHeight;
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), property.FindPropertyRelative("Level"));
        position.y += lineHeight;
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), property.FindPropertyRelative("xAnityBody"));
        position.y += lineHeight;

        var movesProperty = property.FindPropertyRelative("Moves");
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), movesProperty, true);
        
        //배열이 펼쳐져 있는 경우 추가 높이 계산
        if (movesProperty.isExpanded)
        {
            int moveLines = movesProperty.arraySize + 2;
            position.y += moveLines * lineHeight; //배열의 각 요소에 대한 높이 추가
        }

        position.y += lineHeight;

        var ivsProperty = property.FindPropertyRelative("ivs");
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), ivsProperty, true);
        
        if (ivsProperty.isExpanded)
        {
            position.y += (ivsProperty.arraySize + 2) * lineHeight;  // arraySize에 맞춰 높이 조절 (header 포함)
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight + 2;
        var ivsProperty = property.FindPropertyRelative("ivs");
        var movesProperty = property.FindPropertyRelative("Moves");

        //기본 높이에 배열이 펼쳐진 상태에 따른 높이를 추가
        float totalHeight = lineHeight * 6; // 기본 필드 6줄 (DigimonBase 포함)

        // Moves 배열이 펼쳐진 경우 높이 추가
        if (movesProperty.isExpanded)
        {
            totalHeight += (movesProperty.arraySize + 2) * lineHeight;  // 배열의 각 요소에 대한 높이 추가
        }

        // IVs 배열이 펼쳐진 경우 높이 추가
        if (ivsProperty.isExpanded)
        {
            totalHeight += (ivsProperty.arraySize + 2) * lineHeight;  // 배열의 각 요소에 대한 높이 추가
        }

        return totalHeight;
    }
}