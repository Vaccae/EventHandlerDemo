using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threaddemo
{
    public class testEvent :EventArgs
    {
        public string datastr;

        public int id;

        public testEvent(string str,int id)
        {
            datastr = str;
            this.id = id;
        }
    }
}
