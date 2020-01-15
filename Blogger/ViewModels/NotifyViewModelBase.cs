using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedicting { get; set; }
        public string RedirecingtUrl { get; set; }
        public int RedirectingTimeout { get; set; }
        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz...";
            Title = "Geçersiz İşlem";
            IsRedicting = true;
            RedirecingtUrl = "/Home/Index";
            RedirectingTimeout = 3000;
            Items = new List<T>();


        }
    
    }
}