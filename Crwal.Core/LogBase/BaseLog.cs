using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crwal.Core.Log
{
    class BaseClass
    {
        protected BaseClass()
        {
            Log = LogManager.GetLogger(GetType().ToString());
        }
        protected Logger Log { get; private set; }
    }
}
