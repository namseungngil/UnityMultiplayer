using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerID : NetworkBehaviour
{
    [SyncVar]
    public string playerUniqueName;
    public NetworkInstanceId playerNetID;
    public Transform myTransform;

    void Awake ()
    {
        myTransform = transform;
    }

    void Update ()
    {
        if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
            SetIdentity ();
        }
    }

    public override void OnStartLocalPlayer ()
    {
        base.OnStartLocalPlayer ();

        GetNetIdentity ();
        SetIdentity ();
    }

    [Client]
    void GetNetIdentity ()
    {
        playerNetID = gameObject.GetComponent<NetworkIdentity> ().netId;
        CmdTellServerMyIdentity (MakeUniqueName ());
    }

    void SetIdentity ()
    {
        if (!isLocalPlayer) {
            myTransform.name = playerUniqueName;
        } else {
            myTransform.name = MakeUniqueName ();
        }
    }

    [Command]
    void CmdTellServerMyIdentity (string name)
    {
        playerUniqueName = name;
    }

    string MakeUniqueName ()
    {
        return "Player" + playerNetID.ToString ();
    }
}
