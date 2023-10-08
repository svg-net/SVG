using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// A wrapper for a paint server which isn't defined currently in the parse process,
    /// but should be defined by the time the image needs to render.
    /// </summary>
    [TypeConverter(typeof(SvgDeferredPaintServerFactory))]
    public partial class SvgDeferredPaintServer : SvgPaintServer
    {
        private bool _serverLoaded;

        private SvgPaintServer _concreteServer;
        private SvgPaintServer _fallbackServer;

        [Obsolete("Will be removed.")]
        public SvgDocument Document { get; set; }

        public string DeferredId { get; set; }

        public SvgPaintServer FallbackServer { get; private set; }

        public SvgDeferredPaintServer()
        {
        }

        [Obsolete("Will be removed.")]
        public SvgDeferredPaintServer(SvgDocument document, string id)
        {
            Document = document;
            DeferredId = id;
        }

        /// <summary>
        /// Initializes new instance of <see cref="SvgDeferredPaintServer"/> class.
        /// </summary>
        /// <param name="id">&lt;FuncIRI&gt;, &lt;IRI&gt; or &quot;currentColor&quot;.</param>
        public SvgDeferredPaintServer(string id)
            : this(id, null)
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="SvgDeferredPaintServer"/> class.
        /// </summary>
        /// <param name="id">&lt;FuncIRI&gt;, &lt;IRI&gt; or &quot;currentColor&quot;.</param>
        /// <param name="fallbackServer">&quot;none&quot;, &quot;currentColor&quot; or <see cref="SvgColourServer"/> server.</param>
        public SvgDeferredPaintServer(string id, SvgPaintServer fallbackServer)
        {
            DeferredId = id;
            FallbackServer = fallbackServer;
        }

        public void EnsureServer(SvgElement styleOwner)
        {
            if (!_serverLoaded && styleOwner != null)
            {
                if (DeferredId == "currentColor")
                {
                    var colorElement = styleOwner.ParentsAndSelf.OfType<SvgElement>().FirstOrDefault(
                        e => e.Color != None && e.Color != NotSet && e.Color != Inherit);

                    _concreteServer = colorElement?.Color;
                }
                else
                {
                    _concreteServer = styleOwner.OwnerDocument.IdManager.GetElementById(DeferredId) as SvgPaintServer;

                    _fallbackServer = FallbackServer;
                    if (_fallbackServer == null)
                        _fallbackServer = None;
                    else if (!(_fallbackServer is SvgColourServer ||
                        (_fallbackServer is SvgDeferredPaintServer && string.Equals(((SvgDeferredPaintServer)_fallbackServer).DeferredId, "currentColor"))))
                        _fallbackServer = Inherit;
                }
                _serverLoaded = true;
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDeferredPaintServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgDeferredPaintServer;

            newObj.DeferredId = DeferredId;
            newObj.FallbackServer = FallbackServer?.DeepCopy() as SvgPaintServer;
            return newObj;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SvgDeferredPaintServer;
            if (other == null)
                return false;

            return DeferredId == other.DeferredId;
        }

        public override int GetHashCode()
        {
            return DeferredId == null ? 0 : DeferredId.GetHashCode();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(DeferredId))
                return string.Empty;
            if (FallbackServer == null)
                return DeferredId;
            return new StringBuilder(DeferredId).Append(" ").Append(FallbackServer.ToString()).ToString();
        }

        public static T TryGet<T>(SvgPaintServer server, SvgElement parent) where T : SvgPaintServer
        {
            if (!(server is SvgDeferredPaintServer))
                return server as T;

            var deferred = (SvgDeferredPaintServer)server;
            deferred.EnsureServer(parent);
            return (deferred._concreteServer ?? deferred._fallbackServer) as T;
        }
    }
}
