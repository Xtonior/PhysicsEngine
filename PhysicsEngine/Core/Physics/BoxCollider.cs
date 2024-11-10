using OpenTK.Mathematics;
using PhysEngine.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysEngine.Core.Physics
{
    public class BoxCollider : Collider
    {
        public double Length;
        public double Height;

        public BoxCollider(double length, double height)
        {
            Length = length;
            Height = height;
        }

        public Vector2d Min => new Vector2d(ObjectPhysics.GetPosition().X - Length / 2, ObjectPhysics.GetPosition().Y - Height / 2);
        public Vector2d Max => new Vector2d(ObjectPhysics.GetPosition().X + Length / 2, ObjectPhysics.GetPosition().Y + Height / 2);
    }
}
