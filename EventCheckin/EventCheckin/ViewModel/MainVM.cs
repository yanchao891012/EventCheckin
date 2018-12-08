using EventCheckin.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using EventCheckin.DB;
using GalaSoft.MvvmLight.Command;
using YC.UCTool.MessageBoxs;
using EventCheckin.View;

namespace EventCheckin.ViewModel
{
    public class MainVM : ViewModelBase
    {
        #region 字段属性
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

        private SalesManEntity _selectedSalesManDataGrid = new SalesManEntity();
        /// <summary>
        /// 列表中的业务员选中项
        /// </summary>
        public SalesManEntity SelectedSalesManDataGrid
        {
            get
            {
                return _selectedSalesManDataGrid;
            }

            set
            {
                _selectedSalesManDataGrid = value;
                RaisePropertyChanged(() => this.SelectedSalesManDataGrid);
            }
        }

        private SalesManEntity _selectedSalesManListBox = new SalesManEntity();
        /// <summary>
        /// Listbox中的业务员选中项
        /// </summary>
        public SalesManEntity SelectedSalesManListBox
        {
            get
            {
                return _selectedSalesManListBox;
            }

            set
            {
                _selectedSalesManListBox = value;
                RaisePropertyChanged(() => this.SelectedSalesManListBox);
            }
        }
        #endregion

        #region 事件
        private RelayCommand _loadCommand;
        /// <summary>
        /// 加载事件
        /// </summary>
        public RelayCommand LoadCommand
        {
            get
            {
                return _loadCommand ?? (_loadCommand = new RelayCommand(() =>
                {
                    SalesManList= new ObservableCollection<SalesManEntity>(DBHelper.SelectSalesMan());
                    if (SalesManList.Count <= 0)
                    {
                        CustomMessageBox.ShowInfoMessage("现在还没有业务员信息，即将跳转业务员维护页面！");
                        SalesManEditWin win = new SalesManEditWin();
                        win.ShowDialog();
                    }
                }));
            }
        }

        private RelayCommand _salesManEditUcCommand;
        /// <summary>
        /// 跳转业务员维护页面
        /// </summary>
        public RelayCommand SalesManEditUcCommand
        {
            get
            {
                return _salesManEditUcCommand ?? (_salesManEditUcCommand = new RelayCommand(() =>
                    {
                        SalesManEditWin win = new SalesManEditWin();
                        win.ShowDialog();
                    }));
            }
        }

        private RelayCommand _addSalesManCommand;
        /// <summary>
        /// 新增业务员
        /// </summary>
        public RelayCommand AddSalesManCommand
        {
            get
            {
                return _addSalesManCommand ?? (_addSalesManCommand = new RelayCommand(() =>
                    {
                        SalesManAddWin salesManAddWin = new SalesManAddWin();
                        salesManAddWin.Closed += (s, e) =>
                        {
                            SalesManList = new ObservableCollection<SalesManEntity>(DBHelper.SelectSalesMan());
                        };
                        salesManAddWin.ShowDialog();
                    }));
            }
        }

        private RelayCommand _deleteSalesManCommand;
        /// <summary>
        /// 删除业务员
        /// </summary>
        public RelayCommand DeleteSalesManCommand
        {
            get
            {
                return _deleteSalesManCommand ?? (_deleteSalesManCommand = new RelayCommand(() =>
                    {
                        string mess = "是否删除“" + SelectedSalesManDataGrid.Name + "”用户？\r\n删除的同时会清空与之相关的客户关联关系，请小心操作";
                        if (CustomMessageBox.ShowQuestionMessage(mess)== System.Windows.MessageBoxResult.Yes)
                        {
                            DBHelper.DeleteSalesMans(SelectedSalesManDataGrid.ID);
                            SalesManList = new ObservableCollection<SalesManEntity>(DBHelper.SelectSalesMan());
                        }
                    }));
            }
        }
        #endregion
    }
}
