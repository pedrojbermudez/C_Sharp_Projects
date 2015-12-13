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

        /// <summary>
        /// Used to print the main menu. Keywords:
        ///     1. Source Menu
        ///     2. Movement Menu
        ///     3. Category Menu
        ///     4. Exit
        /// </summary>
        /// <returns>Full main menu</returns>
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

        /// <summary>
        /// Used to print the source menu. Keywords:
        ///     1. Create new source
        ///     2. Edit a source
        ///     3. Delete a source
        ///     4. View all sources
        ///     5. Back main menu
        ///     6. Program exit
        /// </summary>
        /// <returns>Full source menu</returns>
        public string SourceMenu()
        {
            menu.Clear();
            return menu.Append("Please select an option:\n1. Create a new money source\n2. Edit an existing money source\n3. Delete an existig money source\n4. View all money sources\n5. Back to main menu\n6. Program exit").ToString();
        }

        /// <summary>
        /// Used to print the movement menu. Keywords:
        ///     1. Create new movement
        ///     2. Edit a movement
        ///     3. Delete a movement
        ///     4. View all movements
        ///     5. Back main menu
        ///     6. Program exit
        /// </summary>
        /// <returns>Full movement menu</returns>
        public string MovementMenu()
        {
            menu.Clear();
            return menu.Append("Please select an option:\n1. Create a new movement\n2. Edit an existing movement\n3. Delete an existig movement\n4. View all movements\n5. Back to main menu\n6. Program exit").ToString();
        }

        /// <summary>
        /// Used to print the category menu. Keywords:
        ///     1. Create new category
        ///     2. Edit a category
        ///     3. Delete a category
        ///     4. View all categories
        ///     5. Back main menu
        ///     6. Program exit
        /// </summary>
        /// <returns>Full category menu</returns>
        public string CategoryMenu()
        {
            menu.Clear();
            return menu.Append("Please select an option:\n1. Create a new category\n2. Edit an existing category\n3. Delete an existig category\n4. View all categories\n5. Back to main menu\n6. Program exit").ToString();
        }
    }
}
