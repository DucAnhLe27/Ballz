using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrickMoveController : MonoBehaviour {

    private BeginSpawnPoints spawnPoints;

    private void Start() {
        spawnPoints = FindObjectOfType<BeginSpawnPoints>();
    }

    public void BricksMove() {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        spawnPoints.BrickMove();
        spawnPoints.PlaceBricks();
    }


}
