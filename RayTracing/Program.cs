using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace RayTracing
{
    public static class Program
    {
        private static void Main()
        {
            //Установка настроек окна программы
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(805, 730),
                Title = "RayTracing",
                Flags = ContextFlags.ForwardCompatible
            };
            GameWindowSettings settings = new GameWindowSettings
            {
                RenderFrequency = 1,
                UpdateFrequency = 1
            };
            //Запуск окна программы
            using var window = new Window(settings, nativeWindowSettings);
            window.Run();
        }
    }
}
