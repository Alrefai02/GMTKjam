using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DiceRoll : MonoBehaviour
{
    int[] dice = new int[51];

    public int[] blockDice;
    public int[] attackDice;

    //first round powerups
    public int[] gamblerDice = { 1, 1, 1, 15, 15 };
    public int[] nerdDice = {7,8,9};


    //second round powerups
    public bool addictdice = false;
    public bool gymBroDice = false;



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

    public static DiceRoll Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        print("Start buddy");
        for (int i = 0; i <= 50; i++)
        {
            dice[i] = i;
        }

        attackDice = dice[5..12];
        blockDice = dice[1..11];

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

    public void setGambler()
    {
        attackDice = gamblerDice;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void setNerd()
    {
        attackDice = nerdDice;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    private void Update()
    {
        print(attackDice[0]);
    }
}
