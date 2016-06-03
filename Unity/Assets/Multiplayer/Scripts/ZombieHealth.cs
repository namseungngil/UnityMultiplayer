using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieHealth : NetworkBehaviour
{
    public int health = 50;

    public void DeductHealth (int dmg)
    {
        health -= dmg;
        CheckHealth ();
    }

    void CheckHealth ()
    {
        if (health <= 0) {
            Destroy (gameObject);
        }
    }
}
