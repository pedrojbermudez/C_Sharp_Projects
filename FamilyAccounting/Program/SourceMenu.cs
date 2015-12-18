using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using FamilyAccounting.Database.Source;
using FamilyAccounting.Utils;

namespace FamilyAccounting.Program
{
    class SourceMenu
    {
        private SourceDb sourceDb;
        private Pagination pagination;
        private static int currentPage;
        private static bool back;

        public SourceMenu()
        {
            currentPage = 0;
            back = false;
            pagination = new Pagination();
            sourceDb = new SourceDb();
        }

        public void NewSource()
        {
            string name;
            string total;
            string answer;
            do
            {
                Console.WriteLine("Do you want to create a new money source?(Y/y/N/n)");
                answer = Console.ReadLine();
            } while (!answer.ToLower().Equals("y") && !answer.ToLower().Equals("n"));
            if (answer.Equals("y"))
            {
                do
                {
                    Console.WriteLine("Please introduce a name:");
                    name = Console.ReadLine();
                } while (name.Equals(""));
                do
                {
                    Console.WriteLine("Please introduce a total value for the money source (format [-]x.xx):");
                    total = Console.ReadLine();
                } while (!Regex.IsMatch(total, Constants.DECIMAL_TOTAL_NUMBER_REGEX));
                sourceDb.NewSource(name, total);
            }
        }

        public void EditSource()
        {
            string id;
            string name;
            string total;
            List<string> editSource;
            currentPage = 1;
            Dictionary<int, List<string>> sources;
            bool[] prevNext;
            pagination.TotalPages(TotalSources());
            while (true)
            {
                sources = ShowSources((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                // Showing the data.
                do
                {
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value.ToArray()));
                    }
                    Console.WriteLine("Please select an id or press " + Constants.BACK_KEY + " if you wish to back. " + Constants.PAG_UP + " to back page " + Constants.PAG_DOWN + " to next page");
                    id = Console.ReadLine();
                } while (!Regex.IsMatch(id, "^[0-9]+$") && !id.Equals(Constants.BACK_KEY.ToString()) && !id.Equals(Constants.PAG_DOWN.ToString()) && !id.Equals(Constants.PAG_UP.ToString()));
                if (id.Equals(Constants.BACK_KEY.ToString()))
                {
                    break;
                }
                else if (prevNext[1] && id.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && id.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (Regex.IsMatch(id, "^[0-9]+$") && sources.ContainsKey(int.Parse(id)))
                {
                    sources.TryGetValue(int.Parse(id), out editSource);
                    string answer;
                    do
                    {
                        Console.WriteLine("Current name: " + editSource.ElementAt(0) + "\nDo you wish to change it?(Y/y/N/n)");
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
                        name = editSource.ElementAt(0);
                    }
                    do
                    {
                        Console.WriteLine("Current total value: " + editSource.ElementAt(1) + "\nDo you wish to change it?(Y/y/N/n)");
                        answer = Console.ReadLine().ToLower();
                    } while (!answer.Equals("y") && !answer.Equals("n"));
                    if (answer.Equals("y"))
                    {
                        do
                        {
                            Console.WriteLine("Introduce the new total value:");
                            total = Console.ReadLine();
                        } while (!Regex.IsMatch(total, Constants.DECIMAL_TOTAL_NUMBER_REGEX));
                    }
                    else
                    {
                        total = editSource.ElementAt(1);
                    }
                    sourceDb.EditSource(int.Parse(id), name, total);
                }
            }
        }

        public void DeleteSource()
        {
            string id;
            List<string> deleteSource;
            currentPage = 1;
            Dictionary<int, List<string>> sources;
            bool[] prevNext;
            pagination.TotalPages(TotalSources());
            while (true)
            {
                sources = ShowSources((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                // Showing the data.
                do
                {
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value.ToArray()));
                    }
                    Console.WriteLine("Please select an id or press " + Constants.BACK_KEY + " if you wish to back. " + Constants.PAG_UP + " to back page " + Constants.PAG_DOWN + " to next page");
                    id = Console.ReadLine();
                } while (!Regex.IsMatch(id, "^[0-9]+$") && !id.Equals(Constants.BACK_KEY.ToString()) && !id.Equals(Constants.PAG_DOWN.ToString()) && !id.Equals(Constants.PAG_UP.ToString()));
                if (id.Equals(Constants.BACK_KEY.ToString()))
                {
                    break;
                }
                else if (prevNext[1] && id.Equals(Constants.PAG_DOWN.ToString()))
                {
                    currentPage++;
                }
                else if (prevNext[0] && id.Equals(Constants.PAG_UP.ToString()))
                {
                    currentPage--;
                }
                else if (Regex.IsMatch(id, "^[0-9]+$") && sources.ContainsKey(int.Parse(id)))
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
                        sourceDb.DeleteSource(int.Parse(id));
                        pagination.TotalPages(TotalSources());
                        break;
                    }
                }
            }
        }

        public void ViewSources()
        {
            int currentPage = 1;
            Dictionary<int, List<string>> sources;
            int totalPages = pagination.TotalPages(TotalSources());
            bool[] prevNext;
            while (true)
            {
                string option;
                prevNext = pagination.ShowPagination(currentPage);
                sources = ShowSources((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                do
                {
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value));
                    }
                    Console.WriteLine("Please select an option: " + Constants.PAG_UP + " to back page, " + Constants.PAG_DOWN + " to next page, " + Constants.BACK_KEY + " to back to money source menu");
                    option = Console.ReadLine();
                } while (!option.Equals(Constants.PAG_UP.ToString()) && !option.Equals(Constants.PAG_DOWN.ToString()) && !option.Equals(Constants.BACK_KEY.ToString()));
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

        public int TotalSources()
        {
            return sourceDb.TotalSources();
        }

        public Dictionary<int, List<string>> ShowSources(int currentElement, int totalElement = 25)
        {
            return sourceDb.GetSources(currentElement, totalElement);
        }

        public void SetBack()
        {
            back = back ? !back : back;
        }
    }
}
