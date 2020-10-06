using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour
{
    //Need to make sure it is facing a correct inherited direction when fired

    public float projectileSpeed = .2f;
    public int projectileDamage = 20;
    public float lifeTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        //delete self after x amount of time
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move forward slowly
        transform.localPosition += Vector3.forward * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect if it's the player
        PlayerMovement _player = other.gameObject.GetComponent<PlayerMovement>();

        //if valid:
        if (_player != null)
        {
            Debug.Log("Lazer has hit " + _player.name);

            _player.TakeDamage(projectileDamage);
        }

        //destroy lazer when it hits any object
        Destroy(gameObject);
    }
}
