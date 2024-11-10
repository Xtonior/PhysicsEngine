using PhysEngine.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysEngine.Core.Systems.Properties
{
    internal class Rigidbody
    {
        public float Mass;

        private GameObject gameObject;
        private Collider collider;

        public Rigidbody(GameObject gameObject, float mass, Collider collider)
        {
            Mass = mass;

            this.gameObject = gameObject;
            this.collider = collider;
        }

        public void AddForce(Vector3 force)
        {

        }

        public void Update()
        {

        }
    }
}
