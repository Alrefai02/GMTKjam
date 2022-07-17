using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    
    public int currHealth;
    public int maxHealth;
    public int block;

    public Animator anim;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public bool TakeDamage(int dmg)
    {

        block -= dmg;

        if (block > 0)
        { 
            block = 0;
            return false;
        }
        else
        {
            currHealth += block; 
            block = 0;
        }
            

        if (currHealth <= 0)
            return true;
      
            
        else
            return false;
    }

    public void Heal(int amount)
    {
        currHealth += amount;
        if (currHealth > maxHealth)
            currHealth = maxHealth;
    }


}
