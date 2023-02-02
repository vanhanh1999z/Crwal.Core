using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crwal.Core.Attribute
{
    [PSerializable]
    public class TracingAttribute : OnExceptionAspect
    {
        private readonly string _pathLog = @":D\TracingLog.txt" + DateTime.Now.ToString("dd/M/yyyy");
        public override void OnException(MethodExecutionArgs args)
        {
            if (!File.Exists(_pathLog))
            {
                File.Create(_pathLog);
            }
            File.AppendText(args.Exception.Message);
        }
    }
}
