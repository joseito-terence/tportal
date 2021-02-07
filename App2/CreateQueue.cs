using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    class CreateQueue
    {
        // appDataPath = path for appdata folder.
        public string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // creates a text file with a list of all the video uploaded.
        public void createQueue() 
        {
            using (System.IO.StreamWriter sw = File.CreateText(appDataPath + "/src/queue.txt"))
            {
                string[] queue = Directory.GetFiles(appDataPath + "/videos/");

                foreach (string fileName in queue)
                    sw.WriteLine(fileName.Substring(appDataPath.Length + 8));
            }
        }    
    }
}
