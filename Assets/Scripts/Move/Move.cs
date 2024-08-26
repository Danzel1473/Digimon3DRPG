using UnityEngine;

public class Move
{
    public MoveBase moveBase { get; private set; }

    public Move(MoveBase moveBase)
    {
        this.moveBase = moveBase;
    }
}