using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Mobet.Droid.Components.ResideMenu.CanvasTransformer
{

    public class ZoomTransformer : ICanvasTransformer
    {
        public void TransformCanvas(Canvas canvas, float percentOpen)
        {
            var scale = (float)(percentOpen * 0.25 + 0.75);
            canvas.Scale(scale, scale, canvas.Width / 2f, canvas.Height / 2f);
        }
    }
}