using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using PhysEngine.Core.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PhysEngine.Core.Systems;
using OpenTK.Graphics.OpenGL4;
using PhysEngine.Core.Systems.Properties;
using PhysEngine.Core.Physics;
using System.Drawing;

namespace PhysEngine.Core
{
    public class Window : GameWindow
    {
        private OpenGLRenderer renderer;
        private GameObject boxGameObject;
        private GameObject floor;
        private GameObject triangleGameObject;
        private Matrix4 view;
        private Matrix4 projection;
        private Shader shader;

        private readonly float[] boxVertices =
        {
            // Position         Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly float[] floorVertices =
        {
            // Position         Texture coordinates
             3.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             3.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -3.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -3.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] boxIndices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private readonly float[] triangleVertices =
        {
            // Position         Texture coordinates
             0.0f,  0.5f, 0.0f, 1.0f, 1.0f, // top
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        };

        private readonly uint[] triangleIndices =
        {
            0, 1, 2
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            renderer = new OpenGLRenderer();
            renderer.Initialize();

            shader = new Shader("Resources/Shaders/shader.vert", "Resources/Shaders/shader.frag");

            Material matRed = new Material(new Vector3(1.0f, 0.0f, 0.0f));
            Material matGreen = new Material(new Vector3(0.0f, 1.0f, 0.0f));
            Material matBlue = new Material(new Vector3(0.0f, 0.0f, 1.0f));

            BoxCollider boxCollider = new BoxCollider(1.0, 1.0);
            PhysicsObject boxObject = new PhysicsObject(1.0, 2, false, boxCollider);

            BoxCollider floorCollider = new BoxCollider(7.0, 1.0);
            PhysicsObject floorObject = new PhysicsObject(1.0, 1, true, floorCollider);

            BoxCollider triangleCollider = new BoxCollider(1.0, 1.0);
            PhysicsObject triangleObject = new PhysicsObject(1.0, 2, false, triangleCollider);

            boxGameObject = renderer.CreateGameObject(matRed, shader, boxVertices, boxIndices, 6, boxObject);
            floor = renderer.CreateGameObject(matBlue, shader, floorVertices, boxIndices, 6, floorObject);
            triangleGameObject = renderer.CreateGameObject(matGreen, shader, triangleVertices, triangleIndices, 3, triangleObject);

            Game.AddNewPhysicsObject(ref boxGameObject);
            boxGameObject.ObjectPhysics.SetPosition(new Vector3d(-1.0f, 0.0f, 0.0f));

            Game.AddNewPhysicsObject(ref triangleGameObject);
            triangleGameObject.ObjectPhysics.SetPosition(new Vector3d(-0.8f, 2.0f, 0.0f));

            Game.AddNewPhysicsObject(ref floor);
            floor.ObjectPhysics.SetPosition(new Vector3d(0.0f, -5.0f, 0.0f));

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -30.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);
        }

        private float time;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            time += 4.0f * (float)e.Time;

            renderer.UpdateCamera(shader, view, projection);

            boxGameObject.Render(renderer);
            floor.Render(renderer);
            triangleGameObject.Render(renderer);

            SwapBuffers();
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            renderer.SetViewport(Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) 
            {
                return;
            }

            var input = KeyboardState;

            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            if (input.IsKeyDown(Keys.F))
            {
                boxGameObject.ObjectPhysics.SetVelocity(new Vector3(0.0f, 9.81f, 0.0f));
            }
        }
    }
}
