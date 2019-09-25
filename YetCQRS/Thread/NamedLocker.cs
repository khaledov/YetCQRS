using System;
using System.Collections.Concurrent;

namespace YetCQRS.Thread
{
    public class NamedLocker
    {
        readonly ConcurrentDictionary<string, object> _lockDict = new ConcurrentDictionary<string, object>();
        public object GetLock(string name)
        {
            return _lockDict.GetOrAdd(name, s => new object());
        }

        public TResult RunWithLock<TResult>(string name, Func<TResult> body)
        {
            lock (_lockDict.GetOrAdd(name, s => new object()))
            {
                return body();
            }
        }

        public void RunWithLock(string name, Action body)
        {
            lock (_lockDict.GetOrAdd(name, s => new object()))
            {
                body();
            }
        }

        public void RemoveLock(string name)
        {
            object o;
            _lockDict.TryRemove(name, out o);
        }
    }
}
