using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using PhysEngine.Core.Systems;
using PhysEngine.Core.Systems.Properties;

namespace PhysEngine.Core.Rendering
{
    public class OpenGLRenderer
    {
        private int vertexArrayObject;
        private int vertexBufferObject;
        private int elementBufferObject;

        public void Initialize()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        public void ClearScreen()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public GameObject CreateGameObject(Material material, Shader shader, float[] vertices, uint[] indices, int elementsCount, Physics.PhysicsObject physics)
        {
            BindVAO();
            BindVBO(vertices);
            BindEBO(indices);
            DefineAttribs(shader);

            GameObject gameObject = new GameObject(shader, material, vertexArrayObject, elementsCount, physics);

            Cleanup();

            return gameObject;
        }

        public void BindVAO()
        {
            // Vertex Array Object
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
        }

        public void BindVBO(float[] vertices)
        {
            // Vertex Buffer Object
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public void BindEBO(uint[] indices)
        {
            // Element Buffer Object
            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        public void DefineAttribs(Shader shader)
        {
            // Define the position attribute
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        }

        public void DefineTexCoords(Shader shader)
        {
            // Define the texture coordinate attribute
            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void Cleanup()
        {
            // Unbind VAO for cleanup
            GL.BindVertexArray(0);
        }

        public void UpdateCamera(Shader shader, Matrix4 view, Matrix4 projection)
        { 
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            ClearScreen();
        }

        public void SetViewport(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        public void DrawElements(int vertexArrayObject, int elementCount)
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, elementCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}
