using UnityEngine;

public class DigimonAI : MonoBehaviour
{
    [SerializeField] Digimon digimon;
    public Rigidbody rigid;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        digimon.Initialize();
    }
    
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("콜리전 충돌");
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag != "Player") return;
        
        StartCoroutine(GameManager.Instance.BattelEnter(digimon));
    }
}