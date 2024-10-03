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
    public Vector3 playerPosition = new Vector3();
    public Quaternion playerRotation = new Quaternion();

    public SituState State => state;

    public void Start()
    {
        foreach(Digimon digimon in playerData.partyData.Digimons)
        {
            digimon.Initialize();
        }
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

    public IEnumerator BattelEnter(NPCData data)
    {
        enemyData = data.npcData;
        state = SituState.Battle;
        InterectionUI.Instance.DisableText();

        SavePlayerTransform();

        yield return SceneManager.LoadSceneAsync("BattleScene");
    }

    public IEnumerator BattelExit()
    {
        state = SituState.OpenWorld;

        yield return SceneManager.LoadSceneAsync("SandBox");
    }

    private void SavePlayerTransform()
    {
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        playerRotation = GameObject.FindWithTag("Player").transform.rotation;
    }

    public enum SituState
    {
        OpenWorld,
        Battle,
        InUI
    }
}