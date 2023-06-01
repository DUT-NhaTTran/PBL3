using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.ViewModel
{
    public class ViewModelLocator
    {
        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ViewModelLocator();

                return _instance;
            }
        }
        private static IContainerProvider _containerProvider;

        public static void RegisterContainer(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        public static T Resolve<T>()
        {
            return _containerProvider.Resolve<T>();
        }
        // Các định nghĩa khác trong ViewModelLocator

        // ...
    }
}
