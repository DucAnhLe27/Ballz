using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickManager : MonoBehaviour
{
    public Gradient gradient;
    private SpriteRenderer brickRenderer;

    public int brickHealth;
    private Text brickHealthText;

    private SoundManager sound;
    private ScoreManager score;
    public GameObject brickDestroyParticle;

    // Use this for initialization
    void Start()
    {
        brickHealth = BeginSpawnPoints.level;
        brickRenderer = GetComponent<SpriteRenderer>();
        brickRenderer.color = gradient.Evaluate(Random.Range(0f, 1f));

        brickHealth = BeginSpawnPoints.level;
        brickHealthText = GetComponentInChildren<Text>();

        sound = FindObjectOfType<SoundManager>();
        score = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        brickHealthText.text = "" + brickHealth;
        if (brickHealth < 1)
        {
            score.IncreaseScore();
            Destroy(this.gameObject);
            Instantiate(brickDestroyParticle, transform.position, Quaternion.identity);
        }
    }

    private void TakeDamage(int damage)
    {
        brickHealth -= damage;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            sound.HitSound();
            TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            TakeDamage(1);
        }
    }

}
