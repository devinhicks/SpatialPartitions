using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spatial_Partitions
{
    public class ProController : MonoBehaviour
    {
        public GameObject friendlyObj;
        public GameObject enemyObj;

        // change materials to detect which enemy is the closest
        public Material enemyMaterial;
        public Material closestEnemyMaterial;

        // to get a cleaner workspace, parent all soldier to these empties
        public Transform enemyParent;
        public Transform friendlyParent;

        // store all soldiers in these lists
        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();

        // save the closest enemies to easier change back its material
        List<Soldier> closestEnemies = new List<Soldier>();

        // grid data
        float mapWidth = 50f;
        int cellSize = 10;

        // number of soldiers on each team
        int numOfSoldiers = 100;

        // the spatial partition grid
        Grid grid;

        // Start is called before the first frame update
        void Start()
        {
            // create a new grid
            grid = new Grid((int)mapWidth, cellSize);

            // add random enemies and friendlies and store them in a list
            for (int i = 0; i < numOfSoldiers; i++)
            {
                // give enemy a random position
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f,
                    Random.Range(0f, mapWidth));

                // create new enemy
                GameObject newEnemy = Instantiate(enemyObj, randomPos,
                    Quaternion.identity) as GameObject;

                // add the enemy to a list
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));

                // parent it
                newEnemy.transform.parent = enemyParent;

                //

                // give friendly a random position
                randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f,
                    Random.Range(0f, mapWidth));

                // create new friendly
                GameObject newFriendly = Instantiate(friendlyObj, randomPos,
                    Quaternion.identity) as GameObject;

                // add the enemy to a list
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));

                // parent it
                newFriendly.transform.parent = friendlyParent;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // to switch to development
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
            //

            // move the enemies
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                enemySoldiers[i].Move();
            }

            // reset materials of the closest enemies
            for (int i = 0; i < closestEnemies.Count; i++)
            {
                closestEnemies[i].soldierMeshRenderer.transform.localScale =
                    new Vector3(1f, 1f, 1f);
            }

            // reset the list with closest enemies
            closestEnemies.Clear();
            
            // for each friendly, find the closest enemy & change its color & chase it
            for (int i = 0; i < friendlySoldiers.Count; i++)
            {
                Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);

                // if we found an enemy
                if (closestEnemy != null)
                {
                    // change material
                    closestEnemy.soldierMeshRenderer.transform.localScale *= 1.2f;

                    closestEnemies.Add(closestEnemy);

                    // move the friendly in the direction of the enemy
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
        }
    }
}
