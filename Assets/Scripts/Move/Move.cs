using System;

[Serializable]
public class Move
{
    public int maxPP;
    public int currentPP;
    public int moveNum;
    public MoveBase MoveBase => MoveTable.Instance[moveNum];

    public Move(int moveNum)
    {
        this.moveNum = moveNum;
        maxPP = MoveBase.PP;
        currentPP = maxPP;
    }

    public void Initialize(int moveNum)
    {
        this.moveNum = moveNum;
        maxPP = MoveBase.PP;
        currentPP = maxPP; 
    }
}