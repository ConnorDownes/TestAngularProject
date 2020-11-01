using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.ConfigurationOptions
{
    public class AppSettingsOptions
    {
        public const string AppSettings = "AppSettings";

        public string Secret { get; set; }
        public int TokenDuration { get; set; }
    }
}
