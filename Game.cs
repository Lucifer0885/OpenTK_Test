using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Circle
{
    public class Game : GameWindow
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default,
                   new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _shader = new Shader("vertex.glsl", "fragment.glsl");

            float[] vertices = GenerateCircleVertices(100);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 100);

            GL.BindVertexArray(0);
            GL.UseProgram(0);

            Context.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            _shader.Dispose();
        }

        private static float[] GenerateCircleVertices(int segments)
        {
            float[] vertices = new float[segments * 2];
            float angleStep = MathF.PI * 2.0f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * angleStep;
                vertices[i * 2] = MathF.Cos(angle);
                vertices[i * 2 + 1] = MathF.Sin(angle);
            }

            return vertices;
        }
    }
}