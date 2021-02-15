using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnet_utils.Enums
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration => typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        public override bool Equals(object obj)
        {
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = (typeMatches && Id.Equals(((Enumeration)obj).Id));

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
