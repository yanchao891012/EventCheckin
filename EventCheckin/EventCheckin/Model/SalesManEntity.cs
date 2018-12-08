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
            }
        }
    }
}
