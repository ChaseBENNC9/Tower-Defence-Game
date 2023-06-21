//The purpose of this script is to Manage the Behaviour of the Bullet Gameobject. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 10;
    public int damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    private Vector3 normalizeDirection;
    private GameManagerBehaviour gameManager;
    // Start is called before the first frame update
    void Start()
    {
        normalizeDirection = (targetPosition - startPosition).normalized;
        GameObject gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update() //Movement of the bullet
    {
        transform.position += normalizeDirection * speed * Time.deltaTime;
    }


    void OnTriggerEnter2D(Collider2D other) //When the bullet enters a trigger collider it checks if it is an enemy and decreases it's health
    {
        target = other.gameObject;
        if (target.tag.Equals("Enemy"))
        {
            Transform healthBarTransform = target.transform.Find("HealthBar");
            HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
            healthBar.currentHealth -= damage; //danage the enemy when the bullet collides with it

            if (healthBar.currentHealth <= 0)
            {
                Destroy(target); //when the enemy has 0 health it is destoryed
                AudioSource audioSource = target.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                gameManager.Gold += 50; //Rewards the player with gold
            }
            Destroy(gameObject); //destroys the bullet gameobject
        }
    }
}
