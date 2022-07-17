using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum EnemyState {ATTACK, BLOCK, HEAL, BUFF, DEBUFF}
public enum PlayerChoice { ATTACK, BLOCK }

[RequireComponent(typeof(Image))]

public class GameController : MonoBehaviour
{
    int attackChance = 60;
    int enemy = 0;

    public int num;

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

    public GameObject playerGO;


    public Animator animGO;

    public GameObject image;

    public TMP_Text playerHP;
    public TMP_Text enemyHP;

    public TMP_Text playerMaxHP;
    public TMP_Text enemyMaxHP;

    public GameObject leftButton;
    public GameObject rightButton;

    public GameObject attackSprite;
    public GameObject nerdAttack;
    public GameObject gambleAttack;


    public GameObject blockSprite;
    public GameObject healSprite;

    public GameObject attackEnemySprite;
    public GameObject blockEnemySprite;

    public GameObject diceTextObject;
    public TMP_Text diceText;

    public GameObject enemyDiceTextObject;
    public TMP_Text enemyDiceText;

    public GameObject winScreen;
    public GameObject loseScreen;

    public GameObject button;
    public Button buttonInt;

    public AudioSource DiceSound;
    public AudioClip DiceRollSound;

    int buffInc = 0;
    bool canBuff = false;

    bool canHeal = false;

    public GameObject effect;

    
    bool isGymBro = false;

    bool finalBoss = false;




    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        image.SetActive(false);

        if (diceRoll.attackDice[0] == diceRoll.gamblerDice[0])
            attackSprite = gambleAttack;
        else if (diceRoll.attackDice[0] == diceRoll.nerdDice[0])
            attackSprite = nerdAttack;
        attackSprite.SetActive(true);

        if (DiceRoll.Instance.isHealing)
            blockSprite = healSprite;

    }

    IEnumerator SetupBattle()
    {
        buttonInt = button.GetComponent<Button>();


        playerUnit = playerGO.GetComponent<Unit>();
        

        num = Random.Range(0, 2);

        GameObject enemyGO = Instantiate(enemyPrefab[num], enemyBattleStation); //NUM num HERE
        enemyUnit = enemyGO.GetComponent<Unit>();

        //Animation Shinanigans
        animGO = enemyGO.GetComponent<Animator>();

        if (enemyUnit.unitName == "Jeff")
        {
            attackChance = 70;
            enemy = 1;
        }
        else if(enemyUnit.unitName == "Shawarma")
        {
            canBuff = true;
            attackChance = 50;
        }
        else if (enemyUnit.unitName == "Boss")
        {
            attackChance = 100;
            finalBoss = true;
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
        StartCoroutine(rollPlayerDice());
        yield return new WaitForSeconds(1f);
        int damage = diceRoll.Roll(diceRoll.attackDice);
        diceText.text = damage.ToString();
        yield return new WaitForSeconds(2f);
        bool isDead = enemyUnit.TakeDamage(damage);

        enemyHP.text = enemyUnit.currHealth + "";
        enemyHP.color = Color.black;

        yield return new WaitForSeconds(0.75f);

        if (isDead)
        {
            //Animator
            animGO.SetTrigger("Die");

            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            //Animator
            animGO.SetTrigger("Take_hit");
            yield return new WaitForSeconds(0.75f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            diceRoll.attackList[enemy] = diceRoll.bossDice;
        }
        playerState = PlayerChoice.ATTACK;
    }

    IEnumerator EnemyTurn()
    {
        buttonInt.enabled = false;

        enemyHP.text = enemyUnit.currHealth + "";
        enemyHP.color = Color.black;

        //enemyDiceTextObject.SetActive(false);
        bool isDead = false;
        if (enemyState == EnemyState.ATTACK)
        {
            StartCoroutine(rollEnemyDice());
            yield return new WaitForSeconds(1f);
            int damage = diceRoll.Roll(diceRoll.attackList[enemy]);
            enemyDiceText.text = (damage+buffInc).ToString();
            //Animator
            animGO.SetTrigger("Attack");
            yield return new WaitForSeconds(0.85f);
            image.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            image.SetActive(false);

            //int damage = diceRoll.Roll(diceRoll.attackList[enemy]);
            //print("enemy damage = "+damage);
            isDead = playerUnit.TakeDamage(damage+buffInc);

            playerHP.text = playerUnit.currHealth + "";
            playerHP.color = Color.black;


        }


        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            // display intent
            ShowIcon();
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        enemyDiceTextObject.SetActive(false);
        EnemyIntent();

        yield return new WaitForSeconds(1f);

        if (enemyState == EnemyState.BLOCK)
            StartCoroutine(enemyBlock());

    }

    IEnumerator EndBattle()
    {
        
        yield return new WaitForSeconds(0.5f);

        if (state == BattleState.WON)
        {
            if (finalBoss)
            {
                winScreen.SetActive(true);
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene(0);
            }
            else
            {
                winScreen.SetActive(true);
            }
        }
        else if (state == BattleState.LOST)
        {
            loseScreen.SetActive(true);

        }
    }

    void PlayerTurn()
    {
        buttonInt.enabled = true;
        //dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerBlock()
    {
        if (!DiceRoll.Instance.isHealing)
        {
            StartCoroutine(rollPlayerDice());
            yield return new WaitForSeconds(1f);
            playerUnit.block = diceRoll.Roll(diceRoll.blockDice);
            int health = playerUnit.currHealth + playerUnit.block;
            diceText.text = playerUnit.block.ToString();
            playerHP.text = health + "";
            playerHP.color = new Color(0, 0, 255, 1);

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());

            playerState = PlayerChoice.ATTACK;
        }
        else
        {
            StartCoroutine(rollPlayerDice());
            yield return new WaitForSeconds(1f);
            playerUnit.block = diceRoll.Roll(diceRoll.blockDice);
            playerUnit.currHealth += playerUnit.block;
            diceText.text = playerUnit.block.ToString();
            playerHP.text = playerUnit.currHealth.ToString();
            //playerHP.color = new Color(0, 0, 255, 1);

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());

            playerState = PlayerChoice.ATTACK;
        }
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
        DiceSound.PlayOneShot(DiceRollSound);
        buttonInt.enabled = false;
        if (playerState == PlayerChoice.ATTACK)
        {
            OnAttackButton();
        }
        else
        {
            OnBlockButton();
        }
    }

    IEnumerator rollPlayerDice()
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        blockSprite.SetActive(false);
        attackSprite.SetActive(false);
        diceTextObject.SetActive(true);

        int[] diceRoll;

        if (playerState == PlayerChoice.ATTACK)
        {
            diceRoll = DiceRoll.Instance.attackDice;
        }
        else
        {
            diceRoll = DiceRoll.Instance.blockDice;
        }


        for (int i = 0; i < diceRoll.Length; i++)
        {
            int num = Random.Range(0, diceRoll.Length);
            diceText.text = diceRoll[num].ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator rollEnemyDice()
    {
        blockEnemySprite.SetActive(false);
        attackEnemySprite.SetActive(false);
        enemyDiceTextObject.SetActive(true);

        int[] diceRoll;

        if (num == 0)
        {
            if (enemyState == EnemyState.ATTACK)
            {
                diceRoll = DiceRoll.Instance.AboodAttackDice;
            }
            else
            {
                diceRoll = DiceRoll.Instance.AboodBlockDice;
            }
        }
        else
        {
            if (enemyState == EnemyState.ATTACK)
            {
                diceRoll = DiceRoll.Instance.jeffAttackDice;
            }
            else
            {
                diceRoll = DiceRoll.Instance.jeffBlockDice;
            }
        }

        for (int i = 0; i < diceRoll.Length; i++)
        {
            int number = Random.Range(0, diceRoll.Length);
            enemyDiceText.text = diceRoll[number].ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ShowIcon()
    {
        leftButton.SetActive(false);
        rightButton.SetActive(true);
        blockSprite.SetActive(false);
        attackSprite.SetActive(true);
        diceTextObject.SetActive(false);
    }

    IEnumerator enemyBlock()
    {
        StartCoroutine(rollEnemyDice());
        yield return new WaitForSeconds(1f);

        if (!canBuff && !canHeal)
        {
            enemyUnit.block = diceRoll.Roll(diceRoll.blockList[enemy]);
            enemyDiceText.text = enemyUnit.block.ToString();
            int health = enemyUnit.currHealth + enemyUnit.block;
            enemyHP.color = new Color(0, 0, 255);
            enemyHP.text = health + "";
        }
        else if (canBuff)
        {
            int inc = diceRoll.Roll(diceRoll.shamermaBuffDice);
            enemyDiceText.text = inc.ToString();
            buffInc += inc;
            GameObject stuff = Instantiate(effect, transform);
            Destroy(stuff, 200f);

        }
        
    }

    public void TryHarder()
    {
        resetStats();
        SceneManager.LoadScene("Level1");
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("UI");
    }

    void resetStats()
    {
        playerUnit.currHealth = playerUnit.maxHealth;
        DiceRoll.Instance.resetDice();
    }

    public void gymBro()
    {
        DiceRoll.Instance.setGymBro();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void drugAddict()
    {
        DiceRoll.Instance.isHealing = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
