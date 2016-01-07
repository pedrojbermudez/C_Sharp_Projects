using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Utils
{
    class Constants
    {
        public static string DECIMAL_NUMBER_REGEX = "^-?[0-9]+\\.?[0-9]{0,2}$";
        public static char BACK_KEY = 'b';
        public static char PAG_DOWN = 'v';
        public static char PAG_UP = 'c';
        public static int ELEMENTS_PER_PAGE = 10;
        public enum Tables { movement, money_source, category};
    }
}
