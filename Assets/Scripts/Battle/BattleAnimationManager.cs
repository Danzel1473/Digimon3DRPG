using System.Collections;
using UnityEngine;

public class BattleAnimationManager : MonoBehaviour
{
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