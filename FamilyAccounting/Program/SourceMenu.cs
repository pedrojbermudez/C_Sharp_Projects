using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FamilyAccounting.Database.Source;
using FamilyAccounting.Utils;

namespace FamilyAccounting.Program
{
    class SourceMenu
    {
        private SourceDb sourceDb;
        private static int currentPage;
        private static bool back;

        public SourceMenu()
        {
            currentPage = 0;
            back = false;
            sourceDb = new SourceDb();
        }

        public void NewSource()
        {
            string name;
            string total;
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
            sourceDb.NewSource(name, float.Parse(total));
        }

        public void EditSource()
        {
            string id;
            string name;
            string total;
            Dictionary<int, List<string>> sources = ShowSources(currentPage * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
            List<string> editSource;
            while (true)
            {
                // Showing the data.
                do
                {
                    // TODO pagination.
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value.ToArray()));
                    }
                    Console.WriteLine("Please select an id or press " + Constants.BACK_KEY + " if you wish to back.");
                    id = Console.ReadLine();
                } while (Regex.IsMatch(id, "^[0-9]+$") || id.Equals(Constants.BACK_KEY));
                if (sources.ContainsKey(int.Parse(id)))
                {
                    sources.TryGetValue(int.Parse(id), out editSource);
                    string answer;
                    do
                    {
                        Console.WriteLine("Current name: " + editSource.ElementAt(0) + "\nDo you wish to change it?(Y/y/N/n)");
                        answer = Console.ReadLine().ToLower();
                    } while (!answer.Equals("y") || !answer.Equals("n"));
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
                    } while (!answer.Equals("y") || !answer.Equals("n"));
                    if (answer.Equals("y"))
                    {
                        do
                        {
                            Console.WriteLine("Introduce the new total value:");
                            total = Console.ReadLine();
                        } while (Regex.IsMatch(total, Constants.DECIMAL_TOTAL_NUMBER_REGEX));
                    }
                    else
                    {
                        total = editSource.ElementAt(1);
                    }
                    sourceDb.EditSource(int.Parse(id), name, float.Parse(total));
                    break;
                }
                else if (id.Equals(Constants.BACK_KEY))
                {
                    break;
                }
            }
        }

        public void DeleteSource()
        {
            string id;
            List<string> deleteSource;
            Dictionary<int, List<string>> sources = ShowSources(currentPage * Constants.ELEMENTS_PER_PAGE, Constants.ELEMENTS_PER_PAGE);
            while (true)
            {
                // Showing the data.
                do
                {
                    // TODO pagination.
                    Console.WriteLine("ID | Name | Total");
                    foreach (KeyValuePair<int, List<string>> source in sources)
                    {
                        Console.WriteLine(source.Key + " | " + String.Join(" | ", source.Value.ToArray()));
                    }
                    Console.WriteLine("Please select an id or press " + Constants.BACK_KEY + "if you wish to back");
                    id = Console.ReadLine();
                } while (Regex.IsMatch(id, "^[0-9]+$") || id.Equals(Constants.BACK_KEY));
                if (sources.ContainsKey(int.Parse(id)))
                {
                    // ID is correct.
                    string answer;
                    sources.TryGetValue(int.Parse(id), out deleteSource);
                    do
                    {
                        Console.WriteLine("The choosed source: id => " + id + " | name => " + deleteSource.ElementAt(0) + " | total => " + deleteSource.ElementAt(1) + "\nDo you wish to delete it?(Y/y/N/n)");
                        answer = Console.ReadLine().ToLower();
                    } while (!answer.Equals("y") || !answer.Equals("n"));
                    if (answer.Equals("y"))
                    {
                        // Deleting source.
                        sourceDb.DeleteSource(int.Parse(id));
                        break;
                    }
                }
                else if (id.Equals(Constants.BACK_KEY))
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
