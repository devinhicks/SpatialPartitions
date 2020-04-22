using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spatial_Partitions
{
    // the soldier base class for enemies and friendlies
    public class Soldier
    {
        // to change material
        public MeshRenderer soldierMeshRenderer;

        // to move the soldier
        public Transform soldierTrans;

        // the speed the soldier is walking with
        protected float walkSpeed;

        // linked list of all the soldiers
        public Soldier previousSoldier;
        public Soldier nextSoldier;

        // the enmy doesn't need any outside information
        public virtual void Move() { }

        // the friendly has to move which soldier is the closest
        public virtual void Move(Soldier soldier) { }
    }
}
