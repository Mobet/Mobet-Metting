using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

using MvvmCross.Core.ViewModels;
using Mobet.Metting.Infrastructure.Services;
using Mobet.Metting.UI;
using MvvmCross.Platform;

namespace Mobet.Metting.Models
{
    public class LoginModel : MvxViewModel
    {
        private readonly IUserService userService;
        private readonly IDialogService dialogService;
        private readonly IToastService toastService;

        public LoginModel(IUserService userService, IDialogService dialogService, IToastService toastService)
        {
            this.userService = userService;

            this.dialogService = dialogService;
            this.toastService = toastService;
        }

        public string Account { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }


        public ICommand LoginCommand
        {
            get
            {
                return new MvxCommand(() => Login());
            }
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(this.Account))
            {
                toastService.Alert("请输入账号");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                toastService.Alert("请输入密码");
                return;
            }

            var model = this.userService.Login(this.Account, this.Password);

            if (model == null)
            {
                toastService.Alert("请检查您的账号或密码");
                return;
            }
            ShowViewModel<MainModel>();
        }

        public override void Start()
        {
            this.Avatar = "https://s1.mi-img.com/mfsv2/avatar/fdsc3/p01ipyFBOr98/GOF2ZyTtU5ejqs_320.jpg";
            this.Account = "392327013";
            this.Password = "mobet";
            base.Start();
        }
    }
}
