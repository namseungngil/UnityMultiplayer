using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerRespawn : NetworkBehaviour
{
    public PlayerHealth healthScript;
    public Image crossHairImage;
    public GameObject respawnButton;

    public override void PreStartClient ()
    {
        base.PreStartClient ();

        healthScript = gameObject.GetComponent<PlayerHealth> ();
        healthScript.EventRespawn += EnablePlayer;
    }

    public override void OnStartLocalPlayer ()
    {
        base.OnStartLocalPlayer ();

        crossHairImage = GameObject.Find ("Crosshair Image").GetComponent<Image> ();
        SetRespawnButton ();
    }

    void SetRespawnButton ()
    {
        if (isLocalPlayer) {
            respawnButton = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().respawnButton;
            respawnButton.GetComponent<Button> ().onClick.AddListener (CommenceRespawn);
            respawnButton.SetActive (false);
        }
    }

    public override void OnNetworkDestroy ()
    {
        healthScript.EventRespawn -= EnablePlayer;
    }

    void EnablePlayer ()
    {
        gameObject.GetComponent<CharacterController> ().enabled = true;
        gameObject.GetComponent<PlayerShoot> ().enabled = true;
        gameObject.GetComponent<BoxCollider> ().enabled = true;

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer> ();
        foreach (Renderer ren in renderers) {
            ren.enabled = true;
        }

        if (isLocalPlayer) {
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
            crossHairImage.enabled = true;
            respawnButton.SetActive (false);
        }
    }

    void CommenceRespawn ()
    {
        CmdRespawnOnServer ();
    }

    [Command]
    void CmdRespawnOnServer ()
    {
        healthScript.ResetHealth ();
    }
}
