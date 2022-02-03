
//данные классы позволяют создавать объекты IEqualityComparer<T> с применением лямбда выражений
//вместо определения нового класса для каждого типа сравнения, которое нужно производить.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PartyInvites.Tests
{
    public class Comparer
    {
        public static Comparer<U> Get<U>(Func<U, U, bool> func) => new Comparer<U>(func);

    }
    public class Comparer<T> : Comparer, IEqualityComparer<T>
    {
        private Func<T, T, bool> comparisonFunc;
        public Comparer(Func<T, T, bool> func)
        {
            comparisonFunc = func;
        }

        public bool Equals(T? x, T? y)
        {
            return comparisonFunc(x, y);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }
    }
}
