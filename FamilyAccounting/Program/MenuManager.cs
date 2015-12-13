using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Program
{
    class MenuManager
    {
        private static bool welcomeMessage;
        private StringBuilder menu;
        public MenuManager()
        {
            menu = new StringBuilder();
            welcomeMessage = true;
        }

        public string MainMenu()
        {
            menu.Clear();
            if (welcomeMessage)
            {
                menu.Append("Welcome to Family Accounting.\n");
                welcomeMessage = false;
            }
            return menu.Append("Please select an option:\n1. Source Menu\n2. Movement Menu\n3. Category Menu\n4. Exit").ToString();
        }

        public string SourceMenu()
        {
            menu.Clear();
            return menu.Append("Please select an option:\n1. Create a new money source\n2. Edit an existing money source\n3. Delete an existig money source\n4. View all money sources\n5. Back to main menu\n6. Exit program").ToString();
        }

        public string MovementMenu()
        {
            menu.Clear();
            return menu.Append("Please select an option:\n1. Create a new movement\n2. Edit an existing movement\n3. Delete an existig movement\n4. View all movements\n5. Back to main menu\n6. Exit program").ToString();
        }

        public string CategoryMenu()
        {
            menu.Clear();
            return menu.Append("Please select an option:\n1. Create a new category\n2. Edit an existing category\n3. Delete an existig category\n4. View all categories\n5. Back to main menu\n6. Exit program").ToString();
        }
    }
}
