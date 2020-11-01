using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Shared
{
    public class SuccessContainer
    {
        public SuccessContainer()
        {

        }

        public SuccessContainer(string Error)
        {
            this.Error = Error;
        }

        public string Error { get; set; }
        public bool IsSuccess => String.IsNullOrEmpty(Error);

    }
}
