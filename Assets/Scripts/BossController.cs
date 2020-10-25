using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//play sound, spawn boss

public class BossController : MonoBehaviour
{
    public GameObject boss;
    public GameObject point;

    public AudioClip spawnAlert;
    public AudioSource _bossIncomming;

    public float waitTime = 2f;
    public float pointFlashTime = .1f;

    public void Spawn()
    {
        _bossIncomming.PlayOneShot(spawnAlert, 1f);
        //boss.SetActive(true);
        StartCoroutine(WaitThenSpawn());
        StartCoroutine(FlashSpawnPoint());
    }

    IEnumerator WaitThenSpawn()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Aw lawd, here he comes!");
        //_bossIncomming.PlayOneShot(spawnAlert, 1f);
        boss.SetActive(true);
    }

    IEnumerator FlashSpawnPoint()//toggle spawn point a few times
    {
        for(int i = 0; i < 10; i++)
        {
            point.SetActive(false);
            yield return new WaitForSeconds(pointFlashTime);
            point.SetActive(true);
            yield return new WaitForSeconds(pointFlashTime);
        }
        point.SetActive(false);
    }
}
