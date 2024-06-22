using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region Definitions

    public enum TowerTargetPriority{First, Last, Strongest, Weakest}

    [Header("Tower Aspects")]
    
    public float attackCooldown = 1f; //This value is used to reset timer, should not be adjusted
    [NonSerialized] public float attackTimer; 
    public float range;
    public bool towerRotates;
    public TowerTargetPriority targetPriority;

    [Header("Tower Set Up")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Bullet Settings")] //Scriptable object material?
    public float damage;
    public float bulletSpeed;
    
    
    [Header("Testing Variables")]
    [NonSerialized] public List<GameObject> enemiesInRange = new List<GameObject>();
    [NonSerialized] public GameObject curTarget = null;

    #endregion

    void Start()
    {
        attackTimer = attackCooldown;

        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        myCollider.radius = range; //this does not update in real time
        if(range <= 0)
        {
            Debug.LogWarning("A Towers range is 0! Did you configure it?");
        }
    }
    
    void Update()
    {
        if(curTarget == null && enemiesInRange.Count != 0)
        {
            curTarget = UpdateTarget();
        }

        if (attackTimer <= 0 && curTarget != null)
        {
            Attack();
        }

        attackTimer -= Time.deltaTime;
    }

    public virtual void Attack() //Towers will attack in their unique way
    {
        if (towerRotates) RotateTowardsTarget();

        GameObject proj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Bullet>().Initialize(curTarget, damage, bulletSpeed);
        //FindObjectOfType<AudioManager>().Play("bigShoot1");

        attackTimer = attackCooldown;
    } 

    #region Rotation, TriggerEnters

    public void RotateTowardsTarget()
    {
        Vector3 offset = curTarget.transform.position - transform.position;
        // Construct a rotation as in the y+ case.
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, offset);
        // Apply a compensating rotation that twists x+ to y+ before the rotation above.
        transform.rotation = rotation * Quaternion.Euler(0, 0, 180); //This assumes the "front" of the sprite is at the bottom
    }

    private void OnTriggerEnter2D (Collider2D other) //Log enemies going in
    //THIS NEEDS A RIGIDBODY AND COLLIDER2D ATTACHED TO OTHER TO WORK
    {
        if(other.CompareTag("Enemy")) enemiesInRange.Add(other.gameObject);
    }

    private void OnTriggerExit2D (Collider2D other) //Remove enemies going out
    {
        if(other.CompareTag("Enemy"))enemiesInRange.Remove(other.gameObject);

        if(curTarget == other.gameObject) curTarget = null; //reset target
    }
    #endregion

    public GameObject UpdateTarget()
    {
        //enemiesInRange.RemoveAll(x => x == null); //I cannot say for certain what this code does

        //runs to save resources if choice is obvious
        if(enemiesInRange.Count == 0)
            return null;
        if(enemiesInRange.Count == 1)
            return enemiesInRange[0];

        switch(targetPriority)
        {
            case TowerTargetPriority.First:
            {
                return enemiesInRange[0];
            }
            case TowerTargetPriority.Last:
            {
                int i = -1;
                foreach(GameObject enemyGO in enemiesInRange)
                {
                    i++;
                }
                return enemiesInRange[i];
            }
            case TowerTargetPriority.Strongest:
            {
                // GameObject strongest = null;
                // float strongestHealth = -1;
                
                // foreach(GameObject enemyGO in enemiesInRange)
                // {
                //     Enemy enemy = enemyGO.GetComponent<Enemy>();
                //     if(enemy.health > strongestHealth)
                //     {
                //         strongest = enemyGO;
                //         strongestHealth = enemy.health;
                //     }
                // }
                // return strongest;
                return null;
            }
            case TowerTargetPriority.Weakest:
            {
                // GameObject weakest = null;
                // float weakestHealth = Mathf.Infinity;

                // foreach(GameObject enemyGO in curEnemiesInRange)
                // {
                //     Enemy enemy = enemyGO.GetComponent<Enemy>();
                //     if(enemy.health < weakestHealth)
                //     {
                //         weakest = enemyGO;
                //         weakestHealth = enemy.health;
                //     }
                // }
                // return weakest;
                return null;
            }
        }
        return null;
    }
}
