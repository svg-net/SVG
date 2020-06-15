using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Svg
{
    [NonSvgElementAttribute("link")]
    public class SvgLink : NonSvgElement
    {
        public SvgLink()
          : base("link")
        {

        }
        public enum RelativeValue
        {
            Unknown,
            Alternate,
            Author,
            Help,
            Icon,
            License,
            Next,
            Pingback,
            Preconnect,
            Prefetch,
            Preload,
            Prerender,
            Prev,
            Search,
            Stylesheet
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLink>();
        }


        [SvgAttribute("href")]
        public string Href
        {
            get { return GetAttribute<string>("href", false, string.Empty); }
            set { Attributes["href"] = value; }
        }

        [SvgAttribute("rel")]
        public RelativeValue Rel
        {
            get { return GetAttribute<RelativeValue>("rel", false, RelativeValue.Unknown); }
            set { Attributes["rel"] = value; }
        }



        public string GetLinkContentAsText(RelativeValue rel)
        {
            var stream = GetLinkContentAsStream(rel);
            if (stream == null)
                return null;

            string content = null;
            using (StreamReader sr = new StreamReader(stream))
            {
                content = sr.ReadToEnd();
            }
            stream.Dispose();
            return content;
        }


        public Stream GetLinkContentAsStream(RelativeValue rel = RelativeValue.Unknown)
        {
            if (Rel != rel && rel != RelativeValue.Unknown)
                return null;
            // Uri MaxLength is 65519 (https://msdn.microsoft.com/en-us/library/z6c2z492.aspx)
            // if using data URI scheme, very long URI may happen.
            var safeUriString = Href.Length > 65519 ? Href.Substring(0, 65519) : Href;

            try
            {
                var uri = new Uri(safeUriString, UriKind.RelativeOrAbsolute);

                if (!uri.IsAbsoluteUri)
                    uri = new Uri(OwnerDocument.BaseUri, uri);

                // should work with http: and file: protocol urls
                var httpRequest = WebRequest.Create(uri);

                var webResponse = httpRequest.GetResponse();
                var stream = webResponse.GetResponseStream();
                if (stream.CanSeek)
                    stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error loading Link content: '{0}', error: {1} ", Href, ex.Message);
                return null;
            }

        }
    }
}
