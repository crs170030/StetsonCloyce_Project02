using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyDamage = 10;
    public float bounce = 10f;
    public float fireRate = 5f;
    public float rotateStrength = .5f;

    public Transform target;
    public GameObject lazer;
    public GameObject enemyGun;
    public Rigidbody rbBall;
    //public GameObject goEnemy;

    //public PlayerMovement _player;
    private float nextTimeToFire;

    void Start()
    {
        rbBall = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //slowly turn towards player
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        float str = Mathf.Min(rotateStrength * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

        //if facing player and reloadTime is == 0, call shoot script to fire gun
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            FireWeapon();
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
        GameObject lazerGO = Instantiate(lazer, (enemyGun.transform.position + Vector3.forward), transform.rotation);
    }
}
