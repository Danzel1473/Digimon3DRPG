using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveTable", menuName = "Tables/new Move Table")]
public class MoveTable : ScriptableObject
{
    private static MoveTable instance;

    public static MoveTable Instance
    {
        get
        {
            if (instance)
                return instance;

            instance = Resources.Load<MoveTable>("Tables/MoveTable");
            instance.Initialize();

            return instance;
        }
    }

    [SerializeField] private MoveBase[] moves;
    [NonSerialized] private Dictionary<int, MoveBase> moveDict;

    private void Initialize()
    {
        if (moveDict == null)
        {
            moveDict = new Dictionary<int, MoveBase>(moves.Select(move => new KeyValuePair<int, MoveBase>(move.MoveID, move)));
        }
    }

    public MoveBase this[int moveID] => moveDict[moveID];
    public int MoveTableLength => moves.Length;
}