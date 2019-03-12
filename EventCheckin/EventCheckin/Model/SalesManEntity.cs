using EventCheckin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCheckin.Model
{
    /// <summary>
    /// 业务员类
    /// </summary>
    public class SalesManEntity
    {
        private long _iD;
        private string _name;
        private string _imageName;
        private string _imagePath;
        private string _tableNo;
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
            }
        }
        /// <summary>
        /// 图片名
        /// </summary>
        public string ImageName
        {
            get
            {
                return _imageName;
            }

            set
            {
                _imageName = value;
                if (!string.IsNullOrEmpty(_imageName))
                    ImagePath = CommonValues.IMAGEPATH + "\\" + _imageName;
            }
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }

            set
            {
                _imagePath = value;
            }
        }
        /// <summary>
        /// 桌号
        /// </summary>
        public string TableNo
        {
            get
            {
                return _tableNo;
            }

            set
            {
                _tableNo = value;
            }
        }
    }
}
