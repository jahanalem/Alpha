using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.ViewModels.Helper
{
    public class PagingInfo
    {
        public int PageSize = 3;
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
                    return PageSize;
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
