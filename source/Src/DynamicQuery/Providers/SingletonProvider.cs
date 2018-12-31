using System;

namespace DotFramework.DynamicQuery
{
    public class SingletonProvider<T> : IDisposable where T : class
    {
        private static T _Instance;
        private static readonly object padlock = new object();

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (padlock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = CreateInstance();
                        }
                    }
                }

                Type t = typeof(string);

                return _Instance;
            }
        }

        private static T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        public virtual void Dispose()
        {

        }
    }
}
