using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Graphics;

namespace Mobet.Droid.Components.AmazingRefresharp.Views
{
    public class RotatableImageView : ImageView
    {
        private float rotation_degrees;


        public RotatableImageView(Context context) : this(context, null, 0)
        {
        }

        public RotatableImageView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public RotatableImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }



        protected override void OnDraw(Canvas canvas)
        {
            canvas.Save();
            if (RotationPivotPoint != null) {
                canvas.Rotate(RotationDegress, RotationPivotPoint.X, RotationPivotPoint.Y);
            } else {
                canvas.Rotate(RotationDegress);
            }

            base.OnDraw(canvas);
        }



        public float RotationDegress {
            get { return rotation_degrees; }
            set {
                if (rotation_degrees != value) {
                    rotation_degrees = value;
                    Invalidate();
                }
            }
        }

        public Point RotationPivotPoint;

    }
}

