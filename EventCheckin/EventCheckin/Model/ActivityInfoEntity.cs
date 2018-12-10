using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCheckin.Model
{
   public class ActivityInfoEntity
    {
        private long _iD;
        private string _componyName;
        private string _eventName;
        private bool _enabled;

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

        public string ComponyName
        {
            get
            {
                return _componyName;
            }

            set
            {
                _componyName = value;
            }
        }

        public string EventName
        {
            get
            {
                return _eventName;
            }

            set
            {
                _eventName = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
            }
        }
    }
}
