using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{

    private GameObject enemy;
    private float radius = 5.0f;
    private float explosionPower = 50000.0f;
    private float damage;
    private int selfDamageRed = 2;

    void Start()
    {
        Vector3 explosionPos = transform.position;
        StartCoroutine(DestroySelf());
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Damages an enemy if it is in the explosion.
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            enemy = other.gameObject;
            damage = Vector3.Distance(transform.position, enemy.transform.position) * 5;

            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyBehavior enemyScrp = enemy.GetComponent<EnemyBehavior>();
                enemyScrp.health -= damage;
            }
            else
            {
                PlayerController playerScrp = enemy.GetComponent<PlayerController>();
                if (!playerScrp.invincible)
                {
                    playerScrp.health -= damage / selfDamageRed;
                    playerScrp.invincible = true;
                }
            }
            
            enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, radius, 3.0f);
            
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
