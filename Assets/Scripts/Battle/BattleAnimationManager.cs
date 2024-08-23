using System.Collections;
using UnityEngine;

public class BattleAnimationManager : MonoBehaviour
{
    private static BattleAnimationManager instance = new BattleAnimationManager();
    public static BattleAnimationManager Instance => instance;
    public IEnumerator PlayAttackAnimation(Move move, BattleEntity attacker)
    {
        bool isSPAttack = move.moveBase.MoveCategory == MoveCategory.Special;

        attacker.PlayAttackAnimation(isSPAttack);

        attacker.PlayMoveParticle(move.moveBase, attacker.transform);

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PlayDefenderDamageAnimation(BattleEntity defender)
    {
        defender.PlayDamageAnimation();

        yield return new WaitForSeconds(1f);
    }
}