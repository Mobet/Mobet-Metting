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
using Android.Views.Animations;
using Android.Graphics;

namespace Mobet.Droid.Components.ResideMenu.CanvasTransformer
{
    public class SlideTransformer : ICanvasTransformer
    {
        private static readonly SlideInterpolator Interpolator = new SlideInterpolator();
        public class SlideInterpolator : Java.Lang.Object, IInterpolator
        {
            public float GetInterpolation(float t)
            {
                t -= 1.0f;
                return t * t * t + 1.0f;
            }
        }

        public void TransformCanvas(Canvas canvas, float percentOpen)
        {
            canvas.Translate(0, canvas.Height * (1 - Interpolator.GetInterpolation(percentOpen)));
        }
    }
}