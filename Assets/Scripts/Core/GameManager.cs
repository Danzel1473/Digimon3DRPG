using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] public PartyData enemyPartyData;

    private void Awake()
    {
    }
}