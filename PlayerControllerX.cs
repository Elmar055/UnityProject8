using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    // rigidbody component of player
    private Rigidbody playerRb;
    // player speed
    private float speed = 500;
    // parent object for camera
    private GameObject focalPoint;
    // turboboost
    private float turboBoost = 10;
    // effect for turboboost
    public ParticleSystem turboSmoke;

    // check for powerup
    public bool hasPowerup;
    // powerup icon
    public GameObject powerupIndicator;
    // powerup activation time
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    
    void Start()
    {
        // assign value to variable
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        // turboBoost code
        if (Input.GetKeyDown(KeyCode.Space))
        {

            playerRb.AddForce(focalPoint.transform.forward * turboBoost, ForceMode.Impulse);
            // play effect
            turboSmoke.Play();
        }


    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        // check tag
        if (other.gameObject.CompareTag("Powerup"))
        {
            // destroy other(powerup)
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            //wait powerUpDuration
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        // check enemy tag
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }



}
