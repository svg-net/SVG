namespace ExCSS
{
    static class Extensions
    {
        public static string ToString(this IToString source, bool friendlyFormat)
        {
            return source.ToString(friendlyFormat, 0);
        }
    }
}
