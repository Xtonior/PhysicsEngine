using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using PhysEngine.Core.Physics;
using PhysEngine.Core.Rendering;
using PhysEngine.Core.Systems.Properties;

namespace PhysEngine.Core.Systems
{
    public class GameObject : IRenderable
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public PhysicsObject ObjectPhysics { get; set; }

        private readonly Shader shader;
        private readonly Material material;
        private readonly int vertexArrayObject;
        private readonly int elementCount;

        public GameObject(Shader shader, Material material, int vertexArrayObject, int elementCount, PhysicsObject objectPhysics)
        {
            this.shader = shader;
            this.material = material;
            this.vertexArrayObject = vertexArrayObject;
            this.elementCount = elementCount;

            ObjectPhysics = objectPhysics;
        }

        public void Render(OpenGLRenderer renderer)
        {
            Position = (Vector3)ObjectPhysics.GetPosition();

            int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "objectColor");
            GL.Uniform3(vertexColorLocation, material.Color);

            shader.Use();

            var model = Matrix4.CreateRotationX(Rotation.X) *
                        Matrix4.CreateRotationY(Rotation.Y) *
                        Matrix4.CreateRotationZ(Rotation.Z) *
                        Matrix4.CreateTranslation(Position);

            shader.SetMatrix4("model", model);
            renderer.DrawElements(vertexArrayObject, elementCount);
        }
    }
}
