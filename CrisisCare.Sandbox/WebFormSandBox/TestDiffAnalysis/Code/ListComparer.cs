using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NubyTouch.Utils;

namespace TestDiffAnalysis
{
    public class ListComparer<T>
    {
        #region Methods

        #region Compare

        /// <summary>
        /// All the optional parameter are required if <see cref="T"/> does not implement <see cref="IComparable"/>.
        /// </summary>
        /// <param name="referenceCol">Reference collection</param>
        /// <param name="newCol">New collection</param>
        /// <param name="keySortGetter"></param>
        /// <returns></returns>
        public IEnumerable<ComparisonResult<T>> Compare(IEnumerable<T> referenceCol, IEnumerable<T> newCol,
                                                        IEqualityComparer<T> identityComparer = null, 
                                                        IEqualityComparer<T> contentComparer = null,
                                                        Func<T, Object> keySortGetter = null)
        {
            if (identityComparer == null) identityComparer = EqualityComparer<T>.Default;
            if (contentComparer == null) contentComparer = EqualityComparer<T>.Default;

            var removedItems = referenceCol.Except(newCol, identityComparer);
            var addedItems = newCol.Except(referenceCol, identityComparer);

            var remainingItemsInRef = referenceCol.Intersect(newCol, identityComparer);

            var changedItems = remainingItemsInRef.Except(newCol, contentComparer);

            var UnChangedItems = remainingItemsInRef.Except(changedItems);

            var r = new List<ComparisonResult<T>>();


            r.AddRange(removedItems.Select(itemRef => new ComparisonResult<T>(ComparisonStatus.Removed, itemRef)));
            r.AddRange(addedItems.Select(itemNew => new ComparisonResult<T>(ComparisonStatus.Added, itemNew)));
            r.AddRange(changedItems.Select(itemNew => new ComparisonResult<T>(ComparisonStatus.Modified, itemNew)));
            r.AddRange(UnChangedItems.Select(itemNew => new ComparisonResult<T>(ComparisonStatus.Unchanged, itemNew)));

            //            var sorter = new Sorter(comparer);
            var r2 =  r.OrderBy(c => (keySortGetter == null) ? c.Item : keySortGetter(c.Item));
            return r2;
        }

        //public IEnumerable<ComparisonResult<T>> Compare_old(IEnumerable<T> referenceCol, IEnumerable<T> newCol,
        //                                                         IEqualityComparer<T> equalityComparer = null, IComparer<T> comparer = null)
        //{
        //    if (equalityComparer == null) equalityComparer = EqualityComparer<T>.Default;
        //    if (comparer == null) comparer = Comparer<T>.Default;

        //    var removedItems = referenceCol.Except(newCol, equalityComparer);
        //    var executedList = removedItems.ToList();
        //    var addedItems = newCol.Except(referenceCol, equalityComparer);


        //    var remainingItemsInRef = referenceCol.Intersect(newCol, equalityComparer);
        //    var remainingItemsInNew = newCol.Except(addedItems);

        //    //var remainingItemsInRef = referenceCol.Except(removedItems);
        //    //var remainingItemsInNew = newCol.Except(addedItems);

        //    var r = new List<ComparisonResult<T>>();

        //    var enumNew = remainingItemsInNew.OrderBy(item => item, comparer).GetEnumerator();

        //    foreach (var itemRef in remainingItemsInRef.OrderBy(item => item, comparer))
        //    {
        //        enumNew.MoveNext();
        //        var itemNew = enumNew.Current;
        //        ComparisonResult<T> newComparison;
        //        if (comparer.Compare(itemRef, itemNew) == 0)
        //            newComparison = new ComparisonResult<T>(ComparisonStatus.Identical, itemRef, itemNew);
        //        else
        //            newComparison = new ComparisonResult<T>(ComparisonStatus.Identical, itemRef, itemNew);
        //        r.Add(newComparison);
        //    }

        //    r.AddRange(removedItems.Select(itemRef => new ComparisonResult<T>(ComparisonStatus.Removed, itemRef, default(T))));
        //    r.AddRange(addedItems.Select(itemNew => new ComparisonResult<T>(ComparisonStatus.Added, default(T), itemNew)));

        //    var sorter = new Sorter(comparer);
        //    return r.OrderBy(c => c, sorter);
        //}

        private class Sorter : IComparer<ComparisonResult<T>>
        {
            public Sorter(IComparer<T> comparer) => Comparer = comparer;

            public int Compare(ComparisonResult<T> c1, ComparisonResult<T> c2)
            {
                return Comparer.Compare(c1.Item, c2.Item);
            }

            IComparer<T> Comparer { get; }
        }

        #endregion

        #region Debug


        #endregion

        #endregion
    }
}
