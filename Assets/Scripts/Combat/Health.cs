using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFX;

    public static Action<Health> OnDeath;

    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private Knockback _knockback;
    private Flash _flash;

    private void Awake() {
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
    }

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(Vector2 damageSourceDir, int damageAmount, float knockbackThrust) {
        TakeDamage(damageAmount);
        _knockback.GetKnockedBack(damageSourceDir, knockbackThrust);
    }

    public void TakeHit() {
        _flash.StartFlash();
    }
}
