using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public int health { get; private set; }
    public int maxHealth { get; private set; }

    public int defaultHealth = 50;
    public int defaultMaxHealth = 100;

    public void Startup() {
        Debug.Log("PlayerManager started");

        UpdateData(defaultHealth, defaultMaxHealth);

        status = ManagerStatus.Started;
    }

    public void UpdateData(int health, int maxHealth) {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public void ChangeHealth(int value) {
        health += value;

        if(health > maxHealth) {
            health = maxHealth;
        } else if(health < 0) {
            health = 0;
        }

        if(health == 0) {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }

        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
    }

    public void Respawn() {
        UpdateData(defaultHealth, defaultMaxHealth);
    }
}