using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public abstract class SvgFilterPrimitive
    {
        private string _in;
        private string _in2;
        private string _result;
        private ISvgFilter _owner;

        protected ISvgFilter Owner
        {
            get { return this._owner; }
        }

        public string In
        {
            get { return this._in; }
            set { this._in = value; }
        }

        public string In2
        {
            get { return this._in2; }
            set { this._in2 = value; }
        }

        public string Result
        {
            get { return this._result; }
            set { this._result = value; }
        }

        public SvgFilterPrimitive(ISvgFilter owner, string input)
        {
            this._in = input;
            this._owner = owner;
        }

        public abstract Bitmap Apply();
    }
}