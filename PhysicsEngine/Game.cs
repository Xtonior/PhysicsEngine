using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysEngine.Core.Physics;
using PhysEngine.Core.Systems;

namespace PhysEngine
{
    static internal class Game
    {
        private static PhysicsEngine physicsEngine;

        public static void Start()
        {
            physicsEngine = new PhysicsEngine(new OpenTK.Mathematics.Vector3d(0.0, -9.81, 0.0));

            physicsEngine.Start();
        }

        public static void AddNewPhysicsObject(ref GameObject gameObject)
        {
            physicsEngine.AddObject(gameObject.ObjectPhysics);
        }
    }
}
