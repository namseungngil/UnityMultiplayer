using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSyncRotation : NetworkBehaviour
{
    [SyncVar]
    public Quaternion syncPlayerRotation;
    [SyncVar]
    public Quaternion syncCamRotation;
    public Transform playerTransform;
    public Transform camTransform;
    public float lerpRate = 15;
    public Quaternion lastPlayerRot;
    public Quaternion lastCamRot;
    public float threshold = 5f;

    void Update ()
    {
        LerpRotations ();
    }

    void FixedUpdate ()
    {
        TransmitRotations ();
    }

    void LerpRotations ()
    {
        if (!isLocalPlayer) {
            playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
            camTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvideRotationsToServer (Quaternion playerRot, Quaternion camRot)
    {
//        Debug.Log ("CmdProvideRotationsToServer");
        syncPlayerRotation = playerRot;
        syncCamRotation = camRot;
    }

    [Client]
    void TransmitRotations ()
    {
        if (isLocalPlayer) {
            if (Quaternion.Angle (playerTransform.rotation, lastPlayerRot) > threshold || Quaternion.Angle (camTransform.rotation, lastCamRot) > threshold) {
                CmdProvideRotationsToServer (playerTransform.rotation, camTransform.rotation);
                lastPlayerRot = playerTransform.rotation;
                lastCamRot = camTransform.rotation;
            }
        }
    }
}
