using EventCheckin.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace EventCheckin.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private CustomEntity _custom = new CustomEntity();
        /// <summary>
        /// 客户
        /// </summary>
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

        private ObservableCollection<SalesManEntity> _salesManList = new ObservableCollection<SalesManEntity>();
        /// <summary>
        /// 业务员列表
        /// </summary>
        public ObservableCollection<SalesManEntity> SalesManList
        {
            get
            {
                return _salesManList;
            }

            set
            {
                _salesManList = value;
                RaisePropertyChanged(() => this.SalesManList);
            }
        }
    }
}
