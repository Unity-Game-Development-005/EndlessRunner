
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    // get a reference to the rigidbody
    private Rigidbody rb;

    // get a reference to the animator component
    [HideInInspector] public Animator playerAnimator;

    // get a reference to the exposion particle component
    public ParticleSystem explosionParticle;

    // get a reference to the dirt particle component
    public ParticleSystem dirtParticle;

    // game start position of player
    private Vector3 playerStartPosition;

    // start position for player walk ing
    private Vector3 walkStartPosition;

    // speed of player's walk
    private float playerWalkSpeed;

    // normally returned from player input
    private float playerHorizontalInput;

    // the direction of player's movement
    private Vector3 playerHorizontalDirection;

    // the amount of force to make player jump
    public float jumpForce = 10f;

    // the amount of gravity to add to the player
    private float gravityModifier;

    // check to see if player is on the ground
    public bool isOnGround = true;

    // get a reference to the audio source component
    private AudioSource audioPlayer;

    // in-game sounds
    public AudioClip jumpSound;

    public AudioClip crashSound;

    public AudioClip pickupSound;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialise();

        GetComponentReferences();
    }


    // Update is called once per frame
    void Update()
    {
        ReadyPlayerOne();
    }


    private void GetComponentReferences()
    {
        // set reference to game controller script
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        // set reference to the audio source component
        audioPlayer = GetComponent<AudioSource>();

        // set reference to animator component
        playerAnimator = GetComponent<Animator>();

        // get the rigidbody component
        rb = GetComponent<Rigidbody>();

        // modify the amount of gravity to add to the player
        Physics.gravity *= gravityModifier;
    }


    private void Initialise()
    {
        // speed of player's walk
        playerWalkSpeed = 0.7f;

        // moves player along the 'z' axis
        playerHorizontalDirection = Vector3.forward;

        // value normally from player input
        playerHorizontalInput = 1f;

        // the amount of gravity to add to the player
        gravityModifier = 2f;

        // player game start position
        playerStartPosition = new Vector3(0f, 0f, 0f);

        // player walk-on start position
        walkStartPosition = new Vector3(-6, 0f, 0f);

        // reset player start position and rotation
        transform.position = walkStartPosition;
    }


    private void ReadyPlayerOne()
    {
        if (!gameController.inPlay)
        {
            return;
        }

        else
        { 
            WalkDontRun();
        }
    }


    private void WalkDontRun()
    {
        // move the player up along the 'z' axis
        transform.Translate(playerWalkSpeed * Time.deltaTime * playerHorizontalInput * playerHorizontalDirection);


        // if the player's 'x' position is greater than the 'x' position of the right boundary
        if (transform.position.x >= playerStartPosition.x)
        {
            // then set the player to the right boundary's 'x' position
            transform.position = new Vector3(playerStartPosition.x, transform.position.y, transform.position.z);

            gameController.gameOver = false;

            StartGame();
        }
    }


    private void StartGame()
    {
        // if we are playing the game
        if (!gameController.gameOver)
        {
            // and the player is on the ground
            if (isOnGround)
            {
                // if we press the jump key (space bar)
                if (Input.GetButtonDown("Jump"))
                {
                    // make player jump
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                    // and set flag to indicate player has jumped
                    isOnGround = false;

                    // the player has jumped
                    // so stop playing the dirt particle effect
                    dirtParticle.Stop();

                    // play jump sound
                    audioPlayer.PlayOneShot(jumpSound, 1f);

                    // play the player's jump animation
                    playerAnimator.SetTrigger("Jump_trig");
                }
            }
        }
    }


    private IEnumerator GameOver()
    {
        // stop playing the dirt particle effect
        dirtParticle.Stop();

        // a short pause before showing start screen
        yield return new WaitForSeconds(3f);

        // show the start screen
        gameController.GameOver();
    }


    private void OnCollisionEnter(Collision collidingObject)
    {
        // if the player is on the ground
        if (collidingObject.gameObject.CompareTag("Ground"))
        {
            // then set the isOnGround flag to true
            isOnGround = true;

            // play the dirt particle effect
            dirtParticle.Play();
        }


        // if player has collided with an obstacle
        if (collidingObject.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("COLLIDED WITH OBSTACLE");
            // set the game over flag
            /*gameController.gameOver = true;

            // play crash sound
            audioPlayer.PlayOneShot(crashSound, 1f);

            // stop playing the dirt particle effect
            dirtParticle.Stop();

            // play the explosion particle effect
            explosionParticle.Play();

            // play player death animation
            playerAnimator.SetBool("Death_b", true);

            playerAnimator.SetInteger("DeathType_int", 1);*/

            gameController.inPlay = false;

            // pause before show start screen
            StartCoroutine(GameOver());
        }


        // if player has collided with a pickup
        if (collidingObject.gameObject.CompareTag("Pickup"))
        {
            // play pickup sound
            audioPlayer.PlayOneShot(pickupSound, 1f);

            // destroy the object
            Destroy(collidingObject.gameObject);

            // update score
            gameController.score++;

            gameController.UpdateScore();
        }
    }


} // end of class
