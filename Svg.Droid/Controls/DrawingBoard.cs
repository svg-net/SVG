using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using String = System.String;

namespace Svg.Controls
{
    public class DrawingBoard : ImageView
    {
        public DrawingBoard(Context context, IAttributeSet attr) : base(context, attr)
        {
        }

        //#region ctors

        //protected DrawingBoard(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        //{
        //    Ctor();
        //}

        //public DrawingBoard(Context context, IAttributeSet attrs) : base(context, attrs)
        //{
        //    Ctor();
        //}

        //public DrawingBoard(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        //{
        //    Ctor();
        //}

        //public DrawingBoard(Context context)
        //    : base(context)
        //{
        //    Ctor();
        //}

        //private void Ctor()
        //{
        //    //var wm = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
        //    //var display = wm.DefaultDisplay;
        //    //var size = new Point();
        //    //display.GetSize(size);

        //    //_firstDraw = true;

        //    //_screenWidth = size.X;
        //    //_screenHeight = size.Y;

        //    //TypedArray styledAttributes = Context.Theme.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.ActionBarSize });
        //    //var actionbarHeight = (int)styledAttributes.GetDimension(0, 0);
        //    //styledAttributes.Recycle();

        //    //var statusBarHeight = 0;

        //    //int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
        //    //if (resourceId > 0)
        //    //{
        //    //    statusBarHeight = Resources.GetDimensionPixelSize(resourceId);
        //    //}

        //    //_screenHeight -= actionbarHeight + statusBarHeight;
        //}
        //#endregion
    }
}