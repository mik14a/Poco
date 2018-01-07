using System;
using System.Linq;
using System.Linq.Expressions;

namespace Poco
{
    static class Activator<T>
    {
        public static T CreateInstance() {
            return activate();
        }
        static readonly Func<T> activate = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
    }
}
