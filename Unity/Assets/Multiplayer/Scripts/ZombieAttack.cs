using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieAttack : NetworkBehaviour
{
    public float attackRate = 3;
    public float nextAttack;
    public int damage = 10;
    public float minDistance = 2;
    public float currentDistance;
    public Transform myTransform;
    public ZombieTarget targetScript;
    public Material zombieGreen;
    public Material zombieRed;

    void Start ()
    {
        myTransform = transform;
        targetScript = gameObject.GetComponent<ZombieTarget> ();

        if (isServer) {
            StartCoroutine (Attack ());
        }
    }

    void CheckifTargetInRange ()
    {
        if (targetScript.targetTransform != null) {
            currentDistance = Vector3.Distance (targetScript.targetTransform.position, myTransform.position);

            if (currentDistance < minDistance && Time.time > nextAttack) {
                nextAttack = Time.time + attackRate;

                targetScript.targetTransform.GetComponent<PlayerHealth> ().DeductHealth (damage);
                StartCoroutine (ChangeZombileMat ());
                RpcChangeZombieAppearance ();
            }
        }
    }

    [ClientRpc]
    void RpcChangeZombieAppearance ()
    {
        StartCoroutine (ChangeZombileMat ());
    }

    IEnumerator ChangeZombileMat ()
    {
        gameObject.GetComponent<Renderer> ().material = zombieRed;
        yield return new WaitForSeconds (attackRate / 2);
        gameObject.GetComponent<Renderer> ().material = zombieGreen;
    }

    IEnumerator Attack ()
    {
        for (;;) {
            yield return new WaitForSeconds (0.2f);
            CheckifTargetInRange ();
        }
    }
}
