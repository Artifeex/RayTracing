using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.IO;

namespace RayTracing
{
    public class Window : GameWindow
    {
        //используем стандартный подход к рисованию объектов, чтобы нарисовать квадрат, заполняющий экран
        //VBO буфер
        private int vertexBufferObject;
        //VAO буфер
        private int vertexArrayObject;
        
        private float[] vertices = {
            -1f, -1f, 0.0f, -1f, 1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0f
        };
        //переменная, для хранения шейдерной программы
        private Shader shader;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            //создание шейдерной программы
            shader = new Shader("..\\..\\..\\Shaders\\shader.vert", "..\\..\\..\\Shaders\\shader.frag");

            //генерация VAO буфера
            vertexArrayObject = GL.GenVertexArray();
            //привязываем только что созданный VAO буфер
            GL.BindVertexArray(vertexArrayObject);

            //Создаем VBO буфер
            vertexBufferObject = GL.GenBuffer();
            //привязываем VBO буфер
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            //передаем данные вершин в VBO буфер
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, 
            BufferUsageHint.StaticDraw);

            //показываем, как нашей шейдерной программе интепретировать данные, находящиеся внутри VBO
            var posAtrib = shader.GetAttribLocation("vPosition");
            GL.EnableVertexAttribArray(posAtrib);
            GL.VertexAttribPointer(posAtrib, 3, VertexAttribPointerType.Float, false, 0, 0);

            //Установка фона
            GL.ClearColor(1.0f, 0.0f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            //Передача информации, связанной с камерой в шейдер.
            shader.SetVector3("uCamera.Position", new Vector3(0f, 0f, -7f));
            shader.SetVector3("uCamera.View", new Vector3(0.0f, 0.0f, 1.0f));
            shader.SetVector3("uCamera.Up", new Vector3(0.0f, 1.0f, 0.0f));
            shader.SetVector3("uCamera.Side", new Vector3(1.0f, 0.0f, 0.0f));

            //Вычисления для сохранения пропорций
            Vector2 v = new Vector2(1.0f);
            if (Size.X >= Size.Y)
                v.X = Size.X / (float)Size.Y;
            else
                v.Y = Size.Y / (float)Size.X;
            //Где больше x или y у v, там возникает сжатие больше
            shader.SetVector2("uCamera.Scale", v);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Очистка буферов цвета и глубины
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Привязка буфера вершин
            GL.BindVertexArray(vertexArrayObject);

            //Указание использовать данную шейдерную программу
            shader.Use();
            Vector2 v = new Vector2(1.0f);
            if (Size.X >= Size.Y)
                v.X = Size.X / (float)Size.Y;
            else
                v.Y = Size.Y / (float)Size.X;
            shader.SetVector2("uCamera.Scale", v);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            SwapBuffers();
        }


    }
}
