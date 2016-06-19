using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

using Mobet.Metting.Models;
using Mobet.Metting.Infrastructure.Services;

namespace Mobet.Metting
{
    public partial class App : MvxApplication
    {
        public App()
        {
            //设置App的启动界面
            Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<LoginModel>());
        }

        public override void Initialize()
        {

            //向IoC注册用户相关的服务
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            base.Initialize();
        }
    }
}
