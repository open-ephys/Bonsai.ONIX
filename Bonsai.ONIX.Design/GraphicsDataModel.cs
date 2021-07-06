using System.Collections.Generic;

// Copied, with minimal modifications, from https://github.com/kampff-lab/bonsai.neuroseeker
namespace Bonsai.ONIX.Design
{
    public class GraphicsDataModel
    {
        readonly List<GraphicsDataModelPart> modelParts;

        public GraphicsDataModel(IEnumerable<GraphicsDataModelPart> parts)
        {
            modelParts = new List<GraphicsDataModelPart>(parts);
        }

        public IList<GraphicsDataModelPart> ModelParts
        {
            get { return modelParts; }
        }
    }
}
