using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Views;
using Java.IO;
using Svg;
using Svg.Droid;
using Bitmap = Svg.Bitmap;
using Exception = Java.Lang.Exception;
using Orientation = Android.Widget.Orientation;

namespace SvgW3CTestRunner.Droid
{
    [Activity(Label = "SvgW3CTestRunner.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // --------------------------------------------------------
        // SETTINGS
        // --------------------------------------------------------
        private bool HEATMAP_ENABLED = false;
        // --------------------------------------------------------

        // --------------------------------------------------------
        // MONITORING
        // --------------------------------------------------------
        int _globalCompletedCount = 0;
        int _globalFailedCount = 0;
        int _globalGfxCount = 0;

        double _globalCorrectPixelAmount = 0;
        double _globalPixelAmount = 0;

        double _pngGlobalTime = 0;
        double _svgGlobalTime = 0;
        // --------------------------------------------------------

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }

        private Tuple<float, Android.Graphics.Bitmap> ImageCompare(Android.Graphics.Bitmap i1, Android.Graphics.Bitmap i2)
        {
            float correctPixel = 0;
            float pixelAmount = i1.Height * i1.Width;
            _globalPixelAmount += pixelAmount;
            var bitmap = Android.Graphics.Bitmap.CreateBitmap(i1.Width, i1.Height, Android.Graphics.Bitmap.Config.Rgb565);
            bitmap.EraseColor(Color.Red);

            for (var y = 0; y < i1.Height; ++y)
                for (var x = 0; x < i1.Width; ++x)
                    if (i1.GetPixel(x, y) == i2.GetPixel(x, y))
                    {
                        if (Color.GetAlphaComponent(i1.GetPixel(x, y)) != 0) // if pixel has alpha
                        {
                            pixelAmount--;
                            _globalPixelAmount--;
                            bitmap.SetPixel(x, y, Color.White);
                        }
                        else
                        {
                            correctPixel++;
                            _globalCorrectPixelAmount++;
                            bitmap.SetPixel(x, y, Color.White);
                        }
                    }

            return new Tuple<float, Android.Graphics.Bitmap>((correctPixel / pixelAmount) * 100, bitmap);
        }

        protected override void OnPostResume()
        {
            base.OnPostResume();

            var mainView = FindViewById<LinearLayout>(Resource.Id.mainView);
            mainView.AddView(new View(this) // Trennlinie
            {
                LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 3),
                Background = new ColorDrawable(Color.Gray),
            });

            var globalCompare = new TextView(this);
            if (HEATMAP_ENABLED)
            {
                // --------------------------------------------------------
                // GLOBAL PERCENTAGE
                // --------------------------------------------------------
                globalCompare.SetPadding(100, DpToPx(1), DpToPx(1), 0);
                globalCompare.Background = new ColorDrawable(Color.White);
                globalCompare.LayoutParameters = new ViewGroup.LayoutParams(1000, 600); // dp?
                globalCompare.Gravity = GravityFlags.Top | GravityFlags.Left;
                globalCompare.TextSize = 16;
                globalCompare.SetTextColor(Color.Black);
                globalCompare.Typeface = Typeface.Monospace;

                globalCompare.Text = $"Completed: {_globalCompletedCount}/{_globalGfxCount}\n" +
                                        $"Failed: {_globalFailedCount}\n" +
                                        "Correctness: 100,00" + "%\n\n" +
                                        $"PNG: 0ms\n" +
                                        $"SVG: 0ms\n" +
                                        $"=> 1.00x";

                mainView.AddView(globalCompare);
                // --------------------------------------------------------

                mainView.AddView(new View(this) // Trennlinie
                {
                    LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 3),
                    Background = new ColorDrawable(Color.Gray),
                });
            }

            new System.Threading.Thread(new System.Threading.ThreadStart(() => {

                var watch = Stopwatch.StartNew();

                var assetManager = Assets;
                var svgs = assetManager.List("svg")
                    .Where(@s => s.StartsWith("painting-"))
                    .OrderBy(@s => s).ToList();

                var pngs = assetManager.List("png")
                    .Where(@s => s.StartsWith("painting-"))
                    .OrderBy(@s => s).ToList();

                _globalGfxCount = pngs.Count;

                UpdateGlobalStats(globalCompare);

                foreach (var png in pngs)
                {
                    try
                    {
                        var ll = new LinearLayout(this);
                        ll.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                        ll.Background = new ColorDrawable(Color.White);
                        ll.Orientation = Orientation.Horizontal;

                        var ivPng = new ImageView(this);
                        ivPng.SetPadding(DpToPx(1), DpToPx(1), 0, DpToPx(1));
                        ivPng.Background = new ColorDrawable(Color.White);
                        ivPng.LayoutParameters = new ViewGroup.LayoutParams(660, 495); // dp?
                        ivPng.SetScaleType(ImageView.ScaleType.FitCenter);

                        // --------------------------------------------------------
                        // PNG
                        // --------------------------------------------------------
                        watch.Start();

                        var ims = Assets.Open($"png/{png}");
                        var pngBitmap = GetResizedBitmap(BitmapFactory.DecodeStream(ims), 480, 360);
                        ivPng.SetImageBitmap(pngBitmap);

                        watch.Stop();
                        double pngTime = 0;
                        _pngGlobalTime += pngTime = watch.ElapsedMilliseconds;
                        watch.Reset();
                        // --------------------------------------------------------

                        var svg = svgs.FirstOrDefault(@s => s.StartsWith(png.Split('.')[0]));

                        var ivSvg = new ImageView(this);
                        ivSvg.SetPadding(0, DpToPx(1), 0, DpToPx(1));
                        ivSvg.Background = new ColorDrawable(Color.White);
                        ivSvg.LayoutParameters = new ViewGroup.LayoutParams(660, 495);
                        ivSvg.SetScaleType(ImageView.ScaleType.FitCenter);

                        // --------------------------------------------------------
                        // SVG
                        // --------------------------------------------------------
                        watch.Start();

                        var svgBitmap = new AndroidBitmap(480, 360);

                        var doc = SvgDocument.Open<SvgDocument>($"svg/{svg}");
                        doc.Draw(svgBitmap);
                        ivSvg.SetImageBitmap(svgBitmap.Image);

                        watch.Stop();
                        double svgTime = 0;
                        _svgGlobalTime += svgTime = watch.ElapsedMilliseconds;
                        watch.Reset();
                        // --------------------------------------------------------

                        // --------------------------------------------------------
                        // HEATMAP
                        // --------------------------------------------------------
                        var heatmap = new ImageView(this);
                        Tuple<float, Android.Graphics.Bitmap> comparedBitmaps = null;
                        if (HEATMAP_ENABLED)
                        {
                            heatmap.SetPadding(0, DpToPx(1), 0, DpToPx(1));
                            heatmap.Background = new ColorDrawable(Color.White);
                            heatmap.LayoutParameters = new ViewGroup.LayoutParams(660, 495); // dp?
                            heatmap.SetScaleType(ImageView.ScaleType.FitCenter);

                            comparedBitmaps = ImageCompare(((BitmapDrawable)ivPng.Drawable).Bitmap, ((BitmapDrawable)ivSvg.Drawable).Bitmap);
                            heatmap.SetImageBitmap(comparedBitmaps.Item2);
                        }

                        // --------------------------------------------------------

                        // --------------------------------------------------------
                        // PERCENTAGE
                        // --------------------------------------------------------
                        var compare = new TextView(this);
                        if (HEATMAP_ENABLED)
                        {
                            compare.SetPadding(0, DpToPx(1), 0, DpToPx(1));
                            compare.Background = new ColorDrawable(Color.White);
                            compare.LayoutParameters = new ViewGroup.LayoutParams(660, 495); // dp?
                            compare.Gravity = GravityFlags.Top | GravityFlags.Left;
                            compare.TextSize = 16;
                            compare.SetTextColor(Color.Black);
                            compare.Typeface = Typeface.Monospace;

                            compare.Text = comparedBitmaps.Item1.ToString("0.00") + "%\n\n" +
                                           $"PNG: {pngTime}ms\n" +
                                           $"SVG: {svgTime}ms\n" +
                                           $"=> {(svgTime / pngTime).ToString("0.00")}x";
                        }
                        // --------------------------------------------------------


                        RunOnUiThread(() =>
                        {
                            _globalCompletedCount++;

                            UpdateGlobalStats(globalCompare);

                            ll.AddView(ivPng);
                            ll.AddView(ivSvg);
                            if (HEATMAP_ENABLED)
                            {
                                ll.AddView(heatmap);
                                ll.AddView(compare);
                            }
                            mainView.AddView(ll);
                            mainView.AddView(new View(this) // Trennlinie
                            {
                                LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 3),
                                Background = new ColorDrawable(Color.Gray),
                            });
                        });
                    }
                    catch (Exception e)
                    {
                        _globalFailedCount++;
                    }
                    catch (System.Exception e)
                    {
                        _globalFailedCount++;
                    }
                }

                UpdateGlobalStats(globalCompare);

                if (!HEATMAP_ENABLED)
                    RunOnUiThread(() => { Toast.MakeText(this, $"Global SVG time: {(_svgGlobalTime / 1000).ToString("0.00")}s", ToastLength.Long).Show(); });

            })).Start();
        }

        private void UpdateGlobalStats(TextView globalCompare)
        {
            if (HEATMAP_ENABLED)
            {
                RunOnUiThread(() =>
                {
                    globalCompare.Text = $"Completed: {_globalCompletedCount}/{_globalGfxCount}\n" +
                                            $"Failed: {_globalFailedCount}\n" +
                                            "Correctness: " + ((_globalCorrectPixelAmount / _globalPixelAmount) * 100).ToString("0.00") + "%\n\n" +
                                            $"PNG: {_pngGlobalTime}ms\n" +
                                            $"SVG: {_svgGlobalTime}ms\n" +
                                            $"=> {(_svgGlobalTime / _pngGlobalTime).ToString("0.00")}x";
                });
            }
        }

        public Android.Graphics.Bitmap GetResizedBitmap(Android.Graphics.Bitmap bm, int newWidth, int newHeight)
        {
            int width = bm.Width;
            int height = bm.Height;
            float scaleWidth = ((float)newWidth) / width;
            float scaleHeight = ((float)newHeight) / height;
            // CREATE A MATRIX FOR THE MANIPULATION
            Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
            // RESIZE THE BIT MAP
            matrix.PostScale(scaleWidth, scaleHeight);

            // "RECREATE" THE NEW BITMAP
            Android.Graphics.Bitmap resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(
                bm, 0, 0, width, height, matrix, false);
            //bm.Recycle();
            return resizedBitmap;
        }

        private int DpToPx(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 14, Resources.DisplayMetrics);
        }
    }
}