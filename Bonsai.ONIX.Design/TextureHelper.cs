using OpenCV.Net;
using OpenTK.Graphics.OpenGL4;
using System;

// Copied, with minimal modifications, from https://github.com/kampff-lab/bonsai.neuroseeker
namespace Bonsai.ONIX.Design
{
    internal static class TextureHelper
    {
        public static void UpdateTexture(int texture, PixelInternalFormat internalFormat, Mat image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            PixelFormat pixelFormat;
            switch (image.Channels)
            {
                case 1: pixelFormat = PixelFormat.Red; break;
                case 2: pixelFormat = PixelFormat.Rg; break;
                case 3: pixelFormat = PixelFormat.Bgr; break;
                case 4: pixelFormat = PixelFormat.Bgra; break;
                default: throw new ArgumentException("Image has an unsupported number of channels.", nameof(image));
            }

            int pixelSize;
            PixelType pixelType;
            switch (image.Depth)
            {
                case Depth.U8:
                    pixelSize = 1;
                    pixelType = PixelType.UnsignedByte;
                    break;
                case Depth.S8:
                    pixelSize = 1;
                    pixelType = PixelType.Byte;
                    break;
                case Depth.U16:
                    pixelSize = 2;
                    pixelType = PixelType.UnsignedShort;
                    break;
                case Depth.S16:
                    pixelSize = 2;
                    pixelType = PixelType.Short;
                    break;
                case Depth.S32:
                    pixelSize = 4;
                    pixelType = PixelType.Int;
                    break;
                case Depth.F32:
                    pixelSize = 4;
                    pixelType = PixelType.Float;
                    break;
                default: throw new ArgumentException("Image has an unsupported pixel bit depth.", nameof(image));
            }

            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.PixelStore(PixelStoreParameter.UnpackRowLength, image.Step / (pixelSize * image.Channels));
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, image.Cols, image.Rows, 0, pixelFormat, pixelType, image.Data);
            GC.KeepAlive(image);
        }
    }
}
