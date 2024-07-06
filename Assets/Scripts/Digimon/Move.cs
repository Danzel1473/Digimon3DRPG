public class Move
{
    public MoveBase moveBase { get; set; }
    public int pp { get; set; }

    public Move(MoveBase moveBase)
    {
        this.moveBase = moveBase;
        pp = moveBase.PP;
    }
}