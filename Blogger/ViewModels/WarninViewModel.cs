using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.ViewModels
{
    public class WarninViewModel  : NotifyViewModelBase<string>
    {
        public WarninViewModel()
        {
            Title = "Uyarı!";
        }
    }
}