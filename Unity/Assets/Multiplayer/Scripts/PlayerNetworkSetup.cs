using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworkSetup : NetworkBehaviour
{
    public Camera fpsCharacterCam;
    public AudioListener audioListener;

    public override void OnStartLocalPlayer ()
    {
        base.OnStartLocalPlayer ();

        GameObject.Find ("Scene Camera").SetActive (false);
        //            gameObject.GetComponent<CharacterController> ().enabled = true;
        gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
        fpsCharacterCam.enabled = true;
        audioListener.enabled = true;
    }
}
