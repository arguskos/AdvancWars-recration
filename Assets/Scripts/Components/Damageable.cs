using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : BaseUnitProprty {
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public Action OnDeath;
    public Action OnHealthChanged;

    

    public void Start()
    {
        MaxHealth = GetComponent<BaseUnit>().Setting.DamageableSettings.MaxSettings;
        Health = MaxHealth;
    }

    public void ModifyHealth(int amount)
    {
        Health += amount;
        
        if (Health<=amount)
        {
            if (OnDeath!=null)
            {
                OnDeath();
            }
            else
            {
                Debug.LogError("should be destroyd on death ");
            }
        }
        else if (Health>MaxHealth)
        {
            Health = MaxHealth;
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }
    [Serializable]
    public class Settings
    {
        public int MaxSettings;
    }
}
