using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(Damageable))]
public class HealthDrawer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Damageable _damageable;
    private NumToSprite _numToSprite;

    [Inject]
    public void Construct(NumToSprite nS, SpriteRenderer sp, Damageable d)
    {
        _spriteRenderer = sp;
        _numToSprite = nS;
        _damageable = d;
        d.OnHealthChanged = UpdateHealth;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if (_damageable.Health == _damageable.MaxHealth)
        {
            _spriteRenderer.enabled = false;
        }
        else
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = _numToSprite.GetSprite(_damageable.Health);
        }
    }

}
