using Bonsai.Shaders;

// Copied, with minimal modifications, from https://github.com/kampff-lab/bonsai.neuroseeker
namespace Bonsai.ONIX.Design
{
    public class GraphicsDataModelPart
    {
        public GraphicsDataModelPart(Mesh mesh, Texture texture)
        {
            Mesh = mesh;
            Texture = texture;
        }

        public Mesh Mesh { get; private set; }

        public Texture Texture { get; private set; }
    }
}
