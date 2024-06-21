using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    public float attackCooldown = 1f; //This value is used to reset timer, should not be adjusted
    public float attackTimer; 
    public GameObject curTarget;
    public float range;
    public bool towerRotates;
    public enum TowerTargetPriority{First, Last, Strongest, Weakest}
    public TowerTargetPriority targetPriority;

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

    public abstract void Attack(); //Towers will attack in their unique way


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
        enemiesInRange.Add(other.gameObject);
    }

    private void OnTriggerExit2D (Collider2D other) //Remove enemies going out
    {
        enemiesInRange.Remove(other.gameObject);
        if(curTarget == other.gameObject) curTarget = null; //removes out of range enemy
    }

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
