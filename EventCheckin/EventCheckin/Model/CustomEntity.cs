using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YC.UtilTool;

namespace EventCheckin.Model
{
    /// <summary>
    /// 客户类
    /// </summary>
    public class CustomEntity : INotifyPropertyChanged, IDataErrorInfo
    {
        private long _iD;
        private string _name;
        private string _phoneNum;
        private int _salesManID;

        public string this[string columnName]
        {
            get
            {
                return this.ValidateProperty<PersonMetadata>(columnName);
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public long ID
        {
            get
            {
                return _iD;
            }

            set
            {
                _iD = value;
            }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged(nameof(this.Name));
            }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum
        {
            get
            {
                return _phoneNum;
            }

            set
            {
                _phoneNum = value;
                RaisePropertyChanged(nameof(this.PhoneNum));
            }
        }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public int SalesManID
        {
            get
            {
                return _salesManID;
            }

            set
            {
                _salesManID = value;
            }
        }

        public string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        internal virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        class PersonMetadata
        {
            public int ID { get; set; }
            [CustomValidation(typeof(ValidationHelper), "CheckIsEmpty")]
            public string Name { get; set; }
            [CustomValidation(typeof(ValidationHelper), "CheckPhoneNum")]
            public string PhoneNum { get; set; }
            public string SalesManID { get; set; }
        }
    }
}
