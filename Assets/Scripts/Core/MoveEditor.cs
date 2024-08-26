using Unity.VisualScripting;
using UnityEditor;

[CustomEditor(typeof(MoveBase))]
public class MoveEditor : Editor
{
    public MoveBase selected;
    private void OnEnable()
    {
        // target은 Editor에 있는 변수로 선택한 오브젝트를 받아옴.
        if (AssetDatabase.Contains(target))
        {
            selected = null;
        }
        else
        {
            // target은 Object형이므로 Enemy로 형변환
            selected = (MoveBase)target;
        }
    }

    public override void OnInspectorGUI()
    {
        if (selected == null)
            return;
 
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("****** 기술 정보 입력 툴 ******");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}