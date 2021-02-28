using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonExtensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<TTarget> GetFromHierarchy<THierarchical, TTarget>(
            this IEnumerable<THierarchical> source,
            Func<THierarchical, IEnumerable<THierarchical>> stepDownIntoHierarchyFor,
            Func<THierarchical, IEnumerable<TTarget>> getTargetItemsFrom)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (stepDownIntoHierarchyFor is null) throw new ArgumentNullException(nameof(source));
            if (getTargetItemsFrom is null) throw new ArgumentNullException(nameof(source));

            Queue<THierarchical> queue = new Queue<THierarchical>(source);

            while (queue.Count > 0)
            {
                THierarchical item = queue.Dequeue();

                foreach (var targetItem in getTargetItemsFrom(item))
                    yield return targetItem;

                queue.EnqueueRange(stepDownIntoHierarchyFor(item));
            }
        }

        public static string GetCommonPrefix(this IEnumerable<string> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            IReadOnlyList<string> sourceAsList = source.ToList().AsReadOnly();

            if (sourceAsList.Count == 0)
                return string.Empty;

            return new string(sourceAsList[0]
                .Substring(0, sourceAsList.Min(s => s.Length))
                .TakeWhile((c, i) => sourceAsList.All(s => s[i] == c))
                .ToArray());
        }
    }
}
