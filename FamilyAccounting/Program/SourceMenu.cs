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

        public SourceMenu()
        {
            currentPage = 0;
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
                answer = Console.ReadLine().ToLower();
            } while (!answer.Equals("y") && !answer.Equals("n"));
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
                } while (!Regex.IsMatch(total, Constants.DECIMAL_NUMBER_REGEX));
                sourceDb.NewSource(name, total);
            }
        }

        public void EditSource()
        {
            string id;
            string name;
            string total;
            string answer = null;
            List<string> source;
            while (true)
            {
                // Showing the data and asking if want to edit or not.
                do
                {
                    source = SelectSource(true);
                    if (source.ElementAt(0).Equals("-1"))
                    {
                        break;
                    }
                    Console.WriteLine("Do you want to edit that source?(Y/y/N/n)\nID " + source.ElementAt(0) + " | Name " + source.ElementAt(1) + " | Total " + source.ElementAt(2));
                    answer = Console.ReadLine().ToLower();
                } while (!answer.Equals("n") && !answer.Equals("y"));
                if (source.ElementAt(0).Equals("-1") || answer.Equals("y"))
                {
                    break;
                }
            }
            if (!source.ElementAt(0).Equals("-1"))
            {
                do
                {
                    Console.WriteLine("Current name: " + source.ElementAt(1) + "\nDo you wish to change it?(Y/y/N/n)");
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
                    name = source.ElementAt(1);
                }
                do
                {
                    Console.WriteLine("Current total value: " + source.ElementAt(2) + "\nDo you wish to change it?(Y/y/N/n)");
                    answer = Console.ReadLine().ToLower();
                } while (!answer.Equals("y") && !answer.Equals("n"));
                if (answer.Equals("y"))
                {
                    do
                    {
                        Console.WriteLine("Introduce the new total value:");
                        total = Console.ReadLine();
                    } while (!Regex.IsMatch(total, Constants.DECIMAL_NUMBER_REGEX));
                }
                else
                {
                    total = source.ElementAt(2);
                }
                sourceDb.EditSource(int.Parse(source.ElementAt(0)), name, total);
            }
        }

        public void DeleteSource()
        {
            List<string> source;
            while (true)
            {
                string answer = null;
                do
                {
                    source = SelectSource(true);
                    if (source.Equals("-1"))
                    {
                        break;
                    }
                    Console.WriteLine("Do you want to delete that source?(Y/y/N/n)\nID " + source.ElementAt(0)
                        + " | Name " + source.ElementAt(1) + " | Total " + source.ElementAt(2));
                    answer = Console.ReadLine().ToLower();
                } while (!answer.Equals("n") && !answer.Equals("y"));
                if (source.ElementAt(0).Equals("-1") || answer.Equals("y"))
                {
                    break;
                }
            }
            if (!source.ElementAt(0).Equals("-1"))
            {
                sourceDb.DeleteSource(int.Parse(source.ElementAt(0)));
            }
        }

        public void ViewSources()
        {
            List<string> sources = null;
            while (sources == null)
            {
                sources = SelectSource(false);
            }
        }

        private List<string> SelectSource(bool editDelete)
        {
            string id;
            currentPage = 1;
            Dictionary<int, List<string>> sources;
            bool[] prevNext;
            pagination.TotalPages(TotalSources());
            while (true)
            {
                sources = ShowSources((currentPage - 1) * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
                prevNext = pagination.ShowPagination(currentPage);
                do
                {
                    Console.WriteLine("ID | Name | Total ");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + source.Value.ElementAt(0) + " | " + source.Value.ElementAt(1));
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
                else if (editDelete && Regex.IsMatch(id, "^[0-9]+$") && sources.ContainsKey(int.Parse(id)))
                {
                    List<string> sourceSelected;
                    sources.TryGetValue(int.Parse(id), out sourceSelected);
                    sourceSelected.Insert(0, id);
                    return sourceSelected;
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
    }
}
