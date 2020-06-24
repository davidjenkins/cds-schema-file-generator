using System;
using System.Collections.Generic;
using System.Linq;

namespace CdsSchemaFileGenerator.Extensions
{
    static class ListExtensions
    {
        static void Set<T>(this List<T> list, T item, Func<T, string> nameSelector)
        {
            if (list.Contains(item))
                return;
            var matches = list.FindAll(x => nameSelector(x) == nameSelector(item));
            if (matches.Count == 0)
            {
                // Append.
                list.Add(item);
            }
            else
            {
                var index = list.IndexOf(matches[0]);
                // Replace
                list.Insert(index, item);
                foreach (var match in matches)
                {
                    list.Remove(match);
                }
            }
        }

        internal static void Set(this List<Models.Entity> list, Models.Entity entity)
        {
            Set(list, entity, e => e.Name);
        }

        internal static void Set(this List<Models.Field> list, Models.Field field)
        {
            Set(list, field, f => f.Name);
        }

        internal static void Set(this List<Models.Relationship> list, Models.Relationship relationship)
        {
            Set(list, relationship, r => r.Name);
        }

        static T Get<T>(this List<T> list, string name, Func<T, string> nameSelector)
        {
            var matches = list.FindAll(x => name == nameSelector(x));
            return matches.SingleOrDefault();
        }

        internal static Models.Entity Get(this List<Models.Entity> list, string name)
        {
            return Get(list, name, e => e.Name);
        }

        internal static Models.Field Get(this List<Models.Field> list, string name)
        {
            return Get(list, name, f => f.Name);
        }

        internal static Models.Relationship Get(this List<Models.Relationship> list, string name)
        {
            return Get(list, name, r => r.Name);
        }
    }
}
