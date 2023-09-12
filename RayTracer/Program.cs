using Raylib_cs;
using RayTracer.src;
using System.Numerics;
using rlImGui_cs;
using ImGuiNET;
namespace HelloWorld;

class Program
{
    public static int WIDTH = 500;
    public static int HEIGHT = 1000;

    public static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.InitWindow(WIDTH, HEIGHT, "Hello World");
        rlImGui.Setup();
        Vector3 camTrans = new Vector3(0f, 0f, 2f);
        Vector3 lightDirection = new Vector3(-1f, -1f, 1f);
        Vector4 sphereColor = new Vector4(Color.GREEN.r, Color.GREEN.g, Color.GREEN.b, Color.GREEN.a);
        Vector4 backgroundColor = new Vector4(Color.BLACK.r, Color.BLACK.g, Color.BLACK.b, Color.BLACK.a);
        var renderer = new Renderer(backgroundColor.ToColor(false), sphereColor.ToColor(false), camTrans, lightDirection);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);
            rlImGui.Begin();
            if(ImGui.SliderFloat3("Camera Transform", ref camTrans, -3f, 3f))
            {
                renderer.UpdateOrigin(camTrans);
            }
            if(ImGui.SliderFloat3("Light Direction", ref lightDirection, -3f, 3f))
            {
                renderer.UpdateLightDirection(lightDirection);
            }
            if (ImGui.ColorPicker4("Sphere Color", ref sphereColor))
            {
                renderer.UpdateSphereColor(sphereColor.ToColor(true));
            };
            if (ImGui.ColorPicker4("Background Color", ref backgroundColor))
            {
                renderer.UpdateBackgroundColor(backgroundColor.ToColor(true));
            };
            for (int y = 0; y < Raylib.GetScreenHeight(); y++)
            {
                for (int x = 0; x < Raylib.GetScreenWidth(); x++)
                {
                    renderer.RenderPixel(new Vector2(x, y));
                }
            }
            rlImGui.End();
            //Raylib.DrawText("Hello, world!", 12, 12, 20, Color.BLACK);
            Raylib.EndDrawing();
            //Console.WriteLine("Finished Drawing Background");
        }

        Raylib.CloseWindow();
    }
}
public static class ExtensionMethods
{
    public static Color ToColor(this Vector4 color, bool normalized)
    {
        float multiplier = 1;
        if (normalized)
        {
            multiplier = 255;
        }
        return new Color(
            (int)Math.Clamp(color.X * multiplier, 0f, 255f),
            (int)Math.Clamp(color.Y * multiplier, 0f, 255f),
            (int)Math.Clamp(color.Z * multiplier, 0f, 255f),
            (int)Math.Clamp(color.W * multiplier, 0f, 255f));
    }
}