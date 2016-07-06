using System;
using Android.Content;
using Android.Util;
using Android.Graphics;

namespace Mobet.Droid.Components.AmazingRefresharp.Views
{
    public class RotatingPullDownIcon : RotatableImageView, IAmazingRefreshsharpPullDownIcon
    {

        public RotatingPullDownIcon(Context context) : this(context, null, 0)
        {
        }

        public RotatingPullDownIcon(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public RotatingPullDownIcon(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }



        public void SetProgress(float progress)
        {
            if (RotationPivotPoint == null) {
                RotationPivotPoint = new Point(MeasuredWidth / 2, MeasuredHeight / 2);
            }
            if (progress < 1)
            {
                RotationDegress = 0;
            }
            if (progress == 1)
            {
                RotationDegress = progress * 180.0f;
            }
        }

    }
}

