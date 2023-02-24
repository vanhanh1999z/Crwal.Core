using System;
using System.IO;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Crwal.Core.Attribute
{
    [PSerializable]
    public class TracingAttribute : OnExceptionAspect
    {
        private readonly string _pathLog = @":D\TracingLog.txt" + DateTime.Now.ToString("dd/M/yyyy");

        public override void OnException(MethodExecutionArgs args)
        {
            if (!File.Exists(_pathLog)) File.Create(_pathLog);
            File.AppendText(args.Exception.Message);
        }
    }
}