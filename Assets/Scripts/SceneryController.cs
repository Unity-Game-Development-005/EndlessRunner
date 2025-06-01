
using UnityEngine;


public class SceneryController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    // background starting position
    private Vector3 sceneryStartPosition;

    // width of background
    private float sceneryWidth;

    // background movement speed
    [HideInInspector] public float sceneryNormalSpeed;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitialiseScenery();

        // set reference to game controller script
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        // get the starting position of the background
        sceneryStartPosition = transform.position;

        // get the width of the background from the size of the attached collider
        // and divide this value by two
        sceneryWidth = GetComponent<BoxCollider>().size.x / 2;
    }


    // Update is called once per frame
    void Update()
    {
        // if the game is running
        if (!gameController.gameOver)
        {
            // move the scenery
            transform.Translate(sceneryNormalSpeed * Time.deltaTime * Vector3.left);

            // if the 'x' position of the scenery is less than the scenery's starting position 'x' - the scenery's width
            if (transform.position.x < sceneryStartPosition.x - sceneryWidth)
            {
                // then move the scenery back to its starting position
                transform.position = sceneryStartPosition;
            }
        }
    }


    private void InitialiseScenery()
    {
        sceneryNormalSpeed = 3f;
    }


} // end of class
