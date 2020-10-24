using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyDamage = 10; //https://www.youtube.com/watch?v=3fiwlk3PWTc
    public float bounce = 10f;
    public float fireRate = 5f;
    public float rotateStrength = .5f;
    public float attackRadius = 50f;
    public float health = 70f;
    public int bounty = 100;

    public float shootForce = 5000f;
    public float projLifetime = 20f;

    public Transform target;
    public GameObject lazer;
    public GameObject enemyGun;
    public Rigidbody rbBall;
    public GameObject explosionEffect;

    public AudioClip shootLaser;
    public AudioClip alert;
    public AudioClip destroySound;
    public AudioSource _enemySounds;

    //public PlayerMovement _player;
    private float nextTimeToFire;
    private float reAdjust;
    private bool hasNoticed = false;

    void Start()
    {
        rbBall = GetComponent<Rigidbody>();
        _enemySounds = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //if player is within x radius of enemy
        if (distance <= attackRadius) {

            if (!hasNoticed)
            {
                _enemySounds.pitch = 1f;
                _enemySounds.PlayOneShot(alert, 1f);
                //wait a bit before firing
                nextTimeToFire = Time.time + (fireRate * 1.5f);
            }

            hasNoticed = true;

            //slowly turn towards player
            /*
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            float str = Mathf.Min(rotateStrength * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
            */
            transform.LookAt(target);

            //if reloadTime is == 0, call shoot script to fire gun
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                //Debug.Log("Distance between enemy and player == " + distance);
                FireWeapon();
            }
        }
        else
        {
            //if player has gone out of range, play low alert sound
            if (hasNoticed)
            {
                _enemySounds.pitch = .7f;
                _enemySounds.PlayOneShot(alert, 1f);
            }

            hasNoticed = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //detect if it's the player
        PlayerMovement _player = other.gameObject.GetComponent<PlayerMovement>();
        
        //if valid:
        if (_player != null)
        {
            Debug.Log("Enemy has been touched by :" + _player.name);

            transform.LookAt(_player.transform);
            rbBall.AddForce(transform.forward * -bounce);

            //Debug.Log("Enemy has been touched!");
            //levelController.Kill(false);
            _player.TakeDamage(enemyDamage);
        }
    }

    void FireWeapon()
    {
        //create lazer clone at enemy gun
        GameObject lazerGO = Instantiate(lazer, (enemyGun.transform.position), transform.rotation);
        //move lazer forward by x force
        lazerGO.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);

        //play Lazer noise
        _enemySounds.PlayOneShot(shootLaser, 1f);

        //destroy projectile after x seconds
        Destroy(lazerGO, projLifetime);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Debug.Log("I, "+ gameObject.name +" am dead! Distributing bounty...");
            //target.IncreaseScore(10);
            //call level controller to increase score
            Level01Controller _lc1 = FindObjectOfType<Level01Controller>();
            _lc1.IncreaseScore(bounty);

            //start death procedure
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        _enemySounds.PlayOneShot(destroySound, 1f);
        //_targetSounds.Play();
        GameObject explosionGO = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionGO, 1.5f);
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
