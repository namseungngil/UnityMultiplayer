using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    public delegate void DieDelegate ();

    public event DieDelegate EventDie;

    public delegate void RespawnDelegate ();

    public event RespawnDelegate EventRespawn;

    [SyncVar (hook = "OnHealthChanged")]
    public int health = 100;
    public Text healthText;
    public bool shouldDie = false;
    public bool isDead = false;

    public override void OnStartLocalPlayer ()
    {
        base.OnStartLocalPlayer ();

        healthText = GameObject.Find ("Health Text").GetComponent<Text> ();
        SetHealthText ();
    }

    void Update ()
    {
        CheckCondition ();
    }

    void CheckCondition ()
    {
        if (health <= 0 && !shouldDie && !isDead) {
            shouldDie = true;
        }

        if (health <= 0 && shouldDie) {
            if (EventDie != null) {
                EventDie ();
            }

            shouldDie = false;
        }

        if (health > 0 && isDead) {
            if (EventRespawn != null) {
                EventRespawn ();
            }

            isDead = false;
        }
    }

    void SetHealthText ()
    {
        if (isLocalPlayer) {
            healthText.text = "Health " + health.ToString ();
        }
    }

    void OnHealthChanged (int hlth)
    {
        health = hlth;
        SetHealthText ();
    }

    public void DeductHealth (int dmg)
    {
        health -= dmg;
    }

    public void ResetHealth ()
    {
        health = 100;
    }
}
