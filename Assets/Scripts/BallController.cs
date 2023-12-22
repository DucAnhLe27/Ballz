using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ballState
{
    AIM,
    FIRE,    
    ENDSHOT,
    NEXTLEVEL,
    ENDGAME
}

public class BallController : MonoBehaviour {
    
    public ballState currentBallState;
    public GameObject ballPrefab;   

    public Vector3 ballLunchPos;
    private Vector2 mouseStartPos;
    private Vector2 mouseEndPos;
    public Vector2 tmpVelocity;  

    private float ballVelocityX;
    private float ballVelocityY;
    public float constantSpeed;
    private ArrayList ballinScence;

    public GameObject Arrow;    
    private BrickMoveController brickMoveController;       

    public float ballWaitTime=0.5f;
    private float ballWaitTimeSeconds;

    public int numberOfBalls;
    public int extraBallPowerup;
    public int numberOfBallsToFire;    

    public Text numberOfBallsText;

    private float errorBallTimer;

    private BeginSpawnPoints spawnPoints;

    // Use this for initialization
    void Start () {
        
        brickMoveController = FindObjectOfType<BrickMoveController>();
        spawnPoints = FindObjectOfType<BeginSpawnPoints>();     
        currentBallState = ballState.AIM; 

        ballWaitTimeSeconds = ballWaitTime;
        numberOfBalls = 1;
        extraBallPowerup = 0;
        numberOfBallsToFire = numberOfBalls;
        numberOfBallsText.text = "" + numberOfBalls;
        errorBallTimer = 0f;
        
    }	
	
	void Update () {
        switch (currentBallState)
        {
            case ballState.AIM:
                GetComponent<SpriteRenderer>().enabled = true;
                if (Input.GetMouseButtonDown(0))
                {
                    MouseClicked();
                }
                if (Input.GetMouseButton(0))
                {
                    MouseDragged();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    ballinScence = new ArrayList();
                    ReleaseMouse();
                }
                break;
            case ballState.FIRE:
                if (numberOfBallsToFire > 0) {
                    if (ballWaitTimeSeconds <= 0) {
                        FireBall();
                    }

                }else {
                    GetComponent<SpriteRenderer>().enabled = false;
                    currentBallState = ballState.ENDSHOT;
                }

                break;            
            
            case ballState.ENDSHOT:
                errorBallTimer += Time.deltaTime;
                if (errorBallTimer > 10f) {
                    errorBallTimer = 0f;
                    if (ErrorBall() != -1) {
                        spawnPoints.ErrorBouncePowerup(ErrorBall());
                    }
                }
                
                 
                break;          

           
            case ballState.NEXTLEVEL:
                ballinScence.Clear();
                numberOfBalls += extraBallPowerup;
                numberOfBallsToFire = numberOfBalls;
                extraBallPowerup = 0;
                brickMoveController.BricksMove();                
                currentBallState = ballState.AIM;
                break;

            case ballState.ENDGAME:
                break;

            default:
                break;
        }
        numberOfBallsText.text = "" + numberOfBallsToFire;
        ballWaitTimeSeconds -= Time.deltaTime;
        
        errorBallTimer += Time.deltaTime;

    }
    
    public void MouseClicked()
    {
        mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);              
    }

    public void MouseDragged()
    {    
        Arrow.SetActive(true);        
        Vector2 tmpMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float diffX = mouseStartPos.x - tmpMousePosition.x;
        float diffY = mouseStartPos.y - tmpMousePosition.y;
        if(diffY<=0)
        {
            diffY = 0.01f;
        }
        float tmptheta = Mathf.Rad2Deg*(Mathf.Atan(diffX / diffY));
        float theta = Mathf.Clamp(tmptheta, -75, 75);
        Arrow.transform.rotation = Quaternion.Euler(0f, 0f, -theta);        
    }
    public void ReleaseMouse()
    {
        Arrow.SetActive(false);
        mouseEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //ballVelocityX = (mouseStartPos.x - mouseEndPos.x);
        ballVelocityY = (mouseStartPos.y - mouseEndPos.y);
        if (ballVelocityY <= 0) {
            ballVelocityY = 0.01f;
        }
        ballVelocityX = Mathf.Clamp((mouseStartPos.x - mouseEndPos.x), ballVelocityY * -Mathf.Tan(Mathf.Deg2Rad * 75), ballVelocityY * Mathf.Tan(Mathf.Deg2Rad * 75));
        tmpVelocity = new Vector2(ballVelocityX, ballVelocityY).normalized;       
        if (tmpVelocity == Vector2.zero)
        {
            return;
        }
        ballLunchPos = transform.position;
        currentBallState = ballState.FIRE;
    }

    private void FireBall() {
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        if (ball != null) {
            ball.transform.position = ballLunchPos;
            ballinScence.Add(ball);
            ball.SetActive(true);
            ball.GetComponent<Rigidbody2D>().velocity = tmpVelocity * constantSpeed;
            ballWaitTimeSeconds = ballWaitTime;
            numberOfBallsToFire--;        
        }
        ballWaitTimeSeconds = ballWaitTime;
        
    } 

    public int ErrorBall() {
        int row=-1;
        if (ballinScence.Count > 0) {
            foreach (GameObject ball in ballinScence) {
                if (ball != null) {
                    if (Mathf.Abs(ball.GetComponent<Rigidbody2D>().velocity.y) < 0.1) {
                        row = (int)ball.transform.position.y + 4;
                    }
                }
            }
            
        }
        return row;

    }
    
}
