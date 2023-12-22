using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {
    
    private BallController ball;
    public GameObject endGamePanel;

    // Use this for initialization
    void Start () {
        ball = FindObjectOfType<BallController>();
        endGamePanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "SquareBrick" || other.gameObject.tag == "TriangleBrick1" || other.gameObject.tag == "TriangleBrick2" ||
            other.gameObject.tag == "TriangleBrick3" || other.gameObject.tag == "TriangleBrick4") {
            ball.currentBallState = ballState.ENDGAME;
            endGamePanel.SetActive(true);
        } else if (other.gameObject.tag == "ExtraBallPowerUp") {
            Destroy(other.gameObject);
        }

    }

    public void RestartBtn() {
        SceneManager.LoadScene("MainScene");
    }
    public void HomeBtn()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void QuitBtn() {
        Application.Quit();
    }
}
