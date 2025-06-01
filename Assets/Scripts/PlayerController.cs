
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // get a reference to the scenery controller script
    private SceneryController sceneryController;

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
    private float playerWalkInSpeed;

    // normally returned from player input
    private float playerHorizontalInput;

    // the direction of player's movement
    private Vector3 playerHorizontalDirection;

    // the amount of force to make player jump
    public float jumpForce = 12f;

    // the amount of gravity to add to the player
    private float gravityModifier;

    // check to see if player is on the ground
    [HideInInspector] public bool isOnGround = true;

    // get a reference to the audio source component
    private AudioSource audioPlayer;

    // in-game sounds
    public AudioClip jumpSound;

    public AudioClip crashSound;

    public AudioClip pickupSound;

    // temporary variables for normal speed
    [HideInInspector] public float sceneryNormalSpeed;

    [HideInInspector] public float playerNormalSpeed;

    // variables for turbo speed
    private float sceneryTurboMode;

    [HideInInspector] public float playerTurboMode;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ComponentInitialisation();

        GetComponentReferences();

        InitialisePlayer();
    }


    // Update is called once per frame
    void Update()
    {
        ReadyPlayerOne();
    }


    private void ComponentInitialisation()
    {
        // the amount of gravity to add to the player
        gravityModifier = 2f;
    }


    private void GetComponentReferences()
    {
        // set reference to game controller script
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        // set reference to scenery controller script
        sceneryController = GameObject.Find("Background").GetComponent<SceneryController>();

        // set reference to the audio source component
        audioPlayer = GetComponent<AudioSource>();

        // set reference to animator component
        playerAnimator = GetComponent<Animator>();

        // get the rigidbody component
        rb = GetComponent<Rigidbody>();

        // modify the amount of gravity to add to the player
        Physics.gravity *= gravityModifier;
    }


    public void InitialisePlayer()
    {
        // speed of player's walk
        playerWalkInSpeed = 0.7f;

        // moves player along the 'z' axis
        playerHorizontalDirection = Vector3.forward;

        // value normally from player input
        playerHorizontalInput = 1f;

        // player game start position
        playerStartPosition = new Vector3(0f, 0f, 0f);

        // player walk-on start position
        walkStartPosition = new Vector3(-6, 0f, 0f);

        // reset player start position and rotation
        transform.position = walkStartPosition;

        // reset player animation to run static
        playerAnimator.SetBool("Death_b", false);

        // start playing the dirt particle effect
        dirtParticle.Play();

        // set player and scenery to normal speed
        sceneryNormalSpeed = sceneryController.sceneryNormalSpeed;

        playerTurboMode = 1f;

        playerAnimator.SetFloat("playerTurboMode", playerTurboMode);

        // turbo mode speed
        sceneryTurboMode = 20f;
    }


    private void ReadyPlayerOne()
    {
        // if the game is not in play yet
        if (!gameController.inPlay)
        {
            // then return
            return;
        }

        // otherwise
        else
        {
            // start the player walk-in
            WalkDontRun();
        }
    }


    private void WalkDontRun()
    {
        // move the player right along the 'x' axis
        transform.Translate(playerWalkInSpeed * Time.deltaTime * playerHorizontalInput * playerHorizontalDirection);


        // if the player's 'x' position is greater than or equal to the 'x' position of the game start position
        if (transform.position.x >= playerStartPosition.x)
        {
            // then set the player's 'x'position to the 'x' postion of the game start position
            transform.position = new Vector3(playerStartPosition.x, transform.position.y, transform.position.z);

            // set game over to false
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
                // and we press the jump key (space bar)
                if (Input.GetButtonDown("Jump"))
                {
                    // make player jump
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                    // and set flag to indicate player has jumped
                    isOnGround = false;

                    // stop playing the dirt particle effect
                    dirtParticle.Stop();

                    // play jump sound
                    audioPlayer.PlayOneShot(jumpSound, 1f);

                    // play the player's jump animation
                    playerAnimator.SetTrigger("Jump_trig");
                }

                // if we press the right arrow key
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    // set player and scenery speed turbo mode
                    sceneryController.sceneryNormalSpeed = sceneryTurboMode;

                    playerTurboMode = 4.3f;

                    playerAnimator.SetFloat("playerTurboMode", playerTurboMode);
                }

                // if the right arrow key is released
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    // reset player and scenery speed
                    sceneryController.sceneryNormalSpeed = sceneryNormalSpeed;

                    playerTurboMode = 1f;

                    playerAnimator.SetFloat("playerTurboMode", playerTurboMode);
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
            // set the game over flag
            gameController.gameOver = true;

            // set the in play flag to false
            gameController.inPlay = false;

            // play crash sound
            audioPlayer.PlayOneShot(crashSound, 1f);

            // stop playing the dirt particle effect
            dirtParticle.Stop();

            // play the explosion particle effect
            explosionParticle.Play();

            // play player death animation
            playerAnimator.SetBool("Death_b", true);

            playerAnimator.SetInteger("DeathType_int", 1);

            // reset player and scenery to normal speed
            sceneryController.sceneryNormalSpeed = sceneryNormalSpeed;

            playerTurboMode = 1f;

            playerAnimator.SetFloat("playerTurboMode", playerTurboMode);


            // pause before showing the start screen
            StartCoroutine(GameOver());
        }


        // if player has collided with a pickup
        if (collidingObject.gameObject.CompareTag("Pickup"))
        {
            // play pickup sound
            audioPlayer.PlayOneShot(pickupSound, 1f);

            // destroy the pickup object
            Destroy(collidingObject.gameObject);

            // update score
            gameController.score++;

            gameController.UpdateScore();
        }
    }


} // end of class
