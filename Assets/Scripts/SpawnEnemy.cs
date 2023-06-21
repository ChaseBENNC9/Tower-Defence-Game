//the purpose of this script is to spawn in the enemies at the beginning of each wave. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave //the wave class holds the required info that can change between each wave
{
    public float spawnInterval = 2;
    public int maxEnemies = 20;
    public List<GameObject> enemyTypes; //A list that contains all the types of enemies
}

public class SpawnEnemy : MonoBehaviour //the spawnenemy class spawns in the enemies in each wave
{
    public Wave[] waves; //an array of waves 
    public int timeBetweenWaves = 5; //delay time between the end of one wave and the start of the next wave,
    private int enemyTypeIndex = 0; //The index for the enemy type list 
    private GameManagerBehaviour gameManager;

    private float lastSpawnTime;
    private int enemiesSpawned = 0;
    public GameObject[] waypoints; //an array of waypoints placed in the scene for the enemy to follow.
    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length)
        {
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;


            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || (enemiesSpawned != 0 && timeInterval > spawnInterval)) &&
            (enemiesSpawned < waves[currentWave].maxEnemies))
            {
                lastSpawnTime = Time.time;
                GameObject newEnemy = (GameObject)Instantiate(waves[currentWave].enemyTypes[enemyTypeIndex]);
                if (enemyTypeIndex < waves[currentWave].enemyTypes.Count - 1) //This will increment each time an enemy is spawned and change the type 
                {
                    enemyTypeIndex++;
                }
                else //when the index reaches the last entry in the list it will return to 0.
                {
                    enemyTypeIndex = 0;
                }
                newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints; //sets the waypoints of the spawned enemy to the waypoints in the scene.
                enemiesSpawned++;
            }

            if (enemiesSpawned == waves[currentWave].maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null) //When there are no enemies left and no more wll be spawned 
            {
                gameManager.Wave++; //increase the wave
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f); //increase gold by 10%
                enemiesSpawned = 0; //reset the enemy count
                lastSpawnTime = Time.time; //
            }
        }
        else //When the final wave has bee completed the game displays the game won text.
        {
            gameManager.gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }
}
