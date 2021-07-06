using Bonsai.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

// Copied, with minimal modifications, from https://github.com/kampff-lab/bonsai.neuroseeker
namespace Bonsai.ONIX.Design
{
    [Description("Draws a data created with " + nameof(CreateGraphicsDataModel) + " using a custom fragment shader.")]
    public class DrawGraphicsDataModel : Sink<GraphicsDataModel>
    {
        [TypeConverter(typeof(ShaderNameConverter))]
        public string ShaderName { get; set; }

        public string TextureUniformName { get; set; }

        public override IObservable<GraphicsDataModel> Process(IObservable<GraphicsDataModel> source)
        {
            return source.CombineEither(
                ShaderManager.ReserveShader(ShaderName).Do(shader =>
                {
                    var samplerLocation = GL.GetUniformLocation(shader.Program, TextureUniformName);
                    if (samplerLocation >= 0)
                    {
                        GL.Uniform1(samplerLocation, 0);
                    }
                }),
                (input, shader) =>
                {
                    shader.Update(() =>
                    {
                        foreach (var modelPart in input.ModelParts)
                        {
                            GL.ActiveTexture(TextureUnit.Texture0);
                            GL.BindTexture(TextureTarget.Texture2D, modelPart.Texture.Id);
                            modelPart.Mesh.Draw();
                        }
                    });
                    return input;
                });
        }
    }
}
