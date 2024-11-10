using OpenTK.Mathematics;
using PhysEngine.Core.Physics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PhysEngine.Core.Physics
{
    public class PhysicsEngine
    {
        public Vector3d Gravity { get; private set; }

        private List<PhysicsObject> objects = new List<PhysicsObject>();
        private Thread physicsThread;
        private bool isRunning = true;
        private int physicsFPS = 60; 
        private double timeStep;

        public PhysicsEngine(Vector3d gravity)
        {
            Gravity = gravity;

            timeStep = 1.0 / physicsFPS;
        }

        public void Start()
        {
            physicsThread = new Thread(PhysicsLoop);
            physicsThread.Start();

            if (physicsThread.IsAlive)
            {
                Console.WriteLine("Main Physics Engine thread is working now");
            }
        }

        public void Stop()
        {
            isRunning = false;
            physicsThread.Join();
        }

        public bool CheckCircleCollision(PhysicsObject circleA, PhysicsObject circleB)
        {
            var distance = (circleA.GetPosition() - circleB.GetPosition()).Length;
            var radiusSum = (circleA.ObjectCollider as CircleCollider).Radius + (circleA.ObjectCollider as CircleCollider).Radius;

            return distance <= radiusSum;
        }

        public bool CheckAABBCollision(BoxCollider boxA, BoxCollider boxB)
        {
            return (boxA.Max.X > boxB.Min.X && boxA.Min.X < boxB.Max.X &&
                    boxA.Max.Y > boxB.Min.Y && boxA.Min.Y < boxB.Max.Y);
        }

        public Vector3d GetCollisionNormal(BoxCollider boxA, BoxCollider boxB)
        {
            double xOverlap = Math.Min(boxA.Max.X, boxB.Max.X) - Math.Max(boxA.Min.X, boxB.Min.X);
            double yOverlap = Math.Min(boxA.Max.Y, boxB.Max.Y) - Math.Max(boxA.Min.Y, boxB.Min.Y);

            if (xOverlap < yOverlap)
            {
                return new Vector3d(Math.Sign(boxB.ObjectPhysics.GetPosition().X - boxA.ObjectPhysics.GetPosition().X), 0, 0).Normalized();
            }
            else
            {
                return new Vector3d(0, Math.Sign(boxB.ObjectPhysics.GetPosition().Y - boxA.ObjectPhysics.GetPosition().Y), 0).Normalized();
            }
        }


        public void ResolvePenetration(PhysicsObject a, PhysicsObject b, Vector3d normal, float penetrationDepth)
        {
            Vector3d correction = normal * (penetrationDepth * 0.5);

            if (!a.Immovable)
                a.SetPosition(a.GetPosition() - correction);

            if (!b.Immovable)
                b.SetPosition(b.GetPosition() + correction);
        }

        public void ResolveCollision(PhysicsObject a, PhysicsObject b, Vector3d normal)
        {
            Vector3d relativeVelocity = b.GetVelocity() - a.GetVelocity();
            double velocityAlongNormal = Vector3d.Dot(relativeVelocity, normal);

            if (velocityAlongNormal > 0)
                return;

            double restitution = Math.Min(a.Restitution, b.Restitution);
            double impulseMagnitude = -(1 + restitution) * velocityAlongNormal;
            impulseMagnitude *= (a.Mass * b.Mass) / (a.Mass + b.Mass);

            Vector3d impulse = impulseMagnitude * normal;

            if (a.Immovable)
            {
                b.SetVelocity(b.GetVelocity() + impulse / b.Mass);
            }
            else if (b.Immovable)
            {
                a.SetVelocity(a.GetVelocity() - impulse / a.Mass);
            }
            else
            {
                a.SetVelocity(a.GetVelocity() - impulse / a.Mass);
                b.SetVelocity(b.GetVelocity() + impulse / b.Mass);
            }
        }

        private void PhysicsLoop()
        {
            while (isRunning)
            {
                var startTime = DateTime.Now;

                Update(timeStep);

                var elapsed = (DateTime.Now - startTime).TotalSeconds;
                var waitTime = timeStep - elapsed;

                if (waitTime > 0)
                    Thread.Sleep((int)(waitTime * 1000));
            }
        }

        private void Update(double deltaTime)
        {
            foreach (var obj in objects)
            {
                obj.ApplyPhysics(deltaTime);
                obj.AddForce(Gravity);
            }

            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = i + 1; j < objects.Count; j++)
                {
                    if (objects[i].ObjectCollider == null || objects[j].ObjectCollider == null) continue;

                    BoxCollider colliderA = objects[i].ObjectCollider as BoxCollider;
                    BoxCollider colliderB = objects[j].ObjectCollider as BoxCollider;

                    Vector3d normal = GetCollisionNormal(colliderA, colliderB);

                    if (CheckAABBCollision(colliderA, colliderB))
                    {
                        ResolvePenetration(colliderA.ObjectPhysics, colliderB.ObjectPhysics, normal, 0.03f);
                        ResolveCollision(colliderA.ObjectPhysics, colliderB.ObjectPhysics, normal);
                    }
                }
            }
        }

        public void AddObject(PhysicsObject obj)
        {
            objects.Add(obj);
        }
    }
}
