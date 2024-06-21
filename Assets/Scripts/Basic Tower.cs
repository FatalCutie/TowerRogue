using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    public override void Attack()
    {
        if (towerRotates) RotateTowardsTarget();

        print("Shooting!");
        attackTimer = attackCooldown;
    }

}
