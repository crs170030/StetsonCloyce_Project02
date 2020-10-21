using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public AudioClip destroySound;
    public AudioSource _targetSounds;

    public GameObject explosionEffect;
    GameObject explosionGO;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            //Die();
            StartCoroutine(Die());
        }
        //explosionGO = Instantiate(explosionEffect, transform.position, transform.rotation);
        //Destroy(explosionGO, 15f);
    }

    IEnumerator Die()
    {
        _targetSounds.PlayOneShot(destroySound, 1f);
        //_targetSounds.Play();
        explosionGO = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionGO, 1.5f);
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
