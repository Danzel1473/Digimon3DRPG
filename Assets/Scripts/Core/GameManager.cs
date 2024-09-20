using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] public PlayerData playerData;
    [SerializeField] public PlayerData enemyData;
    [SerializeField] public PopupMenu popupMenu;
    [SerializeField] public SituState state = SituState.OpenWorld;

    public SituState State => state;

    public void Start()
    {
        foreach(Digimon digimon in playerData.partyData.Digimons) //테스트용
        {
            digimon.Initialize();
        }
        if(playerData.Inventory.Items[0].item == null) Debug.Log("ㄴㄴ");
        Debug.Log(playerData.Inventory.Items[0].item.Description);
        Debug.Log(playerData.Inventory.Items[1].item.ItemId);
        Debug.Log(playerData.Inventory.Items[0].quantity);
    }
    
    public IEnumerator WaitForKeyPress(KeyCode key)
    {
        while(!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }

    public IEnumerator ScreenFadeOut(float s)
    {
        for(float t = 0; t < s; t++)
        {
            yield return null;
        }
    }

    public void SetSituState(SituState state)
    {
        this.state = state;
    }

    public void PlaySound(AudioClip clip)
    {
        Camera cam = FindObjectOfType<Camera>();
        AudioSource audio = cam.GetComponent<AudioSource>();
        audio.PlayOneShot(clip);
    }

    public enum SituState
    {
        OpenWorld,
        Battle,
        InUI
    }
}