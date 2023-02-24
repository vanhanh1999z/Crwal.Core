using NLog;

namespace Crwal.Core.Log
{
    internal class BaseClass
    {
        protected BaseClass()
        {
            Log = LogManager.GetLogger(GetType().ToString());
        }

        protected Logger Log { get; }
    }
}