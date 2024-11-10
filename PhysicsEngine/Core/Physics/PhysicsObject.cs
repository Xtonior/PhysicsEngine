using OpenTK.Mathematics;
using PhysEngine.Core.Physics;

namespace PhysEngine.Core.Physics
{
    public class PhysicsObject
    {
        private Vector3d position;

        public Vector3d GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector3d value)
        {
            position = value;
        }

        private Vector3d velocity;

        public Vector3d GetVelocity()
        {
            return velocity;
        }

        public void SetVelocity(Vector3d value)
        {
            velocity = value;
        }

        private Vector3d acceleration;

        public Vector3d GetAcceleration()
        {
            return acceleration;
        }

        public void SetAcceleration(Vector3d value)
        {
            acceleration = value;
        }

        public Collider ObjectCollider;

        public double Mass;
        public double Restitution;

        public bool Immovable = false;

        public PhysicsObject(double mass, double restitution, bool immovable, Collider collider)
        {
            Mass = mass;
            Restitution = restitution;
            Immovable = immovable;

            ObjectCollider = collider;
            ObjectCollider.SetPhysicsObject(this);
        }

        public void ApplyPhysics(double deltaTime)
        {
            if (Immovable) return;

            SetVelocity(GetVelocity() + GetAcceleration() * deltaTime);
            SetPosition(GetPosition() + GetVelocity() * deltaTime);

            SetAcceleration(Vector3d.Zero);
        }

        public void AddForce(Vector3d force)
        {
            SetAcceleration(GetAcceleration() + force); 
        }
    }
}
