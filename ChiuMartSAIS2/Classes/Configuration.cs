using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiuMartSAIS2.Classes
{
    class Configuration
    {
        //configuration variables
        // private string _cnString = "Server=localhost;Database=poschi;Uid=root;Pwd=root123;Convert Zero Datetime=true;";
        //private string _cnString = "Server=localhost;Database=raza_store;Uid=root;Pwd=;Convert Zero Datetime=true;";
        private string _cnString = "Server=localhost;Database=raza_store;Uid=root;Pwd=;Convert Zero Datetime=true;";

        //setters and getters
        public string connectionstring
        {
            get { return _cnString; }
            set { _cnString = value; }
        }

        //string errorCode = string.Format("Error Code : {0}", ex.Number);
        //MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
