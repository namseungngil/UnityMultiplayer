using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieTarget : NetworkBehaviour
{
    public NavMeshAgent agent;
    public Transform myTransform;
    public Transform targetTransform;
    public LayerMask raycastLayer;
    public float radius = 100f;

    void Start ()
    {
        agent = gameObject.GetComponent<NavMeshAgent> ();
        myTransform = transform;
        raycastLayer = 1 << LayerMask.NameToLayer ("Player");

        if (isServer) {
            StartCoroutine (DoChek ());
        }
    }

    void FixedUpdate ()
    {
//        SearchForTarget ();
//        MoveToTarget ();
    }

    void SearchForTarget ()
    {
        if (!isServer) {
            return;
        }

        if (targetTransform == null) {
            Collider[] hitColliders = Physics.OverlapSphere (myTransform.position, radius, raycastLayer);

            if (hitColliders.Length > 0) {
                int randomint = Random.Range (0, hitColliders.Length);
                targetTransform = hitColliders [randomint].transform;
            }
        }

        if (targetTransform != null && targetTransform.GetComponent<BoxCollider> ().enabled == false) {
            targetTransform = null;
        }
    }

    void MoveToTarget ()
    {
        if (targetTransform != null && isServer) {
            SetNavDestination (targetTransform);
        }
    }

    void SetNavDestination (Transform dest)
    {
        if (agent.enabled) {
            agent.SetDestination (dest.position);
        }
    }

    IEnumerator DoChek ()
    {
        for (;;) {
            SearchForTarget ();
            MoveToTarget ();
            yield return new WaitForSeconds (0.2f);
        }
    }
}
