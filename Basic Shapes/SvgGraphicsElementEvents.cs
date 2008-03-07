using System;
using System.Collections.Generic;
using System.Text;

namespace Svg
{
    public abstract partial class SvgGraphicsElement
    {
        private static readonly object _mouseOverKey = new object();
        private static readonly object _mouseOutKey = new object();
        private static readonly object _focusKey = new object();
        private static readonly object _activeKey = new object();
        private static readonly object _clickKey = new object();

        public event EventHandler MouseOver
        {
            add { this.Events.AddHandler(_mouseOverKey, value); }
            remove { this.Events.RemoveHandler(_mouseOverKey, value); }
        }

        public event EventHandler MouseOut
        {
            add { this.Events.AddHandler(_mouseOutKey, value); }
            remove { this.Events.RemoveHandler(_mouseOutKey, value); }
        }

        public event EventHandler Focus
        {
            add { this.Events.AddHandler(_focusKey, value); }
            remove { this.Events.RemoveHandler(_focusKey, value); }
        }

        public event EventHandler Active
        {
            add { this.Events.AddHandler(_activeKey, value); }
            remove { this.Events.RemoveHandler(_activeKey, value); }
        }

        public event EventHandler Click
        {
            add { this.Events.AddHandler(_clickKey, value); }
            remove { this.Events.RemoveHandler(_clickKey, value); }
        }
    }
}