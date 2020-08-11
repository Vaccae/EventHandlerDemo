using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threaddemo
{
    interface Inftest
    {
        event EventHandler<testEvent> DataReceived;
        void Stop();
        void Start();
    }
}
