using System;
using System.Collections.Generic;
using Android.Content.Res;

namespace Svg
{
    public partial class SvgDocument
    {
        static partial void OpenPartial<T>(string path, Dictionary<string, string> entities, OpenResult<T> result) where T : SvgDocument, new()
        {
            try
            {
                var ctx = Android.App.Application.Context;

                using (var stream = ctx.Assets.Open(path.TrimStart('/'), Access.Streaming))
                {
                    var doc = Open<T>(stream, entities);
                    doc.BaseUri = new Uri(System.IO.Path.GetFullPath(path));
                    result.Result = doc;
                }
            }
            catch (Exception x)
            {

            }
        }
    }
}