using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyDamage = 10;
    public float bounce = 10f;
    public Rigidbody rbBall;
    //public GameObject goEnemy;

    //public PlayerMovement _player;

    void Start()
    {
        rbBall = GetComponent<Rigidbody>();
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
}
