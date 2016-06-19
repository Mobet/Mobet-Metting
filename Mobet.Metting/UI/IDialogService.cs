using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobet.Metting.UI
{
    public interface IDialogService
    {
        void Alert(string message, string title, string OkButtonText);
    }
}
