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
using System.Threading.Tasks;

namespace Mobet.Metting.Droid.ReceiverHandlers {
    public abstract class BaseHandler {

        public abstract string Action {
            get;
        }

        public virtual void Handle(Bundle bundle) {
        }
    }
}