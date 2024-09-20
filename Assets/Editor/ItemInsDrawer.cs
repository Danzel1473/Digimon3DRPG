using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemInstance))]
public class ItemInsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var itemNumProperty = property.FindPropertyRelative("itemId");

        string[] itemNames = ItemTable.Instance.Items.Select(i => i.Name).ToArray();
        int[] itemIDs = ItemTable.Instance.Items.Select(i => i.ItemId).ToArray();

        int selectedIndex = Mathf.Max(0, System.Array.IndexOf(itemIDs, itemNumProperty.intValue));

        selectedIndex = EditorGUI.Popup(
            new Rect(position.x, position.y, position.width * 0.6f, EditorGUIUtility.singleLineHeight),
            "Item", selectedIndex, itemNames);

        itemNumProperty.intValue = itemIDs[selectedIndex];

        var quantityProperty = property.FindPropertyRelative("quantity");
        
        quantityProperty.intValue = EditorGUI.IntField(
            new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, EditorGUIUtility.singleLineHeight),
            "Quantity", quantityProperty.intValue
        );

        EditorGUI.EndProperty();
    }
}