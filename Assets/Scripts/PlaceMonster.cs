//The purpose of ths script is to place the monster units onto specified places in the scene and upgrade them when available

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMonster : MonoBehaviour
{
    public GameObject monsterPrefab;
    public GameObject monster;
    private GameManagerBehaviour gameManager;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }


    private bool CanPlaceMonster() //checks the player current gold to see if a monster can be created
    {
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }

    private bool CanUpgradeMonster() //checks the player gold and the current level of the selected monster to see if the monster  can be upgraded
    {
        if (monster != null)
        {
            MonsterData monsterData = monster.GetComponent<MonsterData>();
            MonsterLevel nextLevel = monsterData.GetNextLevel();
            if (nextLevel != null)
            {
            return gameManager.Gold >= nextLevel.cost;
            }
        }
        return false;
    }

    void OnMouseUp() //when the mouse button is clicked
    {
        if (CanPlaceMonster())
        {
            monster = (GameObject)Instantiate(monsterPrefab, transform.position,Quaternion.identity); //creates a new instance of the monster in the current position and deducts the gold
            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;

            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
        }
        else if (CanUpgradeMonster()) //upgrades the clicked monster and deducts the gold.
        {
            monster.GetComponent<MonsterData>().IncreaseLevel();
            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

}
