using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{ 
    //Hi Stevie i have PTSD so im gonna write this
    //This can account for things like defense etc. in the future
    //It can also be stuck on every enemy
    
    [SerializeField] private float health; //health should be self contained, no need to call outside of here
    public bool debugMessages = false;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health > 0) FindObjectOfType<AudioManager>().Play("damaged");

        if(debugMessages) //I'm dead serious bruh not after that night
        {
            print($"Object has taken {damage} damage and is at {health} health!");
        }

        if(health <= 0)
        {
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("death");

            if (debugMessages)
            {
                print("Object is dead!");
            }
        }
    }
}
