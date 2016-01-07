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
                            sourceMenu.DeleteSource();
                        }
                        else if (sourceOption == 4)
                        {
                            sourceMenu.ViewSources();
                        }
                        else if (sourceOption == 5)
                        {
                            break;
                        }
                        else if (sourceOption == 6)
                        {
                            return;
                        }
                    }
                }
                else if (mainOption == 2)
                {
                    MovementMenu movMenu = new MovementMenu();
                    while (true)
                    {
                        int movOption = mainMenu.MovementMenu();
                        if (movOption == 1)
                        {
                            movMenu.NewMovement();
                        }
                        else if (movOption == 2)
                        {
                            movMenu.EditMovement();
                        }
                        else if (movOption == 3)
                        {
                            movMenu.DeleteMovement();
                        }
                        else if (movOption == 4)
                        {
                            movMenu.ViewMovements();
                        }
                        else if (movOption == 5)
                        {
                            break;
                        }
                        else if (movOption == 6)
                        {
                            return;
                        }
                    }
                }
                else if (mainOption == 3)
                {
                    CategoryMenu catMenu = new CategoryMenu();
                    while (true)
                    {
                        int catOption = mainMenu.CategoryMenu();
                        if (catOption == 1)
                        {
                            catMenu.NewCategory();
                        }
                        else if (catOption == 2)
                        {
                            catMenu.EditCategory();
                        }
                        else if (catOption == 3)
                        {
                            catMenu.DeleteCategory();
                        }
                        else if (catOption == 4)
                        {
                            catMenu.ViewCategories();
                        }
                        else if (catOption == 5)
                        {
                            break;
                        }
                        else if (catOption == 6)
                        {
                            return;
                        }
                    }

                }
                else if(mainOption == 4)
                {
                    mainMenu.HelpMenu();
                }
                else if (mainOption == 5)
                {
                    return;
                }
            }
        }
    }
}
