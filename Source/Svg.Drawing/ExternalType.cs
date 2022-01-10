using System;

namespace Svg
{
    [Flags]
    public enum ExternalType
    {
        None = 0x0,
        Local = 0x1,
        Remote = 0x2,
    }

    public static class ExternalTypeExtensions
    {
        public static bool AllowsResolving(this ExternalType externalType, Uri uri)
        {
            return uri.IsAbsoluteUri &&
                (externalType.HasFlag(ExternalType.Local) && uri.IsFile ||
                externalType.HasFlag(ExternalType.Remote) && !uri.IsFile);
        }
    }
}
