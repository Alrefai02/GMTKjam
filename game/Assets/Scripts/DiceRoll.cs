using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{

    public int[] healDice = { 1, 2, 3, 4, 5, 6 };
    public int[] attackDice = { 1, 2, 3, 4, 5, 6 };

    public int[] enemyAttackDice = { 1, 2, 3, 4, 5, 6 };


    public int Roll(int[] dice)
    {
        int roll = dice[Random.Range(0, 5)];
        return roll;
    }


}
