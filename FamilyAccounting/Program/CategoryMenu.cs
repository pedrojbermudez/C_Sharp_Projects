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
        private static bool back;

        public CategoryMenu()
        {
            currentPage = 0;
            back = false;
            pagination = new Pagination();
            categoryDb = new CategoryDb();
        }

        public void NewCategory()
        {
            string name;
            do
            {
                Console.WriteLine("Please introduce a name:");
                name = Console.ReadLine();
            } while (name.Equals(""));
            categoryDb.NewCategory(name);
        }

        public void EditSource()
        {
            string id;
            string name;
            string total;
            string editSource;
            while (true)
            {
                Dictionary<int, string> sources = ShowCategories(currentPage * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                // Showing the data.
                do
                {
                    // TODO pagination.
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, string> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value.ToArray()));
                    }
                    Console.WriteLine("Please select an id or press " + Constants.BACK_KEY + " if you wish to back.");
                    id = Console.ReadLine();
                    Console.WriteLine("id value: " + id);
                    Console.WriteLine();
                } while (!Regex.IsMatch(id, "^[0-9]+$") && !id.Equals(Constants.BACK_KEY.ToString()));
                if (id.Equals(Constants.BACK_KEY.ToString()))
                {
                    break;
                }
                else if (Regex.IsMatch(id, "^[0-9]+$") && sources.ContainsKey(int.Parse(id)))
                {
                    sources.TryGetValue(int.Parse(id), out editSource);
                    string answer;
                    do
                    {
                        Console.WriteLine("Current name: " + editSource + "\nDo you wish to change it?(Y/y/N/n)");
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
                        name = editSource;
                    }
                    categoryDb.EditCategory(int.Parse(id), name);
                }
                else if (id.Equals(Constants.BACK_KEY.ToString()))
                {
                    break;
                }
            }
        }

        public void DeleteSource()
        {
            string id;
            string deleteSource;
            Dictionary<int, string> sources = ShowCategories(currentPage * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
            while (true)
            {
                // Showing the data.
                do
                {
                    // TODO pagination.
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, string> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + source.Value);
                    }
                    Console.WriteLine("Please select an id or press " + Constants.BACK_KEY + " if you wish to back");
                    id = Console.ReadLine();
                } while (!Regex.IsMatch(id, "^[0-9]+$") && !id.Equals(Constants.BACK_KEY.ToString()));
                if (id.Equals(Constants.BACK_KEY.ToString())) { break; }
                if (Regex.IsMatch(id, "^[0-9]+$") && sources.ContainsKey(int.Parse(id)))
                {
                    // ID is correct.
                    string answer;
                    sources.TryGetValue(int.Parse(id), out deleteSource);
                    do
                    {
                        Console.WriteLine("The choosed source: id => " + id + " | name => " + deleteSource.ElementAt(0) + " | total => " + deleteSource.ElementAt(1) + "\nDo you wish to delete it?(Y/y/N/n)");
                        answer = Console.ReadLine().ToLower();
                    } while (!answer.Equals("y") && !answer.Equals("n"));
                    if (answer.Equals("y"))
                    {
                        // Deleting source.
                        categoryDb.DeleteCategory(int.Parse(id));
                        break;
                    }
                }
                else if (id.Equals(Constants.BACK_KEY.ToString()))
                {
                    break;
                }
            }
        }

        public void ViewCategories()
        {
            int currentPage = 1;
            Dictionary<int, string> categories;
            int totalPages = pagination.TotalPages(TotalCategories());
            bool[] prevNext;
            while (true)
            {
                string option;
                prevNext = pagination.ShowPagination(currentPage);
                Console.WriteLine("current page => " + currentPage);
                categories = ShowCategories((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                do
                {
                    Console.WriteLine("ID | Name | Total");
                    Console.WriteLine("prevNext[0] => " + prevNext[0] + " prevNext[1] => " + prevNext[1]);
                    foreach (KeyValuePair<int, string> category in categories)
                    {
                        Console.WriteLine(category.Key + " | " +  category.Value);
                    }
                    Console.WriteLine("Please select an option: " + Constants.PAG_UP + " to back page " + Constants.PAG_DOWN + " to next page");
                    option = Console.ReadLine();
                    if (option.Equals(Constants.BACK_KEY.ToString()))
                    {
                        break;
                    }
                } while (!option.Equals(Constants.PAG_UP.ToString()) && !option.Equals(Constants.PAG_DOWN.ToString()));
                if (prevNext[1] && option.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && option.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (option.Equals(Constants.BACK_KEY.ToString()))
                {
                    break;
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

        public void SetBack()
        {
            back = back ? !back : back;
        }
    }
}
