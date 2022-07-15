using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    
    public int currHealth;
    public int maxHealth;
    public int block;

    public bool TakeDamage(int dmg)
    {
        currHealth -= dmg;

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
