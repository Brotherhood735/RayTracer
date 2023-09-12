using HelloWorld;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src
{
    public class Renderer
    {
        private Color _backgroundColor;
        private Color _sphereColor;
        private Vector3 _origin;
        private Vector3 _lightDirection;

        public Renderer(Color backgroundColor, Color sphereColor, Vector3 origin, Vector3 lightDirection)
        { 
            _backgroundColor = backgroundColor;
            _sphereColor = sphereColor;
            _origin = origin;
            _lightDirection = lightDirection;
        }
        public void RenderPixel(Vector2 screenCoords)
        {
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();
            //invert the y axis
            var yInverted = height - screenCoords.Y;
            float aspectRatio = width / (float) height;
            int x = (int) (screenCoords.X);
            int y = (int) yInverted;
            
            float xNorm = ((float) screenCoords.X / width) * 2f - 1f;
            float yNorm = ((float) screenCoords.Y / height) * 2f - 1f;

            xNorm *= aspectRatio;
            float radius = 0.5f;
            Vector3 rayDirection = new Vector3(xNorm, yNorm, -1f);
            //Vector3 origin = new Vector3(0f, 0f, 2f);
            // (bx^2 + by^2 + bz^2)t^2 + (2(axbx + ayby + azbz))t + (ax^2 + ay^2 + az^2 - r^2) = 0
            // where
            // a = ray origin
            // b = ray direction
            // r = radius
            // t = hit distance
            Vector3 sphereOrigin = new Vector3(0f, 0f, -1f);
            _lightDirection = Vector3.Normalize(_lightDirection);
            float a = Vector3.Dot(rayDirection, rayDirection);
            float b = 2f * Vector3.Dot(_origin, rayDirection);
            float c = Vector3.Dot(_origin, _origin) - MathF.Pow(radius, 2);
            //b^2 - 4ac

            float delta = (b * b) - (4 * a * c);

            float t1 = 0f;
            float t2 = 0f;
            float closestVal = 0f;
            if (delta >= 0f)
            {
                float[] t = new float[2]
                {
                    -b + MathF.Sqrt(delta) - (4 * a * c),
                    -b - MathF.Sqrt(delta) - (4 * a * c)
                };
                for (int i = 0; i < 2; i++)
                {
                    Vector3 hitPoint = _origin + (rayDirection * t[i]);
                    Vector3 normal = hitPoint - sphereOrigin;
                    normal = Vector3.Normalize(normal);
                    var light = MathF.Abs(Vector3.Dot(normal, -_lightDirection));   
                    var R_gba =  (int)((float) _sphereColor.r * (float)light);
                    var r_G_ba = (int)((float) _sphereColor.g * (float)light);
                    var rg_B_a = (int)((float) _sphereColor.b * (float)light);
                    var rgb_A =  (int)((float) _sphereColor.a * (float)light);
                    Raylib.DrawPixel(x, y, new Color(R_gba, r_G_ba, rg_B_a, 255));
                }
            }
            else 
            {
                Raylib.DrawPixel(x, y, _backgroundColor);
            }
        }

        public void UpdateBackgroundColor(Color newColor)
        {
            _backgroundColor = newColor;
        }   
        public void UpdateSphereColor(Color newColor)
        {
            _sphereColor = newColor;
        }
        public void UpdateOrigin(Vector3 newOrigin)
        {
            _origin = newOrigin;
        }
        public void UpdateLightDirection(Vector3 newLightDirection)
        {
            _lightDirection = newLightDirection;
        }
    }
}
