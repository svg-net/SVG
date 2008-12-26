using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("tspan")]
    public class SvgTextSpan : SvgText
    {
        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SvgUnit X
        {
            get { return base.X; }
            set { base.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SvgUnit Y
        {
            get { return base.Y; }
            set { base.Y = value; }
        }
    }
}