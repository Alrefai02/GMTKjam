using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    int[] dice = new int[51];

    public int[] blockDice;
    public int[] attackDice;

    public int[] AboodAttackDice;
    public int[] AboodBlockDice;

    public int[] jeffAttackDice;
    public int[] jeffBlockDice;

    public int[] shawermaAttackDice;
    public int[] shamermaBuffDice;
    public int[] shawermaDebuffDice;
    public int[] shawermaHealDice;

    public int[] bossDice;

    public List<int[]> attackList = new List<int[]>();
    public List<int[]> blockList = new List<int[]>();



    private void Start()
    {
        for (int i = 0; i <= 50; i++)
        {
            dice[i] = i;
        }

        blockDice = dice[1..11];
        attackDice = dice[5..12];

        AboodAttackDice = dice[2..11];
        AboodBlockDice = dice[1..11];

        jeffAttackDice = dice[4..11];
        jeffBlockDice = dice[1..11];

        shawermaAttackDice = dice[1..6];
        shamermaBuffDice = dice[1..6];
        shawermaDebuffDice = dice[1..6];
        shawermaHealDice = dice[1..6];

        bossDice = dice[8..16];

        attackList.Add(AboodAttackDice);
        attackList.Add(jeffAttackDice);
        attackList.Add(shawermaAttackDice);
        attackList.Add(bossDice);

        blockList.Add(AboodBlockDice);
        blockList.Add(jeffBlockDice);

    }

    public int Roll(int[] dice)
    {
        int roll = dice[Random.Range(0, 5)];
        return roll;
    }



}
