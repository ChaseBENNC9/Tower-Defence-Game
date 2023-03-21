//The purpsoe of this script is to manage the main game. it manages the player's gold, current wave and current Health.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public Text goldLabel;
    public Text waveLabel;
    public bool gameOver = false;
    public Text healthLabel;
    public GameObject[] healthIndicator;  
    public GameObject[] nextWaveLabels;
    private int gold; //The player's gold, this can be used to create or upgrade new units (Monsters) to help the player complete the wave, can be awarded for destroying enemies or winning a wave
    public int Gold 
    {

        get {return gold; }

        set 
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
        }
    }


    private int wave; //The current wave, This increases after each wave is completed and increases the difficulty over time.
    public int Wave 
    {
        get { return wave; }
        set
        {
            wave = value;
            if (!gameOver)
            {
            for (int i = 0; i < nextWaveLabels.Length; i++)
            {
                nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
            }
            }
            waveLabel.text = "WAVE: " + (wave + 1);
        }
    }


    private int health; //The Health of the player, when this reaches 0 the game will be over.
    public int Health
    {
        get { return health; }
        set
        {
            if (value < health)    
            Camera.main.GetComponent<CameraShake>().Shake();
            
            health = value;
            healthLabel.text = "HEALTH: " + health;

            if (health <= 0 && !gameOver)
            {
            gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }

            for (int i = 0; i < healthIndicator.Length; i++)
            {
            if (i < Health)
                healthIndicator[i].SetActive(true);
            else
                healthIndicator[i].SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start() //initial values
    {
        Gold = 1000;
        Wave = 0;
        Health = 5;

    }

}
