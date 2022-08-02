using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Utilities
{
    public class TexLib
    {
        public static void LoadTexture(string pImagePath, out int tex_id_out, int filtering = 0)
        {
            GL.GenTextures(1, out tex_id_out);

            GL.BindTexture(TextureTarget.Texture2D, tex_id_out);

            Bitmap image = default(Bitmap);
            BitmapData imageData = default(BitmapData);

            image = new Bitmap(pImagePath);

            LoadTexture(image, imageData, filtering);
        }

        public static void LoadTexture(Bitmap image, BitmapData imageData, int filtering)
        {
            imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, imageData.Width, imageData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, imageData.Scan0);

            if (filtering == 1)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            }

            image.UnlockBits(imageData);
            image.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static void AddTexture(MemoryStream ms, int tex_id_out, int filtering = 0)
        {
            GL.DeleteTexture(tex_id_out);
            GL.BindTexture(TextureTarget.Texture2D, tex_id_out);

            Bitmap image = default(Bitmap);
            BitmapData imageData = default(BitmapData);

            image = new Bitmap(Image.FromStream(ms));

            AddTexture(image, imageData, filtering);
        }

        public static void AddTexture(string pImagePath, int tex_id_out, int filtering = 0)
        {
            GL.DeleteTexture(tex_id_out);
            GL.BindTexture(TextureTarget.Texture2D, tex_id_out);

            Bitmap image = default(Bitmap);
            BitmapData imageData = default(BitmapData);

            image = new Bitmap(pImagePath);

            AddTexture(image, imageData, filtering);
        }

        public static void AddTexture(Bitmap image, BitmapData imageData, int tex_id_out, int filtering = 0)
        {
            try
            {
                imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            catch
            {
                return;
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, imageData.Width, imageData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, imageData.Scan0);

            if (filtering == 1)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            }

            image.UnlockBits(imageData);
            image.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}