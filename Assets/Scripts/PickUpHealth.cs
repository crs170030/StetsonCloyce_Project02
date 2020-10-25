using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHealth : MonoBehaviour
{
    [SerializeField] float speeen = 0.6f;
    [SerializeField] int healAmount = 50;
    [SerializeField] float respawnTime = 45f;

    float timer = 0f;
    float timerEnd = 1f;
    bool hasBeenUsed = false;

    Vector3 activePosition;// = transform.position;
    Vector3 inActivePosition = new Vector3(0f,-400f,0f);

    void Awake()
    {
        activePosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasBeenUsed)
        {
            timer = Time.time;

            if(timer > timerEnd)
            {
                //if the time limit is reached, respawn the health pack
                hasBeenUsed = false;
                //this.gameObject.SetActive(true);
                this.transform.position = activePosition;
            }
        }
        else
        {
            //rotate
            this.transform.Rotate(0f, speeen, 0f, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider with Health Pick Up Entered!");

        PlayerMovement _player = other.gameObject.GetComponent<PlayerMovement>();

        if (_player != null)
        {
            //only call player take damage if the player has low health
            if (_player._levelController.currentHealth < _player._levelController.maxHealth)
            {
                Debug.Log("Healing player...");
                _player.TakeDamage(-healAmount);//uses take damage script in reverse

                //make powerup go away
                //this.gameObject.SetActive(false);
                this.transform.position = inActivePosition;
                hasBeenUsed = true;
                timerEnd = Time.time + respawnTime;
            }
        }
    }
}
