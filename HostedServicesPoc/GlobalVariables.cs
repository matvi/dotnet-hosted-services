using System;
using System.Threading;
using Nito.Disposables;

namespace HostedServicesPoc
{
    public static class GlobalVariables
    {
        private static AsyncLocal<Guid> _TraceLogId = new();
        public static Guid TraceLogId => _TraceLogId.Value;
        public static IDisposable SetTraceLogId(Guid value)
        {
            var oldValue = _TraceLogId.Value;
            _TraceLogId.Value = value;
            return Disposable.Create(() => _TraceLogId.Value = oldValue);
        }
    }
}