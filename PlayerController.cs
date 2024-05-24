using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private LayerMask groundMask;

    private Camera mainCamera;
    private WallController wallController;

    private float horizontalInput;
    private float verticalInput;
    public Transform rightWall;
    public Transform leftWall;
    public Transform frontWall;
    public Transform backWall;
    private Rigidbody playerRb;
    public float speed = 20.0f;
    public GameObject crosshair;

    public bool rocketIsOnCooldown = false;
    public bool selfDamageIsOnCooldown = true;
    public Transform rocketSpawnPoint;
    public Quaternion rocketSpawnDirection;
    public GameObject rocketPrefab;
    public GameObject rocketHolder;
    public int rocketCount = 2;
    public float health = 100;
    public UnityEngine.UI.Image healthBar;

    public bool invincible = false;
    private bool invFailSafe = true;

    public ParticleSystem deathExplosion;
    public GameManager gameManager;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerRb = GetComponent<Rigidbody>();
        wallController = GameObject.Find("Front Wall").GetComponent<WallController>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        invFailSafe = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Left and right movement
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed, Space.World);

        // Forward and backward movement
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * speed, Space.World);
        
        if (horizontalInput != 0 || verticalInput != 0) 
        {
            animator.SetBool("Run", true);
            animator.SetBool("Idle", false);
        }
        else 
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", true);
        }

        // Shoots a rocket when LMB is clicked
        if (Input.GetMouseButtonDown(0) && rocketCount > 0 && !rocketIsOnCooldown)
        {
            Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation, rocketHolder.transform);
            animator.SetTrigger("Shoot");
            --rocketCount;
        }

        // Reload launcher when ammo = 0
        if (rocketCount == 0)
        {
            rocketIsOnCooldown = true;
            Invoke("RocketReload", 0.5f);
            Invoke("RocketCooldown", 1);
        }

        // Kill player on death
        if (health < 0.1f) 
        {
            Instantiate(deathExplosion, transform.position, transform.rotation);
            gameManager.GameOver();
            Destroy(gameObject);
        }

        if (transform.position.y > 2)
        {
            transform.position = new Vector3 (transform.position.x, 2, transform.position.z);
            playerRb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }

        // Activates "facemouse" every update
        facemouse();

        // Activates wall collisions every update
        WallCollision();

        if (invincible && invFailSafe)
        {
            invFailSafe = false;
            StartCoroutine(Invincibility());
        }

        healthBar.fillAmount = health / 100;

        

    }

    // Resets the cooldown for shooting rockets
    private void RocketReload()
    {
        rocketCount = 2;
    }

    private void RocketCooldown()
    {
        rocketIsOnCooldown = false;
    }

    // Creates a raycast that hits the ground and turns the player toward its end position
    private void facemouse()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength)) 
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));

            // Sets the crosshair's position to the mouse location
            crosshair.transform.position = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
        }
    }

    // Gives the player a second of invincibility after taking damage
    public IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(1);
        invincible = false;
        invFailSafe = true;
    }

    // Makes Player "collide" with walls
    void WallCollision()
    {
        if (transform.position.x > rightWall.position.x - 1)
        {
            transform.position = new Vector3(rightWall.position.x - 1, transform.position.y, transform.position.z);
        }

        if (transform.position.x < leftWall.position.x + 1)
        {
            transform.position = new Vector3(leftWall.position.x + 1, transform.position.y, transform.position.z);
        }

        if (transform.position.z < backWall.position.z + 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, backWall.position.z + 1);
        }

        if (transform.position.z > frontWall.position.z - 1 && wallController.wallDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, frontWall.position.z - 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            playerRb.velocity = Vector3.zero;
        }
    }

}
