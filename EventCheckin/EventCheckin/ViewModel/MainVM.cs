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
using System.Text.RegularExpressions;
using YC.UtilTool;

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

        private ObservableCollection<CustomEntity> _customList = new ObservableCollection<CustomEntity>();
        /// <summary>
        /// 客户列表
        /// </summary>
        public ObservableCollection<CustomEntity> CustomList
        {
            get
            {
                return _customList;
            }

            set
            {
                _customList = value;
                RaisePropertyChanged(() => this.CustomList);
            }
        }

        private CustomEntity _selectedCustom = new CustomEntity();
        /// <summary>
        /// 客户选择
        /// </summary>
        public CustomEntity SelectedCustom
        {
            get
            {
                return _selectedCustom;
            }

            set
            {
                _selectedCustom = value;
                RaisePropertyChanged(() => this.SelectedCustom);
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
                    SalesManList = new ObservableCollection<SalesManEntity>(DBHelper.SelectSalesMan());
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
                        win.Owner = NativeMethods.GetActiveWindowEx();
                        win.ShowDialog();
                    }));
            }
        }

        private RelayCommand _customEditUcCommand;
        /// <summary>
        /// 跳转客户维护页面
        /// </summary>
        public RelayCommand CustomEditUcCommand
        {
            get
            {
                return _customEditUcCommand ?? (_customEditUcCommand = new RelayCommand(() =>
                    {
                        CustomEditWin win = new CustomEditWin();
                        win.Owner = NativeMethods.GetActiveWindowEx();
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
                        if (CustomMessageBox.ShowQuestionMessage(mess) == System.Windows.MessageBoxResult.Yes)
                        {
                            DBHelper.DeleteSalesMans(SelectedSalesManDataGrid.ID);
                            SalesManList = new ObservableCollection<SalesManEntity>(DBHelper.SelectSalesMan());
                        }
                    }));
            }
        }

        private RelayCommand _addCustomCommand;
        /// <summary>
        /// 新增客户
        /// </summary>
        public RelayCommand AddCustomCommand
        {
            get
            {
                return _addCustomCommand ?? (_addCustomCommand = new RelayCommand(() =>
                    {

                        if (string.IsNullOrEmpty(Custom.Name) || string.IsNullOrEmpty(Custom.PhoneNum) || !Regex.IsMatch(Custom.PhoneNum, RegexHelper.PhoneNum_Regex) || string.IsNullOrEmpty(SelectedSalesManListBox.ImageName))
                        {
                            CustomMessageBox.ShowInfoMessage("请填入正确数据，并且选择业务员！");
                            return;
                        }

                        List<CustomEntity> temp = DBHelper.SelectCustom("PhoneNum=" + Custom.PhoneNum);
                        if (temp.Count > 0)
                        {
                            CustomMessageBox.ShowInfoMessage("此手机号已存在，请更换其他手机号进行添加！");
                            return;
                        }
                        DBHelper.InsertCustom(Custom.Name, Custom.PhoneNum, SelectedSalesManListBox.ID);
                        CustomMessageBox.ShowInfoMessage("添加成功！");
                        Custom.Name = null;
                        Custom.PhoneNum = null;
                        SelectedSalesManListBox = null;
                    })
                );
            }
        }

        private RelayCommand _loadCustomCommand;
        /// <summary>
        /// 客户维护页面加载
        /// </summary>
        public RelayCommand LoadCustomCommand
        {
            get
            {
                return _loadCustomCommand ?? (_loadCustomCommand = new RelayCommand(() =>
                {
                    LoadCustom();
                }));
            }
        }

        private void LoadCustom()
        {
            CustomList = new ObservableCollection<CustomEntity>(DBHelper.SelectCustom());
            foreach (var item in CustomList)
            {
                item.SalesManName = SalesManList.FirstOrDefault(p => p.ID == item.SalesManID) == null ? "" : SalesManList.FirstOrDefault(p => p.ID == item.SalesManID).Name;
            }
        }

        private RelayCommand _exportCustomCommand;
        /// <summary>
        /// 导出客户
        /// </summary>
        public RelayCommand ExportCustomCommand
        {
            get
            {
                return _exportCustomCommand ?? (_exportCustomCommand = new RelayCommand(() =>
                    {

                    }));
            }
        }

        private RelayCommand _clearCustomCommand;
        /// <summary>
        /// 清空客户
        /// </summary>
        public RelayCommand ClearCustomCommand
        {
            get
            {
                return _clearCustomCommand ?? (_clearCustomCommand = new RelayCommand(() =>
                    {
                        if (CustomMessageBox.ShowQuestionMessage("确定要情况客户列表？")== System.Windows.MessageBoxResult.Yes)
                        {
                            DBHelper.DeleteCustom();
                        }
                        LoadCustom();
                    }));
            }
        }

        private RelayCommand _deleteCustomCommand;
        /// <summary>
        /// 删除客户
        /// </summary>
        public RelayCommand DeleteCustomCommand
        {
            get
            {
                return _deleteCustomCommand ?? (_deleteCustomCommand = new RelayCommand(() =>
                    {
                        if (CustomMessageBox.ShowQuestionMessage("确定要删除选中用户？")== System.Windows.MessageBoxResult.Yes)
                        {
                            DBHelper.DeleteCustom("ID=" + SelectedCustom.ID);
                        }
                        LoadCustom();
                    }));
            }
        }
        #endregion
    }
}
