using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spatial_Partitions
{
    public class Friendly : Soldier
    {
        // init friendly
        public Friendly(GameObject soldierObj, float mapWidth)
        {
            this.soldierTrans = soldierObj.transform;
            this.walkSpeed = 2f;
        }

        // move towards the closest enemy - will always move within its grid
        public override void Move(Soldier closestEnemy)
        {
            // rotate towards the closest enemy
            soldierTrans.rotation = Quaternion.LookRotation(
                closestEnemy.soldierTrans.position - soldierTrans.position);
            // move towards the closest enemy
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
        }
    }
}
