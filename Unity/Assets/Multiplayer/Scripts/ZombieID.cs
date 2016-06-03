using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieID : NetworkBehaviour
{
    [SyncVar]
    public string zombileID;
    public Transform myTransform;

    void Start ()
    {
        myTransform = transform;
    }

    void Update ()
    {
        SetIdentity ();
    }

    void SetIdentity ()
    {
        if (myTransform.name == "" || myTransform.name == "Zombile(Clone)") {
            myTransform.name = zombileID;
        }
    }
}
