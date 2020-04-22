using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spatial_Partitions
{
    public class Grid
    {
        // need this to convert from world coordinate pos to cell pos
        int cellSize;

        // this is the actual grid, where a soldier is in each cell
        // each individual soldier links to other soldiers in the same cell
        Soldier[,] cells;

        // init the grid
        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;

            int numberOfCells = mapWidth / cellSize;

            cells = new Soldier[numberOfCells, numberOfCells];
        }

        // add a unit to the grid
        public void Add(Soldier soldier)
        {
            // determine which grid cell the soldier is in
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            // add the soldier to the front of the list for the cell its in
            soldier.previousSoldier = null;
            soldier.nextSoldier = cells[cellX, cellZ];

            // associates this cell with this soldier
            cells[cellX, cellZ] = soldier;

            if (soldier.nextSoldier != null)
            {
                // set this soldier to be the previous soldier of the next one
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        // get the closest enemy from the grid
        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            // determine which grid cell the friendly soldier is in
            int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

            // get the first enemy in grid
            Soldier enemy = cells[cellX, cellZ];

            // find the closest soldier of all in the linked list
            Soldier closestSoldier = null;

            float bestDistSqr = Mathf.Infinity;

            // loop through the linked list
            while (enemy != null)
            {
                // the distance sqr between the soldier and this enemy
                float distSqr = (enemy.soldierTrans.position -
                    friendlySoldier.soldierTrans.position).sqrMagnitude;

                // if this distance is better than the previous best distance,
                // then we have found an enemy that's closer
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestSoldier = enemy;
                }

                // get the next enemy in the list
                enemy = enemy.nextSoldier;
            }

            return closestSoldier;
        }

        // a soldier in the grid has moved, so see if we need to update
        public void Move(Soldier soldier, Vector3 oldPos)
        {
            // see which cell it was in
            int oldCellX = (int)(oldPos.x / cellSize);
            int oldCellZ = (int)(oldPos.z / cellSize);

            // see which cell it is in now
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            // if it didn't change cell, we are done
            if (oldCellX == cellX && oldCellZ == cellZ)
            {
                return;
            }

            // unlink it from the list of its old cell
            if (soldier.previousSoldier != null)
            {
                soldier.previousSoldier.nextSoldier = soldier.nextSoldier;
            }

            if (soldier.nextSoldier != null)
            {
                soldier.nextSoldier.previousSoldier = soldier.previousSoldier;
            }

            // if its the head of a list, remove it
            if (cells[oldCellX, oldCellZ] == soldier)
            {
                cells[oldCellX, oldCellZ] = soldier.nextSoldier;
            }

            // add it back to the grid at its new cell
            Add(soldier);
        }
    }
}
