using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyAccounting.Utils;

namespace FamilyAccounting.Program
{
    class Pagination
    {
        private int totalPages;

        public int TotalPages(int totalElements)
        {
            totalPages = totalElements <= Constants.ELEMENTS_PER_PAGE ? 1 : (totalElements / Constants.ELEMENTS_PER_PAGE) + 1;
            return totalPages;
        }

        public int GetTotalPages()
        {
            return this.totalPages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>bool[0] if can be possible go to the previous page.
        /// bool[1] if can be possible go to the next page.</returns>
        public bool[] ShowPagination(int currentPage)
        {
            bool[] prevNext = new bool[2];
            if (this.totalPages == 1)
            {
                Console.WriteLine("1");
                return prevNext;
            }
            else
            {
                if (currentPage == 1)
                {
                    Console.WriteLine("Press " + Constants.PAG_DOWN + " if you wish to go to page 2.");
                    prevNext[0] = false;
                    prevNext[1] = true;
                }
                if (currentPage > 1 && currentPage < this.totalPages)
                {
                    Console.WriteLine("Press " + Constants.PAG_DOWN + " if you wish to go to page " + (currentPage + 1) + ".");
                    Console.WriteLine("Press " + Constants.PAG_UP + " if you wish to go to page " + (currentPage - 1) + ".");
                    prevNext[0] = true;
                    prevNext[1] = true;
                }
                if (currentPage == this.totalPages)
                {
                    Console.WriteLine("Press " + Constants.PAG_UP + " if you wish to go to page " + (this.totalPages - 1) + ".");
                    prevNext[0] = true;
                    prevNext[1] = false;
                }
            }
            return prevNext;
        }
    }
}
