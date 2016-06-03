using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerDeath : NetworkBehaviour
{
    public PlayerHealth healthScript;
    public Image crossHairImage;

    public override void PreStartClient ()
    {
        base.PreStartClient ();

        healthScript = gameObject.GetComponent<PlayerHealth> ();
        healthScript.EventDie += DisablePlayer;
    }

    public override void OnStartLocalPlayer ()
    {
        base.OnStartLocalPlayer ();

        crossHairImage = GameObject.Find ("Crosshair Image").GetComponent<Image> ();
    }

    public override void OnNetworkDestroy ()
    {
        healthScript.EventDie -= DisablePlayer;
    }

    void DisablePlayer ()
    {
        gameObject.GetComponent<CharacterController> ().enabled = false;
        gameObject.GetComponent<PlayerShoot> ().enabled = false;
        gameObject.GetComponent<BoxCollider> ().enabled = false;

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer> ();
        foreach (Renderer ren in renderers) {
            ren.enabled = false;
        }

        healthScript.isDead = true;

        if (isLocalPlayer) {
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = false;
            crossHairImage.enabled = false;
            GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().respawnButton.SetActive (true);
        }
    }
}
