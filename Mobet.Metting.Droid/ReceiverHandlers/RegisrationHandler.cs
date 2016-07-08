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
using CN.Jpush.Android.Api;
using System.Threading.Tasks;

namespace Mobet.Metting.Droid.ReceiverHandlers {

    /// <summary>
    /// 
    /// </summary>
    public class RegisrationHandler : BaseHandler {
        public override string Action {
            get {
                return "cn.jpush.android.intent.REGISTRATION";
            }
        }

        public override void Handle(Bundle bundle) {
            //SDK �� JPush Server ע�����õ���ע�� ȫ��Ψһ�� ID ������ͨ���� ID ���Ӧ�Ŀͻ��˷�����Ϣ��֪ͨ��
            var id = bundle.GetString(JPushInterface.ExtraRegistrationId);
        }
    }
}