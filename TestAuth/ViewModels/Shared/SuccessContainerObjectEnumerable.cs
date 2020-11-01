using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Shared
{
    public class SuccessContainerObjectEnumerable<T>
    {
        public SuccessContainerObjectEnumerable(string Error)
        {
            Outcome = new SuccessContainer(Error);
        }

        public SuccessContainerObjectEnumerable(IEnumerable<T> Items)
        {
            this.Items = Items;
        }

        public SuccessContainer Outcome { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
