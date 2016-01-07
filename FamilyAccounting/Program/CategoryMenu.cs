using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FamilyAccounting.Database.Category;
using FamilyAccounting.Utils;

namespace FamilyAccounting.Program
{
    class CategoryMenu
    {
        private CategoryDb categoryDb;
        private Pagination pagination;
        private static int currentPage;

        public CategoryMenu()
        {
            currentPage = 0;
            pagination = new Pagination();
            categoryDb = new CategoryDb();
        }

        public void NewCategory()
        {
            string name;
            string answer;
            do
            {
                Console.WriteLine("Do you want to create a new category?(Y/y/N/n)");
                answer = Console.ReadLine().ToLower();
            } while (!answer.ToLower().Equals("y") && !answer.ToLower().Equals("n"));
            if (answer.Equals("y"))
            {
                do
                {
                    Console.WriteLine("Please introduce a name:");
                    name = Console.ReadLine();
                } while (name.Equals(""));
                categoryDb.NewCategory(name);
            }
        }

        public void EditCategory()
        {
            string id;
            string name;
            List<string> category;
            string answer = null;
            while (true)
            {
                // Showing the data and asking if want to edit or not.
                do
                {
                    category = SelectCategory(true);
                    if (!category.ElementAt(0).Equals("-1"))
                    {
                        Console.WriteLine("Do you really want to edit that category?(Y/y/N/n)\nID " + category.ElementAt(0) + " | Name " + category.ElementAt(1));
                        answer = Console.ReadLine().ToLower();
                    }
                    else
                    {
                        answer = "n";
                    }
                    
                } while (!answer.Equals("n") && !answer.Equals("y"));
                if (category.ElementAt(0).Equals("-1") || answer.Equals("y"))
                {
                    break;
                }
            }

            if (!category.ElementAt(0).Equals("-1"))
            {
                do
                {
                    Console.WriteLine("Current name: " + category.ElementAt(1) + "\nDo you wish to change it?(Y/y/N/n)");
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
                    name = category.ElementAt(1);
                }
                categoryDb.EditCategory(int.Parse(category.ElementAt(0)), name);
            }
        }

        public void DeleteCategory()
        {
            List<string> category;
            while (true)
            {
                // Showing data and if want delete.
                string answer = null;
                do
                {
                    category = SelectCategory(true);
                    if (!category.ElementAt(0).Equals("-1"))
                    {
                        Console.WriteLine("Do you want to delete that category?(Y/y/N/n)\nID " + category.ElementAt(0) + "| Name " + category.ElementAt(1));
                        answer = Console.ReadLine().ToLower();
                    }
                    else
                    {
                        answer = "n";
                    }
                } while (!answer.Equals("n") && !answer.Equals("y"));

                if (category.ElementAt(0).Equals("-1") || answer.Equals("y"))
                {
                    break;
                }
            }
            if (!category.ElementAt(0).Equals("-1"))
            {
                // Deleting category
                categoryDb.DeleteCategory(int.Parse(category.ElementAt(0)));
            }
        }

        public void ViewCategories()
        {
            List<string> categories = null;
            while (categories == null)
            {
                categories = SelectCategory(false);
            }
        }

        private List<string> SelectCategory(bool editDelete)
        {
            string id;
            currentPage = 1;
            Dictionary<int, string> categories;
            bool[] prevNext;
            pagination.TotalPages(TotalCategories());
            while (true)
            {
                categories = ShowCategories((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                do
                {
                    Console.WriteLine("ID | Name");
                    foreach (KeyValuePair<int, string> category in categories)
                    {
                        Console.WriteLine(category.Key + " | " + category.Value);
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
                    List<string> categorySelected = new List<string>();
                    categorySelected.Add("-1");
                    return categorySelected;
                }
                else if (prevNext[1] && id.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && id.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (editDelete && Regex.IsMatch(id, "^[0-9]+$") && categories.ContainsKey(int.Parse(id)))
                {
                    List<string> category = new List<string>();
                    string categorySelected;
                    categories.TryGetValue(int.Parse(id), out categorySelected);
                    category.Insert(0, id);
                    category.Insert(1, categorySelected);
                    return category;
                }
            }
        }

        public int TotalCategories()
        {
            return categoryDb.TotalCategories();
        }

        public Dictionary<int, string> ShowCategories(int currentElement, int totalElement = 25)
        {
            return categoryDb.GetCategories(currentElement, totalElement);
        }
    }
}
