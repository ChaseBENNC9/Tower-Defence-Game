//The purpose of this script is to manage the monster and it's current state.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterLevel
{
    public int cost;
    public GameObject visualization;
    public GameObject bullet;
    public float fireRate;
}
public class MonsterData : MonoBehaviour
{
    private MonsterLevel currentLevel;
    public List<MonsterLevel> levels;


    public MonsterLevel CurrentLevel
    {

    get { return currentLevel; }

    set
    {
        currentLevel = value;
        int currentLevelIndex = levels.IndexOf(currentLevel);

        GameObject levelVisualization = levels[currentLevelIndex].visualization;
        
        for (int i = 0; i < levels.Count; i++)
        {
        if (levelVisualization != null) //Hides the sprites for the other levels so only the current level sprite is shown.
            if (i == currentLevelIndex)         
            levels[i].visualization.SetActive(true);        
            else        
            levels[i].visualization.SetActive(false);           
        }
    }
    
    }

   void OnEnable() 
    {
        CurrentLevel = levels[0];
    } 

    public MonsterLevel GetNextLevel() //Returns the Next level available to the monster
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex+1];
        } 
        else
        {
            return null;
        }
    }

    public void IncreaseLevel() //Increases the level to the next value
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
        }
    }



}
