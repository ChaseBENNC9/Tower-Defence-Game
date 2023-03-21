//The purpose of this script is to allow the monsters to shoot at enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemies : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    private float lastShotTime;
    private MonsterData monsterData;
    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        monsterData = gameObject.GetComponentInChildren<MonsterData>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange) //tests every enemy within its range and find's the closest enemy to the goal. Sets that enemy as it's current target.
        {
            float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }

        if (target != null)//when the monster has a target and the cooldown is 0, start shooting at enemy.
        {
            if (Time.time - lastShotTime > monsterData.CurrentLevel.fireRate)
            {
            Shoot(target.GetComponent<Collider2D>());
            lastShotTime = Time.time;
            }
            Vector3 direction = gameObject.transform.position - target.transform.position;
            gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2 (direction.y, direction.x) * 180 / Mathf.PI, new Vector3 (0, 0, 1));
        }
    }


    void OnTriggerEnter2D (Collider2D other) //adds any enemies that enter the monster's trigger collider to the within range list
    {
        if (other.gameObject.tag.Equals("Enemy"))
            enemiesInRange.Add(other.gameObject);
    }

    void OnTriggerExit2D (Collider2D other) //removes an enemy that leaves the trigger from the within range list
    {
        if (other.gameObject.tag.Equals("Enemy"))
            enemiesInRange.Remove(other.gameObject);
    }


    void Shoot(Collider2D target) //shoots a bullet at the set target
    {
        GameObject bulletPrefab = monsterData.CurrentLevel.bullet;
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        BulletBehaviour bulletComp = newBullet.GetComponent<BulletBehaviour>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        Animator animator = monsterData.CurrentLevel.visualization.GetComponent<Animator>();
        animator.SetTrigger("fireShot");
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }
}
