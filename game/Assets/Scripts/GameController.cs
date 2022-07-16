using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum EnemyState {ATTACK, BLOCK, HEAL, BUFF, DEBUFF}
public enum PlayerChoice { ATTACK, BLOCK }

[RequireComponent(typeof(Image))]

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
    public PlayerChoice playerState;

    public Animator animGO;

    public GameObject image;

    public TMP_Text playerHP;
    public TMP_Text enemyHP;

    public TMP_Text playerMaxHP;
    public TMP_Text enemyMaxHP;

    public GameObject leftButton;
    public GameObject rightButton;

    public GameObject attackSprite;
    public GameObject blockSprite;

    public GameObject attackEnemySprite;
    public GameObject blockEnemySprite;



    // Start is called before the first frame update
    void Start()
    {   
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        image.SetActive(false);
        
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        int num = Random.Range(0, 2);

        GameObject enemyGO = Instantiate(enemyPrefab[num], enemyBattleStation); //NUM num HERE
        enemyUnit = enemyGO.GetComponent<Unit>();

        //Animation Shinanigans
        animGO = enemyGO.GetComponent<Animator>();

        if (enemyUnit.unitName == "Jeff")
        {
            attackChance = 70;
            enemy = 1;
        }
            

        diceRoll = playerGO.GetComponent<DiceRoll>();

        playerHP.text = playerUnit.currHealth + "";
        enemyHP.text = enemyUnit.currHealth + "";

        playerMaxHP.text = "/" + playerUnit.maxHealth;
        enemyMaxHP.text = "/" + enemyUnit.maxHealth;

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        enemyState = EnemyState.ATTACK;
        playerState = PlayerChoice.ATTACK;
        

        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        int damage = diceRoll.Roll(diceRoll.attackDice);
        bool isDead = enemyUnit.TakeDamage(damage);

        enemyHP.text = enemyUnit.currHealth + "";
        enemyHP.color = Color.black;

        yield return new WaitForSeconds(0.75f);

        if (isDead)
        {
            //Animator
            animGO.SetTrigger("Die");

            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //Animator
            animGO.SetTrigger("Take_hit");
            yield return new WaitForSeconds(0.75f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());

        }
       
    }

    IEnumerator EnemyTurn()
    {
        bool isDead = false;

        if (enemyState == EnemyState.ATTACK)
        {
            //Animator
            animGO.SetTrigger("Attack");
            yield return new WaitForSeconds(0.85f);
            image.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            image.SetActive(false);

            int damage = diceRoll.Roll(diceRoll.attackList[enemy]);
            isDead = playerUnit.TakeDamage(damage);

            playerHP.text = playerUnit.currHealth + "";
            playerHP.color = Color.black;


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
        EnemyIntent();

        if (enemyState == EnemyState.BLOCK) 
        {
            int health = enemyUnit.currHealth + enemyUnit.block;
            enemyHP.text = health + "";
            enemyHP.color = new Color(0, 0, 255);
            enemyUnit.block = diceRoll.Roll(diceRoll.blockList[enemy]);

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

    IEnumerator PlayerBlock()
    {

        playerUnit.block = diceRoll.Roll(diceRoll.blockDice);
        int health = playerUnit.currHealth + playerUnit.block;
        playerHP.text = health + "" ;
        playerHP.color = new Color(0, 0, 255, 1);

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

    public void OnBlockButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerBlock());
    }

    public void EnemyIntent()
    {
        int intent = Random.Range(0, 100);
        if (intent < attackChance)
        {
            enemyState = EnemyState.ATTACK;
            blockEnemySprite.SetActive(false);
            attackEnemySprite.SetActive(true);
        }
        else
        {
            enemyState = EnemyState.BLOCK;
            blockEnemySprite.SetActive(true);
            attackEnemySprite.SetActive(false);
        }
            
    }
    public void NextState()
    {
        leftButton.SetActive(true);
        rightButton.SetActive(false);
        playerState = PlayerChoice.BLOCK;
        blockSprite.SetActive(true);
        attackSprite.SetActive(false);

    }

    public void PrevState()
    {
        leftButton.SetActive(false);
        rightButton.SetActive(true);
        playerState = PlayerChoice.ATTACK;
        blockSprite.SetActive(false);
        attackSprite.SetActive(true);
    }

    public void onButtonClick()
    {
        if (playerState == PlayerChoice.ATTACK)
        {
            OnAttackButton();
        }
        else
        {
            OnBlockButton();
        }
    }



}
