using FamilyAccounting.Database.Movement;
using FamilyAccounting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FamilyAccounting.Program
{
    class MovementMenu
    {
        private MovementDb movementDb;
        private Pagination pagination;
        private static int currentPage;

        public MovementMenu()
        {
            currentPage = 0;
            pagination = new Pagination();
            movementDb = new MovementDb();
        }

        internal void NewMovement()
        {
            string name;
            string value;
            string date;
            int sourceId;
            int categoryId;
            string answer;
            do
            {
                Console.WriteLine("Do you want to create a new movement?(Y/y/N/n)");
                answer = Console.ReadLine().ToLower();
            } while (!answer.Equals("y") && !answer.Equals("n"));
            if (answer.Equals("y"))
            {
                SourceMenu sourceMenu = new SourceMenu();
                CategoryMenu categoryMenu = new CategoryMenu();

                // creating name
                do
                {
                    Console.WriteLine("Please introduce a name:");
                    name = Console.ReadLine();
                } while (name.Equals(""));
                sourceId = SelectSource();
                if (sourceId == -1)
                {
                    Console.WriteLine("First you must create a new money source.\nPlease press Enter key to back and create a new money source");
                }
                else
                {
                    categoryId = SelectCategory();
                    if (categoryId == -1)
                    {
                        Console.WriteLine("First you must create a new category.\nPlease press Enter key to back and create a new money source");
                    }
                    else
                    {
                        // value
                        do
                        {
                            Console.WriteLine("Please introduce a numeric value for the movement (format [-]x.xx):");
                            value = Console.ReadLine();
                        } while (!Regex.IsMatch(value, Constants.DECIMAL_NUMBER_REGEX));
                        Console.WriteLine("value: " + value);
                        // date
                        do
                        {
                            Console.WriteLine("Do you want to set a date?(Y/y/N/n): ");
                            answer = Console.ReadLine().ToLower();
                        } while (!answer.Equals("y") && !answer.Equals("n"));
                        if (answer.Equals("y"))
                        {
                            date = SetDate();
                        }
                        else
                        {
                            date = DateTimeOffset.Now.ToString("u").Substring(0, 10);
                        }
                        movementDb.NewMovement(sourceId, categoryId, name, float.Parse(value.Replace('.', ',')), date);
                    }
                }
            }
        }

        internal void EditMovement()
        {
            List<string> movement;
            string name;
            string value;
            int sourceId;
            int categoryId;
            while (true)
            {
                // Showing the data and asking if want to edit or not.
                string answer = null;
                do
                {
                    movement = SelectMovement(true);
                    if (movement.ElementAt(0).Equals("-1"))
                    {
                        break;
                    }

                    Console.WriteLine("Do you want to edit that movement?(Y/y/N/n)\nId " + movement.ElementAt(0) + " | Name " + movement.ElementAt(1) + " | Date " + movement.ElementAt(2) + " | Source Name " + movement.ElementAt(3) + " | Category Name " + movement.ElementAt(4) + " | Value " + (float.Parse(movement.ElementAt(5)) > 0 ? movement.ElementAt(5) : "-" + movement.ElementAt(6)));
                    answer = Console.ReadLine();
                } while (!answer.Equals("n") && !answer.Equals("y"));
                if (movement.ElementAt(0).Equals("-1") || answer.Equals("y"))
                {
                    break;
                }
            }

            // user wants to edit
            if (!movement.ElementAt(0).Equals("-1"))
            {
                string answer;

                // asking if user wants to edit the current name.
                do
                {
                    Console.WriteLine("Current name: " + movement.ElementAt(1) + "\nDo you wish to change it?(Y/y/N/n)");
                    answer = Console.ReadLine().ToLower();
                } while (!answer.Equals("y") && !answer.Equals("n"));
                if (answer.Equals("y"))
                {
                    do
                    {
                        Console.WriteLine("Introduce the new name:");
                        name = Console.ReadLine();
                    } while (name.Equals(""));
                }
                else
                {
                    name = movement.ElementAt(1);
                }
                // end for asking name.

                // asking for value.
                do
                {
                    Console.WriteLine("Current movement value: " + (float.Parse(movement.ElementAt(5)) > 0 ? movement.ElementAt(5) : "-"
                        + movement.ElementAt(6)) + "\nDo you wish to change it?(Y/y/N/n)");
                    answer = Console.ReadLine().ToLower();
                } while (!answer.Equals("y") && !answer.Equals("n"));
                if (answer.Equals("y"))
                {
                    do
                    {
                        Console.WriteLine("Introduce the new total value:");
                        value = Console.ReadLine();
                    } while (!Regex.IsMatch(value, Constants.DECIMAL_NUMBER_REGEX));
                }
                else
                {
                    value = float.Parse(movement.ElementAt(5)) > 0 ? movement.ElementAt(5) : "-" + movement.ElementAt(6);
                }
                // end asking value

                // asking money source
                do
                {
                    Console.WriteLine("Current money source: " + movement.ElementAt(3));
                    answer = Console.ReadLine();
                } while (!answer.Equals("y") && !answer.Equals("n"));
                if (answer.Equals("y"))
                {
                    sourceId = SelectSource();
                }
                else
                {
                    sourceId = int.Parse(movement.ElementAt(7));
                }
                //end asking money source

                // asking category
                do
                {
                    Console.WriteLine("Current category: " + movement.ElementAt(4));
                    answer = Console.ReadLine();
                } while (!answer.Equals("y") && !answer.Equals("n"));
                if (answer.Equals("y"))
                {
                    categoryId = SelectSource();
                }
                else
                {
                    categoryId = int.Parse(movement.ElementAt(8));
                }
                //end asking category

                movementDb.EditMovement(int.Parse(movement.ElementAt(0)), sourceId, categoryId, name, float.Parse(value.Replace('.', ',')), float.Parse(movement.ElementAt(5).Replace('.', ',')) > 0 ? float.Parse(movement.ElementAt(5).Replace('.', ',')) : float.Parse("-" + movement.ElementAt(6).Replace('.', ',')));
            }
        }

        internal void DeleteMovement()
        {
            List<string> movement;
            while (true)
            {
                // Showing the data and asking if want to delete or not.
                string answer = null;
                do
                {
                    movement = SelectMovement(true);
                    if (movement.ElementAt(0).Equals("-1"))
                    {
                        break;
                    }
                    Console.WriteLine("Do you want to delete that movement?(Y/y/N/n)\nId " + movement.ElementAt(0) + " | Name " + movement.ElementAt(1) + " | Date " + movement.ElementAt(2) + " | Source Name " + movement.ElementAt(3) + " | Category Name " + movement.ElementAt(4) + " | Value " + (float.Parse(movement.ElementAt(5)) > 0 ? movement.ElementAt(5) : "-" + movement.ElementAt(6)));
                    answer = Console.ReadLine();
                } while (!answer.Equals("n") && !answer.Equals("y"));
                if (movement.ElementAt(0).Equals("-1") || answer.Equals("y"))
                {
                    break;
                }
            }

            // user wants to delete it
            if (!movement.ElementAt(0).Equals("-1"))
            {
                movementDb.DeleteMovement(int.Parse(movement.ElementAt(0)), int.Parse(movement.ElementAt(7)), float.Parse(movement.ElementAt(5)) > 0 ? float.Parse(movement.ElementAt(5)) : float.Parse("-" + movement.ElementAt(6)));
            }
        }

        internal void ViewMovements()
        {
            List<string> movements = null;
            while (movements == null)
            {
                movements = SelectMovement(false);
            }
        }

        internal int TotalMovements()
        {
            return movementDb.TotalMovements();
        }

        /// <summary>
        /// This return: 
        ///     id 
        ///     {
        ///         name[0] 
        ///         movement_date[1] 
        ///         source_name[2]
        ///         category_name[3]
        ///         income[4]
        ///         outgoing[5]
        ///         source_id[6]
        ///         category_id[7]
        ///     }
        /// </summary>
        /// <param name="currentElement"></param>
        /// <param name="totalElement"></param>
        /// <returns></returns>
        private Dictionary<int, List<string>> ShowMovements(int currentElement, int totalElement = 25)
        {
            return movementDb.GetMovements(currentElement, totalElement);
        }

        private int SelectSource()
        {
            SourceMenu sourceMenu = new SourceMenu();
            string sourceId;
            currentPage = 1;
            pagination.TotalPages(sourceMenu.TotalSources());
            Dictionary<int, List<string>> sources;
            bool[] prevNext;
            Console.WriteLine("Please select an sourceId");
            while (true)
            {
                if (sourceMenu.TotalSources() <= 0)
                {
                    return -1;
                }
                sources = sourceMenu.ShowSources((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                // Showing the data.*
                do
                {
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value.ToArray()));
                    }
                    Console.WriteLine("Please select an id or press " + Constants.PAG_UP + " to back page " + Constants.PAG_DOWN + " to next page");
                    sourceId = Console.ReadLine();
                } while (!Regex.IsMatch(sourceId, "^[0-9]+$") && !sourceId.Equals(Constants.PAG_DOWN.ToString()) && !sourceId.Equals(Constants.PAG_UP.ToString()));
                if (prevNext[1] && sourceId.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && sourceId.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (Regex.IsMatch(sourceId, "^[0-9]+$") && sources.ContainsKey(int.Parse(sourceId)))
                {
                    string answerSource;
                    List<string> source;
                    sources.TryGetValue(int.Parse(sourceId), out source);
                    do
                    {
                        Console.WriteLine("Do you want that money source?(Y/y/N/n)\n" + sourceId + " | " + source.ElementAt(0) + " | " + source.ElementAt(1));
                        answerSource = Console.ReadLine().ToLower();
                    } while (!answerSource.Equals("y") && !answerSource.Equals("n"));
                    if (answerSource.Equals("y"))
                    {
                        return int.Parse(sourceId);
                    }
                }
            }
        }

        private int SelectCategory()
        {
            CategoryMenu categoryMenu = new CategoryMenu();
            string categoryId;
            bool[] prevNext;
            currentPage = 1;
            int totalCategories = categoryMenu.TotalCategories();
            pagination.TotalPages(totalCategories);
            Dictionary<int, string> categories;
            string answer;

            while (true)
            {
                if (totalCategories <= 1)
                {
                    return -1;
                }
                categories = categoryMenu.ShowCategories((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                // Showing the data.
                do
                {
                    Console.WriteLine("ID | Name");
                    foreach (KeyValuePair<int, string> category in categories)
                    {
                        Console.WriteLine(category.Key + " | " + category.Value);
                    }
                    Console.WriteLine("Please select an id or press " + Constants.PAG_UP + " to back page " + Constants.PAG_DOWN + " to next page");
                    categoryId = Console.ReadLine();
                } while (!Regex.IsMatch(categoryId, "^[0-9]+$") && !categoryId.Equals(Constants.PAG_DOWN.ToString()) && !categoryId.Equals(Constants.PAG_UP.ToString()));
                if (prevNext[1] && categoryId.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && categoryId.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (Regex.IsMatch(categoryId, "^[0-9]+$") && categories.ContainsKey(int.Parse(categoryId)))
                {
                    string answerCategory;
                    string category;
                    categories.TryGetValue(int.Parse(categoryId), out category);
                    do
                    {
                        Console.WriteLine("Do you want that category?(Y/y/N/n)\n" + categoryId + " | " + category);
                        answerCategory = Console.ReadLine().ToLower();
                    } while (!answerCategory.Equals("y") && !answerCategory.Equals("n"));
                    if (answerCategory.Equals("y"))
                    {
                        return int.Parse(categoryId);
                    }
                }
            }
        }

        private List<string> SelectMovement(bool editDelete)
        {
            string id;
            currentPage = 1;
            Dictionary<int, List<string>> movements;
            bool[] prevNext;
            pagination.TotalPages(TotalMovements());
            while (true)
            {
                movements = ShowMovements((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                do
                {
                    Console.WriteLine("ID | Name | Date | Source | Category | Value");
                    foreach (KeyValuePair<int, List<string>> movement in movements)
                    {
                        Console.WriteLine(movement.Key + " | " + movement.Value.ElementAt(0) + " | " + movement.Value.ElementAt(1) + " | "
                            + movement.Value.ElementAt(2) + " | " + movement.Value.ElementAt(3) + " | " + (float.Parse(movement.Value.ElementAt(4)) > 0 ? movement.Value.ElementAt(4) : "-" + movement.Value.ElementAt(5)));
                    }
                    if (!prevNext[0] && prevNext[1])
                    {
                        Console.WriteLine("Current page: " + currentPage + "\nPress " + Constants.PAG_DOWN
                            + " to next page or " + Constants.BACK_KEY + " to back to money source menu");
                    }
                    else if (prevNext[0] && prevNext[1])
                    {
                        Console.WriteLine("Current page: " + currentPage + "\nPress " + Constants.PAG_UP + " to back page or "
                            + Constants.PAG_DOWN + " to next page or " + Constants.BACK_KEY + " to back to money source menu");
                    }
                    else if (prevNext[0] && !prevNext[1])
                    {
                        Console.WriteLine("Current page: " + currentPage + "\nPress " + Constants.PAG_UP
                            + " to back page or " + Constants.BACK_KEY + " to back to money source menu");
                    }
                    else if (!prevNext[0] && !prevNext[1])
                    {
                        Console.WriteLine("Press " + Constants.BACK_KEY + " to back to money source menu");
                    }
                    if (editDelete)
                    {
                        Console.WriteLine("Select an ID if you wish to work with it");
                    }
                    id = Console.ReadLine();
                } while (!Regex.IsMatch(id, "^[0-9]+$") && !id.Equals(Constants.BACK_KEY.ToString()) && !id.Equals(Constants.PAG_DOWN.ToString()) && !id.Equals(Constants.PAG_UP.ToString()));
                if (id.Equals(Constants.BACK_KEY.ToString()))
                {
                    List<string> movementSelected = new List<string>();
                    movementSelected.Add("-1");
                    return movementSelected;
                }
                else if (prevNext[1] && id.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && id.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (editDelete && Regex.IsMatch(id, "^[0-9]+$") && movements.ContainsKey(int.Parse(id)))
                {
                    List<string> movementSelected;
                    movements.TryGetValue(int.Parse(id), out movementSelected);
                    movementSelected.Insert(0, id);
                    return movementSelected;
                }
            }
        }

        private string SetDate()
        {
            string month, day, year, answer;
            while (true)
            {
                answer = "";
                do
                {
                    Console.WriteLine("Introduce the month in number:(mm)");
                    month = Console.ReadLine();
                    if (Regex.IsMatch(month, "^(0[1-9]|1[012])$"))
                    {
                        Console.WriteLine("Do you really want that month?(Y/y/N/n) " + month);
                        answer = Console.ReadLine().ToLower();
                    }
                    else
                    {
                        Console.WriteLine("Please introduce a number between 01-12.");
                    }
                } while (!answer.Equals("y"));
                answer = "";
                do
                {
                    Console.WriteLine("Introduce the day in number:(dd)");
                    day = Console.ReadLine();
                    if (Regex.IsMatch(day, "^(0[1-9]|[12]\\d|3[01])$"))
                    {
                        Console.WriteLine("Do you really want that day?(Y/y/N/n) " + day);
                        answer = Console.ReadLine().ToLower();
                    }
                    else
                    {
                        Console.WriteLine("Please introduce a number between 01-31.");
                    }
                } while (!answer.Equals("y"));
                answer = "";
                do
                {
                    Console.WriteLine("Introduce the year in number:(yyyy)");
                    year = Console.ReadLine();
                    if (Regex.IsMatch(year, "^(19|20)\\d{2}$"))
                    {
                        Console.WriteLine("Do you really want that year?(Y/y/N/n) " + year);
                        answer = Console.ReadLine().ToLower();
                    }
                    else
                    {
                        Console.WriteLine("Please introduce a number between 1900-2099");
                    }
                } while (!answer.Equals("y"));
                answer = "";
                do
                {
                    Console.WriteLine("Do you really want that date[mm/dd/yyyy]?(Y/y/N/n)\n" + month + "/" + day + "/" + year);
                    answer = Console.ReadLine();
                } while (!answer.Equals("n") && !answer.Equals("y"));
                if (answer.Equals("y"))
                {
                    return year + "/" + month + "/" + day;
                }
            }
        }
    }
}

