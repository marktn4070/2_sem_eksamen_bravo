using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2_sem_eksamen_bravo
{
    static class MessageEmulator
    {
        public static string path = Directory.GetCurrentDirectory();

        public static void EmulateSendSms(string message)
        {
            //Husk at lave en mappe for de filer der bliver gemt
            using (StreamWriter w = new StreamWriter(path.Remove(path.IndexOf(@"\bin")) + @"\log.html"))
            {

            }
        }
        public static void EmulateSendEmail(string message)
        {
            //Husk at lave en mappe for de filer der bliver gemt
            using (StreamWriter w = new StreamWriter(path.Remove(path.IndexOf(@"\bin")) + @"\log.txt"))
            {

            }
        }
    }
}
