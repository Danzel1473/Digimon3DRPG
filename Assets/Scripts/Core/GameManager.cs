using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] public PlayerData playerData;
    [SerializeField] public PlayerData enemyData;
    [SerializeField] public PopupMenu popupMenu;
    private bool isBattle;

    public IEnumerator WaitForKeyPress(KeyCode key)
    {
        while(!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }
}