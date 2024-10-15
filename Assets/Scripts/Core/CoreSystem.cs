using System.Collections.Generic;
using UnityEngine;

public class CoreSystem : MonoBehaviour
{
    public static List<string> names = new List<string>();
    public void Awake()
    {
        if(names.Contains(name)) 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        names.Add(name);
    }
}