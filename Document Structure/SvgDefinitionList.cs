using System;
using System.Collections.Generic;
using System.Text;

namespace Svg
{
    public class SvgDefinitionList : SvgElement
    {
        public SvgDefinitionList()
        {
        }

        protected override void Render(System.Drawing.Graphics graphics)
        {
            // Do nothing. Children should NOT be rendered.
        }

        protected override string ElementName
        {
            get { return "defs"; }
        }
    }
}