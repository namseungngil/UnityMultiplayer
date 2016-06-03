using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShoot : NetworkBehaviour
{
    public int damage = 25;
    public float range = 200;
    public Transform camTransform;
    public RaycastHit hit;

    void Update ()
    {
        CheckifShooting ();
    }

    void CheckifShooting ()
    {
        if (!isLocalPlayer) {
            return;
        }

        if (Input.GetMouseButtonDown (0)) {
            Shoot ();
        }
    }

    void Shoot ()
    {
        if (Physics.Raycast (camTransform.TransformPoint (0, 0, 0.5f), camTransform.forward, out hit, range)) {
            if (hit.transform.tag == "Player") {
                CmdTellServerWhoWasShot (hit.transform.name, damage);
            } else if (hit.transform.tag == "Zombie") {
                CmdTellServerWhichZombieWasShot (hit.transform.name, damage);
            }
        }
    }

    [Command]
    void CmdTellServerWhoWasShot (string uniqueID, int dmg)
    {
        GameObject gO = GameObject.Find (uniqueID);
        gO.GetComponent<PlayerHealth> ().DeductHealth (dmg);
    }

    [Command]
    void CmdTellServerWhichZombieWasShot (string uniqueId, int dmg)
    {
        GameObject gO = GameObject.Find (uniqueId);
        gO.GetComponent<ZombieHealth> ().DeductHealth (dmg);
    }
}
