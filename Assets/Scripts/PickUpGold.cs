using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGold : MonoBehaviour
{
    [SerializeField] float speeen = 0.6f;
    [SerializeField] int value = 500;

    void FixedUpdate()
    {
        //rotate
        this.transform.Rotate(0f, speeen, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider with Gold Pick Up Entered!");

        PlayerMovement _player = other.gameObject.GetComponent<PlayerMovement>();

        if (_player != null)
        {
            Debug.Log("Acquiring Currency...");
            _player._levelController.IncreaseScore(value);

            //make powerup go away
            this.gameObject.SetActive(false);
        }
    }
}
