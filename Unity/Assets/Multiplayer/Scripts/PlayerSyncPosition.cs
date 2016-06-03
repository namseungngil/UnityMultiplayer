using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[NetworkSettings (channel = 0, sendInterval = 0.1f)]
public class PlayerSyncPosition : NetworkBehaviour
{
    [SyncVar (hook = "SyncPositionValue")]
    public Vector3 syncPos;
    public Transform myTransform;
    public float lerpRate;
    public float normalLerpRate = 16;
    public float fasterLerpRate = 27;
    public Vector3 lastPos;
    public float threshold = 0.5f;
    public NetworkClient nClient;
    public int latency;
    public Text latencyText;
    public List<Vector3> syncPosList = new List<Vector3> ();
    public bool useHistoricalLerping = false;
    public float closeEnough = 0.11f;

    void Start ()
    {
        nClient = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ().client;
        latencyText = GameObject.Find ("Latency Text").GetComponent<Text> ();
        lerpRate = normalLerpRate;
    }

    void Update ()
    {
        LerpPosition ();
        ShowLatency ();
    }

    void FixedUpdate ()
    {
        TransmitPositions ();
    }

    void LerpPosition ()
    {
        if (!isLocalPlayer) {
            if (useHistoricalLerping) {
                HistoricalLerping ();
            } else {
                OrdinaryLerping ();
            }

//            Debug.Log (Time.deltaTime);
        }
    }

    [Command]
    void CmdProvidePositionToServer (Vector3 pos)
    {
//        Debug.Log ("CmdProvidePositionToServer " + pos);
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPositions ()
    {
        if (isLocalPlayer && Vector3.Distance (myTransform.position, lastPos) > threshold) {
            CmdProvidePositionToServer (myTransform.position);
            lastPos = myTransform.position;
        }
    }

    [Client]
    void SyncPositionValue (Vector3 latestPos)
    {
        syncPos = latestPos;
        syncPosList.Add (syncPos);
    }

    void ShowLatency ()
    {
        if (isLocalPlayer) {
            latency = nClient.GetRTT ();
            latencyText.text = "" + latency;
        }
    }

    void OrdinaryLerping ()
    {
        myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    void HistoricalLerping ()
    {
        if (syncPosList.Count > 0) {
            myTransform.position = Vector3.Lerp (myTransform.position, syncPosList [0], Time.deltaTime * lerpRate);

            if (Vector3.Distance (myTransform.position, syncPosList [0]) < closeEnough) {
                syncPosList.RemoveAt (0);
            }

            if (syncPosList.Count > 0) {
                lerpRate = fasterLerpRate;
            } else {
                lerpRate = normalLerpRate;
            }
        }
    }
}
