using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysEngine.Core.Physics
{
    public abstract class Collider
    {
        public PhysicsObject ObjectPhysics { get; internal set; }

        public void SetPhysicsObject(PhysicsObject physicsObject)
        {
            ObjectPhysics = physicsObject;
        }
    }
}
