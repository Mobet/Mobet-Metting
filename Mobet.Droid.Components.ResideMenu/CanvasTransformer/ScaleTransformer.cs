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
    public class ScaleTransformer : ICanvasTransformer
    {
        public void TransformCanvas(Canvas canvas, float percentOpen)
        {
            canvas.Scale(percentOpen, 1, 0, 0);
        }
    }
}