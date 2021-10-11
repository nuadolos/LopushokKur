using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LopushokKur
{
    class PageViewModel
    {
        public int NumberPage { get; set; }
        public int StartIndex { get; set; }
        public int CountRangeItems { get { return 20; } }
        public int CountItems { get; set; }

        public PageViewModel(int numPage, int startIndex)
        {
            NumberPage = numPage;
            StartIndex = startIndex;
        }

        public int GetTotalPage()
        {
            return (int)Math.Ceiling((decimal)CountItems / CountRangeItems);
        }

        public bool LessCountRangeItems
        {
            get
            {
                return (CountItems < CountRangeItems);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (NumberPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (NumberPage <= GetTotalPage());
            }
        }

        public void GetIndex()
        {
            StartIndex = 0;

            if (HasPreviousPage && HasNextPage)
            {
                for (int i = 1; i < NumberPage; i++)
                {
                    StartIndex += CountRangeItems;
                }
            }
        }
    }
}
