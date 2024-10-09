using System;

namespace Alpha.Infrastructure.PaginationUtility
{
    public class PagingInfo
    {
        public static int DefaultItemsPerPage { get; set; } = 5;
        //int.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultItemsPerPage"]);
        /// <summary>
        /// All of items that exist
        /// </summary>
        public virtual int TotalItems { get; set; }

        private int _itemsPerPage = 0;
        /// <summary>
        /// 
        /// </summary>
        public virtual int ItemsPerPage
        {
            get
            {
                if (_itemsPerPage <= 0)
                    return DefaultItemsPerPage;
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
            }
        }

        /// <summary>
        /// Current page
        /// </summary>
        public virtual int CurrentPage { get; set; }

        /// <summary>
        /// returns total pages that is possible based on TotalItems and ItemsPerPage
        /// </summary>
        public virtual int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
