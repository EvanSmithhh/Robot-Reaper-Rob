using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{

    public bool rocketSelfDamage = false;
    public float speed = 20.0f;
    public ParticleSystem explosionParticle;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Enables the ability for a rocket to hit the player half of a second after its creation
        Invoke("enableSelfDamage", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Rocket moves forward constantly
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {

        /* Rocket collides with:
         A wall and destroys itself;
         an enemy and damages(destroys) the enemy and itself or;
         the player and destroys itself: explosion damages the player */

        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Instantiate(explosionParticle, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyBehavior enemyScrp = other.gameObject.GetComponent<EnemyBehavior>();
            enemyScrp.health -= 100f;
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Instantiate(explosionParticle, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Player") && rocketSelfDamage)
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Instantiate(explosionParticle, transform.position, transform.rotation);
            // Player is damaged by explosion
        }

        

    }

    // Enables the self damage boolean
    void enableSelfDamage()
    {
        rocketSelfDamage = true;
    }

}
