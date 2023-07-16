using System.Collections.Generic;
using System.Linq;

namespace Svg
{
    public partial class SvgTextRef : SvgTextBase
    {
        internal override IEnumerable<ISvgNode> GetContentNodes()
        {
            var refText = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) as SvgTextBase;
            IEnumerable<ISvgNode> contentNodes = null;

            if (refText == null)
            {
                contentNodes = base.GetContentNodes();
            }
            else
            {
                contentNodes = refText.GetContentNodes();
            }

            contentNodes = contentNodes.Where(o => !(o is ISvgDescriptiveElement));

            return contentNodes;
        }
    }
}
