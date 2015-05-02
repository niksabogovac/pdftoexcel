using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drugi
{
    class ReadFromFile
    {
        public static string[] ParseFile(String fileName)
        { 
            // Read the file as one string. 
            string text = System.IO.File.ReadAllText(fileName);
 
            // Read each line of the file into a string array. Each element 
            // of the array is one line of the file. 
            string[] lines = System.IO.File.ReadAllLines(fileName);
            
            //return array of strings :) 
            return lines;
        }

        //internal static void ParseFile(string p)
        //{
        //    Drugi.ReadFromFile(p);
        //    throw new NotImplementedException();
        //}
    }
}
