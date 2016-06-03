using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieMotionSync : NetworkBehaviour
{
    [SyncVar]
    public Vector3 syncPos;
    [SyncVar]
    public float syncYRot;
    public Vector3 lastPos;
    public Quaternion lastRot;
    public Transform myTransform;
    public float lerpRate = 10;
    public float posThreshold = 0.5f;
    public float rotThreshold = 5;

    void Start ()
    {
        myTransform = transform;
    }

    void Update ()
    {
        TransmitMotion ();
        LeftMotion ();
    }

    void TransmitMotion ()
    {
        if (!isServer) {
            return;
        }

        if (Vector3.Distance (myTransform.position, lastPos) > posThreshold || Quaternion.Angle (myTransform.rotation, lastRot) > rotThreshold) {
            lastPos = myTransform.position;
            lastRot = myTransform.rotation;

            syncPos = myTransform.position;
            syncYRot = myTransform.localEulerAngles.y;
        }
    }

    void LeftMotion ()
    {
        if (isServer) {
            return;
        }

        myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);

        Vector3 newRot = new Vector3 (0, syncYRot, 0);
        myTransform.rotation = Quaternion.Lerp (myTransform.rotation, Quaternion.Euler (newRot), Time.deltaTime * lerpRate);
    }
}
