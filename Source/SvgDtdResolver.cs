using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Svg
{
    internal class SvgDtdResolver : XmlUrlResolver
    {
        /// <summary>
        /// Defaults to `false` to prevent XXE.  Set to `true` to resolve external resources.
        /// </summary>
        /// <see ref="https://owasp.org/www-community/vulnerabilities/XML_External_Entity_(XXE)_Processing"/>
        public bool ResolveExternalResources { get; set; }

        /// <summary>
        /// Maps a URI to an object containing the actual resource.
        /// </summary>
        /// <param name="absoluteUri">The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)"/></param>
        /// <param name="role">The current implementation does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios.</param>
        /// <param name="ofObjectToReturn">The type of object to return. The current implementation only returns System.IO.Stream objects.</param>
        /// <returns>
        /// A System.IO.Stream object or null if a type other than stream is specified.
        /// </returns>
        /// <exception cref="T:System.Xml.XmlException">
        ///     <paramref name="ofObjectToReturn"/> is neither null nor a Stream type. </exception>
        /// <exception cref="T:System.UriFormatException">The specified URI is not an absolute URI. </exception>
        /// <exception cref="T:System.NullReferenceException">
        ///     <paramref name="absoluteUri"/> is null. </exception>
        /// <exception cref="T:System.Exception">There is a runtime error (for example, an interrupted server connection). </exception>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.ToString().IndexOf("svg11.dtd", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream("Svg.Resources.svg11.dtd");
            }
            else
            {
                if (ResolveExternalResources)
                {
                    return base.GetEntity(absoluteUri, role, ofObjectToReturn);
                }
                else
                {
                    return new MemoryStream();
                }
            }
        }
    }
}
