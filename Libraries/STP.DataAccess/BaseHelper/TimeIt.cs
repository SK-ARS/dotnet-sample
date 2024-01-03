#region

using System;
using System.Diagnostics;

#endregion

namespace STP.DataAccess.BaseHelper
{
    public class TimeIt : IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly string _testName;
        private readonly int _total;
        private readonly Type _type;

        public TimeIt(int total, string testName, Type type)
        {
            _total = total;
            _testName = testName;
            _type = type;
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Trace.WriteLine(string.Format("{0}, {1}, {2}, {3}", _testName, _type.Name, _total,
                _stopwatch.ElapsedMilliseconds));
        }
    }
}