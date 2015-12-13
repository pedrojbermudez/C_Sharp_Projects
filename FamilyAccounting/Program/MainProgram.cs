using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Program
{
    class MainProgram
    {
        public static void Main(string[] args)
        {
            MenuManager mainMenu = new MenuManager();
            int optionMain = -1;
            int optionSecondary = -1;

            bool back = true;
            while (true)
            {
                if (back)
                {
                    Console.Write(mainMenu.MainMenu());
                    back = false;
                }
                optionMain = Console.Read();
                if (optionMain >= 1 || optionMain <= 3)
                {
                    switch (optionMain)
                    {
                        case 1:
                            Console.Write(mainMenu.SourceMenu());
                            SourceMenu sourceMenu = new SourceMenu();
                            while (true)
                            {
                                optionSecondary = Console.Read();
                                if (optionSecondary >= 1 || optionSecondary <= 4)
                                {
                                    switch (optionSecondary)
                                    {
                                        case 1:
                                            break;
                                        case 2:
                                            break;
                                        case 3:
                                            break;
                                        case 4:
                                            break;
                                    }
                                }
                                else if (optionSecondary == 5)
                                {
                                    optionSecondary = -1;
                                    back = true;
                                    break;
                                }
                                else if (optionSecondary == 6)
                                {
                                    optionSecondary = -1;
                                    optionMain = 4;
                                    break;
                                }
                                else
                                {
                                    Console.Write("Please select an option number (1-6)");
                                }
                            }
                            break;
                        case 2:
                            Console.Write(mainMenu.MovementMenu());
                            while (true)
                            {
                                optionSecondary = Console.Read();
                                if (optionSecondary >= 1 || optionSecondary <= 4)
                                {
                                    switch (optionSecondary)
                                    {
                                        case 1:
                                            break;
                                        case 2:
                                            break;
                                        case 3:
                                            break;
                                        case 4:
                                            break;
                                    }
                                }
                                else if (optionSecondary == 5)
                                {
                                    optionSecondary = -1;
                                    break;
                                }
                                else if (optionSecondary == 6)
                                {
                                    optionSecondary = -1;
                                    optionMain = 4;
                                    break;
                                }
                                else
                                {
                                    Console.Write("Please select an option number (1-6)");
                                }
                            }
                            break;
                        case 3:
                            Console.Write(mainMenu.CategoryMenu());
                            while (true)
                            {
                                optionSecondary = Console.Read();
                                if (optionSecondary >= 1 || optionSecondary <= 4)
                                {
                                    switch (optionSecondary)
                                    {
                                        case 1:
                                            break;
                                        case 2:
                                            break;
                                        case 3:
                                            break;
                                        case 4:
                                            break;
                                    }
                                }
                                else if (optionSecondary == 5)
                                {
                                    optionSecondary = -1;
                                    break;
                                }
                                else if (optionSecondary == 6)
                                {
                                    optionSecondary = -1;
                                    optionMain = 4;
                                    break;
                                }
                                else
                                {
                                    Console.Write("Please select an option number (1-6)");
                                }
                            }
                            break;
                    }
                }
                else if (optionMain == 4)
                {
                    break;
                }
                else
                {
                    Console.Write("Please select an option number (1-4)");
                }
            }
        }
    }
}
