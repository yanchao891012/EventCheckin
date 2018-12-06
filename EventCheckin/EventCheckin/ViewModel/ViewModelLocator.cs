using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCheckin.ViewModel
{
   public class ViewModelLocator
    {
        private static readonly object SynObject = new object();

        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance
        {
            get
            {
                if (_instance==null)
                {
                    lock(SynObject)
                    {
                        _instance = new ViewModelLocator();
                    }
                }
                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        private MainVM _main;
        public MainVM Main
        {
            get
            {
                return _main ?? (_main = new MainVM());
            }
        }
    }
}
