using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threaddemo
{
    public class CItem
    {
        

        public int threadid;

        public int currentcount;

        public string name;

        public DateTime lastdatetime;

        public CItem(int threadid, int currentcount, string name, DateTime lastdatetime)
        {
            this.threadid = threadid;
            this.currentcount = currentcount;
            this.name = name;
            this.lastdatetime = lastdatetime;
        }
    }
}
