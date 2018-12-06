using EventCheckin.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCheckin.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private CustomEntity _custom = new CustomEntity();
        public CustomEntity Custom
        {
            get
            {
                return _custom;
            }

            set
            {
                _custom = value;
                RaisePropertyChanged(() => this.Custom);
            }
        }
    }
}
