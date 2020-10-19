using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public AudioClip destroySound;
    public AudioSource _targetSounds;

    public GameObject explosionEffect;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            //Die();
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        _targetSounds.PlayOneShot(destroySound, 1f);
        //_targetSounds.Play();
        GameObject explosionGO = Instantiate(explosionEffect, transform);
        Destroy(explosionGO, 1.5f);
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
