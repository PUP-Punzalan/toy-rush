using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PLR_Controller : MonoBehaviour
{
    // Component variables
    private TR_SpawnManager spawnManager;
    private Rigidbody playerRigidbody;
    private Animator playerAnim;
    public CharacterController controller;
    
    // Particle component variables
    public ParticleSystem speedVFX;
    public ParticleSystem crashVFX;
    public ParticleSystem heartVFX;
    public ParticleSystem shieldVFX;
    public ParticleSystem deathVFX;

    // Object variables
    private GameObject previousGrabbed;

    // Basic data type variables
    public float verticalInput;
    public float horizontalInput;
    private float slowDuration = 2.0f;
    public float movementSpeed;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public bool hasGrabbed = false;
    public bool canCrash = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnim = GameObject.Find("Player_Armature").GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<TR_SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isGameActive)
        {
            PlayerMovement();
        }

    }

    /* - This function will be called whenever the player collides/touches something. 
       - It can be called whenever the player collides/touches the obstacles/cars or points. */
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Nabangga, namatay, hit n run");

            Destroy(other.gameObject);

            if (canCrash)
            {
                crashVFX.Play();

                spawnManager.soundEffects.clip = spawnManager.crashSFX;
                spawnManager.soundEffects.Play();

                movementSpeed -= 2;
                canCrash = false;
                spawnManager.UpdateLives(-1);

                if (hasGrabbed)
                {
                    DropObject(previousGrabbed);

                }

                StartCoroutine(SlowdownCooldown());
            }    
        }

        if (((other.gameObject.CompareTag("Shape 1") || other.gameObject.CompareTag("Shape 2") || other.gameObject.CompareTag("Shape 3")) && !hasGrabbed))
        {
            Debug.Log("GRABBED");
            hasGrabbed = true;
            
            other.gameObject.GetComponent<PTS_PointsGrab>().enabled = true;
            previousGrabbed = other.gameObject;
        }
    }

    /* - This function will be called whenever the player touches something that have isTrigger turned on. 
       - Gives the player buff or power according to type of powerup.
       - Play the VFX. */
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Speed Powerup"))
        {
            Debug.Log("Powerup 1 (Speed) collected");
            movementSpeed += 2;
            spawnManager.hasPowerup = true;

            speedVFX.Play();
            spawnManager.soundEffects.clip = spawnManager.powerupUpSFX;
            spawnManager.soundEffects.Play();

            StartCoroutine(Powerup1Duration());
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Lives Powerup"))
        {
            Debug.Log("Powerup 2 (Lives) collected");
            spawnManager.UpdateLives(1);
            spawnManager.hasPowerup = true;

            heartVFX.Play();
            spawnManager.soundEffects.clip = spawnManager.powerupUpSFX;
            spawnManager.soundEffects.Play();

            StartCoroutine(Powerup2Duration());
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Invincible Powerup"))
        {
            Debug.Log("Powerup 3 (Invincible) collected");
            canCrash = false;
            spawnManager.hasPowerup = true;

            shieldVFX.Play();
            spawnManager.soundEffects.clip = spawnManager.powerupUpSFX;
            spawnManager.soundEffects.Play();

            StartCoroutine(Powerup3Duration());
            Destroy(other.gameObject);
        }
    }

    /* - This function will be called whenever the player collides with obstacle.
       - Reverts the player movement after a certain amount of time or delay. */
    IEnumerator SlowdownCooldown()
    {
        yield return new WaitForSeconds(slowDuration);
        movementSpeed += 2;
        canCrash = true;
    }

    /* - This function will be called when the player pressed Spacebar while holding a point.
       - Drops the object at the front of the player. */
    private void DropObject(GameObject obj)
    {
        Debug.Log("NABATO!");
        hasGrabbed = false;
        obj.transform.position = transform.position + (2 * transform.forward) + (3 * transform.up);

        obj.gameObject.GetComponent<PTS_PointsGrab>().enabled = false;
    }

    /* - This function will be called repeatedly when the game is active.
       - Gets the vertical and horizontal input and move the player accordingly. 
       - Play the accurate animation according to the state and expressions. */
    private void PlayerMovement()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(-horizontalInput, 0, -verticalInput).normalized;

        transform.position = new Vector3(transform.position.x, 0.21f, transform.position.z);

        if (hasGrabbed == false && (verticalInput != 0 || horizontalInput != 0) && spawnManager.life != 0)
        {
            playerAnim.SetBool("isRunning", true);
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isGrabbingRunning", false);
            playerAnim.SetBool("isGrabbingIdle", false);
            playerAnim.SetBool("isThrowing", false);
            playerAnim.SetBool("isDead", false);
        }
        
        if (hasGrabbed == false && (verticalInput == 0 && horizontalInput == 0) && spawnManager.life != 0)
        {
            playerAnim.SetBool("isIdle", true);
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isGrabbingRunning", false);
            playerAnim.SetBool("isGrabbingIdle", false);
            playerAnim.SetBool("isThrowing", false);
            playerAnim.SetBool("isDead", false);
        }
        
        if (hasGrabbed == true && (verticalInput != 0 || horizontalInput != 0) && spawnManager.life != 0)
        {
            playerAnim.SetBool("isGrabbingRunning", true);
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isGrabbingIdle", false);
            playerAnim.SetBool("isThrowing", false);
            playerAnim.SetBool("isDead", false);
        }

        if (hasGrabbed == true && (verticalInput == 0 && horizontalInput == 0) && spawnManager.life != 0)
        {
            playerAnim.SetBool("isGrabbingIdle", true);
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isGrabbingRunning", false);
            playerAnim.SetBool("isThrowing", false);
            playerAnim.SetBool("isDead", false);
        }

        if (hasGrabbed == true && Input.GetKeyDown(KeyCode.Space) && spawnManager.life != 0)
        {
            playerAnim.SetBool("isThrowing", true);
            playerAnim.SetBool("isGrabbingIdle", false);
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isGrabbingRunning", false);
            playerAnim.SetBool("isDead", false);
            DropObject(previousGrabbed);
        }

        if (spawnManager.life == 0)
        {
            Debug.Log("DEATH ANIMATION");

            playerAnim.SetBool("isDead", true);
            playerAnim.SetBool("isThrowing", false);
            playerAnim.SetBool("isGrabbingIdle", false);
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isGrabbingRunning", false);
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * movementSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0.15f, transform.position.z);
        }
    }

    // This function will be called whenever the player gets the bolt powerup.
    IEnumerator Powerup1Duration()
    {
        yield return new WaitForSeconds(8);
        movementSpeed -= 2;
        speedVFX.Stop();
        yield return new WaitForSeconds(8);
        spawnManager.hasPowerup = false;
    }

    // This function will be called whenever the player gets the heart powerup.
    IEnumerator Powerup2Duration()
    {
        yield return new WaitForSeconds(8);
        heartVFX.Stop();
        yield return new WaitForSeconds(8);
        spawnManager.hasPowerup = false;
    }

    // This function will be called whenever the player gets the shield powerup.
    IEnumerator Powerup3Duration()
    {
        yield return new WaitForSeconds(8);
        canCrash = true;
        shieldVFX.Stop();
        yield return new WaitForSeconds(8);
        spawnManager.hasPowerup = false;
    }

    // This function will be called whenever the player dies or runs out of lives.
    public void DeathEffect()
    {
        deathVFX.Play();
    }
}
