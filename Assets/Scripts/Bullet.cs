using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Definitions

    public static Bullet bullet;
    void Awake() => bullet = this;
    [SerializeField] private GameObject target = null;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if(target == null) Destroy(gameObject); //TODO: Bullets just fly offscreen when target dies?

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        
        //Checks Collision
        if(Vector2.Distance(transform.position, target.transform.position) < .5f) //TODO: Figure out stable variable so it looks nice (unique one for each enemy?)
        {
            target.GetComponent<Health>().TakeDamage(damage);  
            Destroy(gameObject);
        }
    }

    public void Initialize(GameObject target, float damage, float moveSpeed)
    {
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
    }

#region unused
    // void OnCollisionEnter2D(Collision2D coll)
    // {
    //         // coll.gameObject.GetComponent<Health>().TakeDamage(1);
    //         // Destroy(gameObject);

    //     if(coll.gameObject == target && Vector2.Distance(transform.position, coll.gameObject.transform.position) < 1f)
    //     {
    //         coll.gameObject.GetComponent<Health>().TakeDamage(damage);  
    //         Destroy(gameObject);
    //     }
    // }
    #endregion
}
