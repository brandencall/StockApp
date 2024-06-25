using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsertScript
{
    public static class UserInterface
    {
        public static int MenuOption()
        {
            Console.WriteLine("Please enter a value. \n" +
               "1. Insert Stocks into Stocks Table. \n" +
               "2. Bulk insert Financial data for stocks. \n" +
               "3. Bulk insert Market Cap data. \n" +
               "\n");

            bool success = false;
            int output;

            do
            {
                Console.Write("Enter value: ");
                string input = Console.ReadLine();

                int.TryParse(input, out output);

                if (output == 1 || output == 2 || output == 3)
                {
                    success = true;
                }

            } while (success == false);

            return output;
           
        }
    }
}
