using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    private float speed = 20000.0f;
    private float maxSpeed = 10.0f;
    private float minDistance = 2.0f;
    private float failSafeDistance = -20.0f;
    private float distanceFromPlayer;
    public float health = 100;
    private float damage = 50.0f;
    private GameObject player;
    private Rigidbody enemyRb;
    private GameManager gameManager;

    private Animator animator;

    public ParticleSystem deathExplosion;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            MoveToPlayer();
        }

        // Stops the enemies from moving too fast
        if (enemyRb.velocity.magnitude > maxSpeed)
        {
            enemyRb.velocity = Vector3.ClampMagnitude(enemyRb.velocity, maxSpeed);
        }

        // Destroys enemies if they glitch through the floor
        if (transform.position.y < failSafeDistance)
        {
            Destroy(gameObject);
        }

        // Destroys enemies when their health drops below 1
        if (health < 1)
        {
            gameManager.AddScore();
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (gameManager.gameOver)
        {
            animator.SetTrigger("StopRun");
            StartCoroutine(KillRemains());
        } 

        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    // Makes the enemies look at and move toward the player, stopping if they are too close
    void MoveToPlayer()
    {
        transform.LookAt(new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
        if (distanceFromPlayer > minDistance)
        {
            enemyRb.AddForce(transform.forward * speed * Time.deltaTime);
        }
        else
        {
            enemyRb.AddForce(-transform.forward * speed * Time.deltaTime);
            animator.SetTrigger("Punch");
        }
        
    }

    // Damage the player on contact
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController robVar = player.GetComponent<PlayerController>();
            if (!robVar.invincible)
            {
                robVar.health -= damage;
                robVar.invincible = true;
            }
        }
    }

    private IEnumerator KillRemains() 
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            health -= 10;
        }
    }

}
