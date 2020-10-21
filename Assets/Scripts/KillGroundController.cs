using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGroundController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //detect if it's the player
        PlayerMovement _player = other.gameObject.GetComponent<PlayerMovement>();

        //if valid:
        if (_player != null)
        {
            Debug.Log(_player.name + " fell to a clumsy and painful death.");
            _player.TakeDamage(9999999);
        }
    }
}
