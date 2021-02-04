using System.Collections.Generic;

namespace TestDiffAnalysis
{
    public class ComparisonResult<T>
    {
        #region Cctor
        public ComparisonResult(ComparisonStatus status, T item)
        {
            Status = status;
            Item = item;
        }
        #endregion

        #region Properties

        public ComparisonStatus Status { get; }

        public T Item { get; }
        //public T ItemNew { get; }

        #endregion

        #region Methods

        public override string ToString()
        {
            try
            {
                var r = $"{Status}\t{Item?.ToString()??"null"}";
                return r;
            }
            catch (System.Exception)
            {
                return base.ToString();
            }
        }


        #endregion 

    }

    public enum ComparisonStatus { Unchanged, Added, Removed, Modified }
}