using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnManagerZombieSpawner : NetworkBehaviour
{
    public GameObject zombiePrefab;
    public GameObject[] zombieSpawns;
    public int counter;
    public int numberOfZobiles = 20;
    public int maxNumberOfZombies = 80;
    public float waveRate = 1;
    public bool isSpawnActivated = true;

    public override void OnStartServer ()
    {
        base.OnStartServer ();

        zombieSpawns = GameObject.FindGameObjectsWithTag ("ZombieSpawn");
        StartCoroutine (ZombieSpawner ());
    }

    IEnumerator ZombieSpawner ()
    {
        for (;;) {
            yield return new WaitForSeconds (waveRate);
            GameObject[] zombies = GameObject.FindGameObjectsWithTag ("Zombie");
            if (zombies.Length < maxNumberOfZombies) {
                CommenceSpawn ();
            }
        }
    }

    void CommenceSpawn ()
    {
        if (isSpawnActivated) {
            for (int i = 0; i < zombieSpawns.Length; i++) {
                int randomIndex = Random.Range (0, zombieSpawns.Length);
                SpawnZombies (zombieSpawns [randomIndex].transform.position);
            }
        }
    }

    void SpawnZombies (Vector3 spawnPos)
    {
        counter++;
        GameObject gO = GameObject.Instantiate (zombiePrefab, spawnPos, Quaternion.identity) as GameObject;
        gO.GetComponent<ZombieID> ().zombileID = "Zombie" + counter;

        NetworkServer.Spawn (gO);
    }
}
