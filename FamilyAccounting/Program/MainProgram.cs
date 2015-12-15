using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FamilyAccounting.Program
{
    class MainProgram
    {
        public static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();
            int mainOption;
            while (true)
            {
                mainOption = mainMenu.ShowMainMenu();
                if (mainOption == 1)
                {
                    SourceMenu sourceMenu = new SourceMenu();
                    while (true)
                    {
                        int sourceOption = mainMenu.SourceMenu();
                        if (sourceOption == 1)
                        {
                            sourceMenu.NewSource();
                        }
                        else if (sourceOption == 2)
                        {
                            sourceMenu.EditSource();
                        }
                        else if (sourceOption == 3)
                        {

                        }
                        else if (sourceOption == 4)
                        {

                        }
                        else if (sourceOption == 5)
                        {

                        }
                        else if (sourceOption == 6)
                        {
                            return;
                        }
                    }
                }
                else if (mainOption == 2)
                {
                    MovementMenu movementMenu = new MovementMenu();
                }
                else if (mainOption == 3)
                {
                    SourceMenu sourceMenu = new SourceMenu();
                }
                else if (mainOption == 4)
                {
                    return;
                }
            }
        }
    }
}
