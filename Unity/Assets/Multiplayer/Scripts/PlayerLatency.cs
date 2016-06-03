using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerLatency : NetworkBehaviour
{
    public NetworkClient nClient;
    public int latency;
    public Text latencyText;

    public override void OnStartLocalPlayer ()
    {
        base.OnStartLocalPlayer ();

        nClient = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ().client;
        latencyText = GameObject.Find ("Latency Text").GetComponent<Text> ();
    }

    void Update ()
    {
        ShowLatency ();
    }

    void ShowLatency ()
    {
        if (isLocalPlayer) {
            latency = nClient.GetRTT ();
            latencyText.text = "" + latency;
        }
    }
}
