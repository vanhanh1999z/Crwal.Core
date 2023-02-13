using NLog;

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
