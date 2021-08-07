using System.Runtime.Serialization;

namespace PWC.Infrastructure
{
    [DataContract]
    public class PagingCriteria
    {
        #region Properties

        [DataMember]
        public int PageNo { set; get; }
        [DataMember]
        public int PageSize { set; get; }
        [DataMember]
        public string SortingColumn { set; get; }
        public bool IsAscending { set; get; }
        private string _OrderSelector;
        private static readonly int _defaultPageSize = 10;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an object with default settings: PageSize=10, SortingColumn=null, IsAscending=true (No Sorting)
        /// </summary>
        /// <param name="pageNo">Start index for paging</param>
        public PagingCriteria(int pageNo)
        {
            PageNo = pageNo;
            PageSize = 10;
            SortingColumn = null;
            IsAscending = true;
        }

        /// <summary>
        /// Initializes an object with default sorting settings (No Sorting)
        /// </summary>
        /// <param name="pageNo">Start index for paging</param>
        /// <param name="pageSize">Page size for paging</param>
        public PagingCriteria(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            SortingColumn = null;
            IsAscending = true;
        }

        /// <summary>
        /// Initializes an object with sorting settings
        /// </summary>
        /// <param name="pageNo">Start index for paging</param>
        /// <param name="pageSize">Page size for paging</param>
        /// <param name="sortingCriteria">A string which contains column name and sorting type, e.g:"columnName ASC" or "columnName DESC"</param>
        public PagingCriteria(int pageNo, int pageSize, string sortingCriteria)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            if (!string.IsNullOrEmpty(sortingCriteria))
            {
                SortingColumn = (sortingCriteria.ToLower().EndsWith("asc")) ? sortingCriteria.Remove(sortingCriteria.Length - 4) : sortingCriteria.Remove(sortingCriteria.Length - 5);
                IsAscending = (sortingCriteria.ToLower().EndsWith("asc"));
            }
            else
            {
                SortingColumn = null;
                IsAscending = true;
            }
        }

        /// <summary>
        /// Initializes an object with sorting settings
        /// </summary>
        /// <param name="pageNo">Start index for paging</param>
        /// <param name="pageSize">Page size for paging</param>
        /// <param name="sortingCriteria">A string which contains column name and sorting type, e.g:"columnName ASC" or "columnName DESC"</param>
        public PagingCriteria(int pageNo, string sortingCriteria)
        {
            PageNo = pageNo * _defaultPageSize;
            PageSize = _defaultPageSize;
            if (!string.IsNullOrEmpty(sortingCriteria))
            {
                SortingColumn = (sortingCriteria.ToLower().EndsWith("asc")) ? sortingCriteria.Remove(sortingCriteria.Length - 4) : sortingCriteria.Remove(sortingCriteria.Length - 5);
                IsAscending = (sortingCriteria.ToLower().EndsWith("asc"));
            }
            else
            {
                SortingColumn = null;
                IsAscending = true;
            }
        }

        /// <summary>
        /// Initializes an object with sorting settings
        /// </summary>
        /// <param name="pageNo">Start index for paging</param>
        /// <param name="pageSize">Page size for paging</param>
        /// <param name="sortingColumn">The column name to sort</param>
        /// <param name="isAscending">Sorting type, if true then sorting will be in ascending order, else sorting will be in descending order</param>
        public PagingCriteria(int pageNo, int pageSize, string sortingColumn, bool isAscending)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            if (!string.IsNullOrEmpty(sortingColumn))
            {
                SortingColumn = sortingColumn;
                IsAscending = isAscending;
            }
            else
            {
                SortingColumn = null;
                IsAscending = true;
            }
        }


        #endregion

        #region Properties

        public string OrderSelector
        {
            get
            {
                string expression = null;
                if (_OrderSelector == null)
                {
                    if (!string.IsNullOrEmpty(this.SortingColumn))
                    {
                        expression = string.Format("{0} {1}", this.SortingColumn, !this.IsAscending ? "Desc" : "Asc");
                    }
                }
                else
                {
                    expression = _OrderSelector;
                }

                return expression;


            }
            set{
                _OrderSelector = value;
            }
        }

        public int CountToSkip
        {
            get
            {
                int value = 0;
                if(PageNo > 1)
                {
                    value = (PageNo -1) * PageSize;
                }
                return value;
            }
          
        }

        public int CountToTake
        {
            get
            {
                return PageSize;
            }

        }

        #endregion
    }
}
