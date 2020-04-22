using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spatial_Partitions
{
    public class Enemy : Soldier
    {
        // the position the soldier is heading for when moving
        Vector3 currentTarget;

        // the position the soldier had before it moved,
        //so we can see if it should change cell
        Vector3 oldPos;

        // the width of the map to generate random coordinated within the map
        float mapWidth;

        Grid grid; // the grid

        // init enemy
        public Enemy(GameObject soldierObj, float mapWidth, Grid grid)
        {
            // save what we need to save
            this.soldierTrans = soldierObj.transform;
            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();
            this.mapWidth = mapWidth;
            this.grid = grid;

            // add this unit to the grid
            grid.Add(this);

            // init oldPos
            oldPos = soldierTrans.position;

            this.walkSpeed = 5f;

            // give it a random coordinate to move towards
            GetNewTarget();
        }

        // move the cube randomly across the map
        public override void Move()
        {
            // move towards the target
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);

            // see if the cube has moved to another cell
            grid.Move(this, oldPos);

            // save the old position
            oldPos = soldierTrans.position;

            // if the soldier has reached the target, find a new target
            if ((soldierTrans.position - currentTarget).magnitude < 1f)
            {
                GetNewTarget();
            }
        }

        // give the enemy a new target to move and rotate towards
        void GetNewTarget()
        {
            currentTarget = new Vector3(Random.Range(0f, mapWidth), 0.5f,
                Random.Range(0f, mapWidth));

            // rotate towards the target
            soldierTrans.rotation = Quaternion.LookRotation(currentTarget -
                soldierTrans.position);
        }
    }
}
