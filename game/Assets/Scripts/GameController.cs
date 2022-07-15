using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum EnemyState {ATTACK, BLOCK, HEAL, BUFF, DEBUFF}
 
public class GameController : MonoBehaviour
{
    int attackChance = 60;
    int enemy = 0;

    public GameObject playerPrefab;
    public GameObject[] enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    DiceRoll diceRoll;

    public BattleState state;
    public EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {   
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        int num = Random.Range(0, 2);

        GameObject enemyGO = Instantiate(enemyPrefab[num], enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        
        if (enemyUnit.unitName == "Jeff")
        {
            attackChance = 70;
            enemy = 1;
        }
            

        diceRoll = playerGO.GetComponent<DiceRoll>();

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        enemyState = EnemyState.ATTACK;

        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        int damage = diceRoll.Roll(diceRoll.attackDice);
        bool isDead = enemyUnit.TakeDamage(damage);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        bool isDead = false;

        if (enemyState == EnemyState.ATTACK)
        {
            int damage = diceRoll.Roll(diceRoll.attackList[enemy]);
            isDead = playerUnit.TakeDamage(damage);
        }
        else
        {
            enemyUnit.block = - diceRoll.Roll(diceRoll.blockList[enemy]);
        }
  
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            // display intent
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    void EndBattle()
    {
        /*if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }*/

        print("aaa");
    }

    void PlayerTurn()
    {
        //dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {

        int heal = diceRoll.Roll(diceRoll.healDice);
        playerUnit.Heal(heal);

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    public void EnemyIntent()
    {
        int intent = Random.Range(0, 100);
        if (intent < attackChance)
            enemyState = EnemyState.ATTACK;
        else
            enemyState = EnemyState.BLOCK;
    }

}
