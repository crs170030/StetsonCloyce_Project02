using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyDamage = 10; //https://www.youtube.com/watch?v=3fiwlk3PWTc
    public float bounce = 10f;
    public float fireRate = 5f;
    public float rotateStrength = .5f;
    public float attackRadius = 60f;
    public float moveCloserRadius = 80f;
    public float moveAwayRadius = 10f;
    public float health = 70f;
    public int bounty = 100;
    public float speed = 12f;

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

    Vector3 lazerPosition;
    private float nextTimeToFire;
    private float reAdjust;
    private bool hasNoticed = false;
    bool isDead = false;
    float pitchEffect = 1f;

    void Start()
    {
        rbBall = GetComponent<Rigidbody>();
        _enemySounds = GetComponent<AudioSource>();

        if(gameObject.name == "Enemy_BOOS")
        {
            pitchEffect = .5f;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //if player is within x radius of enemy
        if (distance <= attackRadius) {

            if (!hasNoticed)//if enemy has just noticed player
            {
                _enemySounds.pitch = 1f * pitchEffect;
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

            if(distance <= moveAwayRadius)//if player is too close, back away a bit
            {
                rbBall.velocity = -transform.forward * (speed/2);
            }else if (distance >= moveCloserRadius) //if 60 < distance < 80
            {
                rbBall.velocity = transform.forward * speed;
            }

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
            //player has just gone out of attack range
            if (hasNoticed)
            {
                _enemySounds.pitch = .7f * pitchEffect;
                _enemySounds.PlayOneShot(alert, 1f);
                hasNoticed = false;
            }           
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
        /*
        if (gameObject.name == "Enemy_BOOS")
        {
            //if boss, shoot from front
            lazerPosition = enemyGun.transform.position * (Vector3.right * 1.5f); 
        }
        else
        {
            //if not boss, shoot from left
            lazerPosition = enemyGun.transform.position; 
        }*/
        lazerPosition = enemyGun.transform.position;

        //create lazer clone at enemy gun
        GameObject lazerGO = Instantiate(lazer, lazerPosition, transform.rotation);
        //move lazer forward by x force
        lazerGO.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);

        //play Lazer noise
        _enemySounds.PlayOneShot(shootLaser, .5f);

        //destroy projectile after x seconds
        Destroy(lazerGO, projLifetime);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        //if shot by player and has not noticed, make enemy angry and always chase player
        if (!hasNoticed)
        {
            attackRadius = Vector3.Distance(target.position, transform.position) * 1.15f;
            Debug.Log("Shot me in the back? Now I'm angry! Attack radius == " + attackRadius);
            //moveCloserRadius = attackRadius + 20f; 
            //hasNoticed = true;
        }

        if (health <= 0f && !isDead)//if health is below zero and is not already dead
        {
            Debug.Log("I, "+ gameObject.name +" am dead! Distributing bounty...");
            isDead = true;
            //target.IncreaseScore(10);
            //call level controller to increase score
            Level01Controller _lc1 = FindObjectOfType<Level01Controller>();
            _lc1.IncreaseScore(bounty);
            if (gameObject.name == "Enemy_BOOS")
                _lc1.KillBoss();
           //start death procedure
           StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        _enemySounds.PlayOneShot(destroySound, .5f);
        //_targetSounds.Play();
        GameObject explosionGO = Instantiate(explosionEffect, transform.position, transform.rotation);
        if (gameObject.name == "Enemy_BOOS")
        {
            Debug.Log("The Boss is dead!");
            //_enemySounds.pitch = .5f;
            explosionGO.transform.localScale = new Vector3(4f,4f,4f); //make explosion much bigger
        }
        else
            explosionGO.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //make explosion lil bigger

        Destroy(explosionGO, 1.5f);
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
