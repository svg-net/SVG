using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// A wrapper for a paint server which isn't defined currently in the parse process, but
    /// should be defined by the time the image needs to render.
    /// </summary>
    public class SvgDeferredPaintServer : SvgPaintServer
    {
        private bool _serverLoaded = false;
        private SvgPaintServer _concreteServer;

        public SvgDocument Document { get; set; }
        public string DeferredId { get; set; }

        public SvgDeferredPaintServer() { }
        public SvgDeferredPaintServer(SvgDocument document, string id)
        {
            this.Document = document;
            this.DeferredId = id;
        }

        private void EnsureServer()
        {
            if (!_serverLoaded)
            {
                _concreteServer = this.Document.IdManager.GetElementById(this.DeferredId) as SvgPaintServer;
                _serverLoaded = true;
            }
        }

        public override System.Drawing.Brush GetBrush(SvgVisualElement styleOwner, float opacity)
        {
            EnsureServer();
            return _concreteServer.GetBrush(styleOwner, opacity);
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDeferredPaintServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgDeferredPaintServer;
            newObj.Document = this.Document;
            newObj.DeferredId = this.DeferredId;
            return newObj;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SvgDeferredPaintServer;
            if (other == null)
                return false;

            return this.Document == other.Document && this.DeferredId == other.DeferredId;
        }

        public override int GetHashCode()
        {
            if (this.Document == null || this.DeferredId == null) return 0;
            return this.Document.GetHashCode() ^ this.DeferredId.GetHashCode();
        }

        public override string ToString()
        {
            return (_serverLoaded ? _serverLoaded.ToString() : string.Format("deferred: {0}", this.DeferredId));
        }

        public static T TryGet<T>(SvgPaintServer server) where T : SvgPaintServer
        {
            var deferred = server as SvgDeferredPaintServer;
            if (deferred == null)
            {
                return server as T;
            }
            else
            {
                deferred.EnsureServer();
                return deferred._concreteServer as T;
            }
        }
    }
}