using UnityEngine;

[CreateAssetMenu(fileName = "NewMove", menuName = "Digimon/Move")]
public class Move : ScriptableObject
{
    public string moveName;
    public string description;
    public int power; //위력
    public int powerPoints; //사용 횟수(포켓몬의 PP)
    public MoveType moveType;
    public Element moveElement; //속성

    public enum MoveType
    {
        Physical,
        Special
    }

}