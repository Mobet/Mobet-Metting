using CoreGraphics;
using System;

using UIKit;

namespace App2
{
    public partial class ViewController1 : UIViewController
    {
        public event EventHandler ButtonClick;

        public ViewController1()
            : base("ViewController1", null)
        {
            this.ButtonClick += (sender, args) =>
            {
                this.View.BackgroundColor = UIColor.Red;
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            UIButton btn = new UIButton();

            btn.SetTitle("Hello World", UIControlState.Normal);
            btn.AddTarget(ButtonClick, UIControlEvent.TouchUpInside);

            btn.Frame = new CGRect(0, 0, 100, 50);

            this.View.AddSubview(btn);
        }




    }
}