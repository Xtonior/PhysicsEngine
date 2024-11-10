using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysEngine.Core.Physics
{
    internal class CircleCollider : Collider
    {
        public double Radius;

        public CircleCollider(double radius)
        {
            Radius = radius;
        }
    }
}
