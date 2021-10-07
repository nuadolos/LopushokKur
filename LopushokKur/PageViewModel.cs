using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LopushokKur
{
    class PageViewModel
    {
        public static int PageNumber { get; set; }
        public static int TotalPage { get; set; }

        public  PageViewModel(int count, int pageNumber, int countItems)
        {
            PageNumber = pageNumber;
            TotalPage = count / countItems;
        }

        public static bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public static bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPage);
            }
        }
    }
}
