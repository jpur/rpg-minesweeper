using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public event System.Action HealthChanged;

    public int CurrentHealth {
        get { return _health; }
        set {
            _health = value;
            if (HealthChanged != null) {
                HealthChanged();
            }
        }
    }
   
    public int MaxHealth;

    private int _health;

    public void TakeDamage(int damage) {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0) {
            CurrentHealth = 0;
            Messenger.SendMessage(Message.GameOver);
        }
        if (HealthChanged != null) {
            HealthChanged();
        }
    }
}
