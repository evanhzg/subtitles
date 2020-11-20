using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SousTitresProj
{
    public class Srt
    {
        public DateTime defaultTime = DateTime.Now;
        public DateTime curTimecode = new DateTime();
        public DateTime subtitleStart = new DateTime();
        public DateTime subtitleEnd = new DateTime();
        public DateTime startSub = new DateTime();
        public DateTime endSub = new DateTime();
        public TimeSpan subLength = new TimeSpan();
        public TimeSpan curTime = new TimeSpan();
        public TimeSpan nextSub = new TimeSpan();
        public TimeSpan beforeNext = new TimeSpan();
        public int premierSoustitre = 1;
        public int compDatesStart = 0;
        public int compDatesEnd = 0;
        public int endOfSub = 0;
        public int shown = 0;
        public string format = "HH:mm:ss,fff";

        public async Task Stream()
        {
            // Définition du dossier Desktop et du chemin des fichiers de sous-titres
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string oldFilePath = mydocpath + @"\Evan_Hoizey_SubsProj\old_subs.srt";
            string newFilePath = mydocpath + @"\Evan_Hoizey_SubsProj\new_subs.srt";

            //StreamReader, lit dans le fichier choisi (ici oldFilePath)
            using (StreamReader sr = new StreamReader(oldFilePath))

            // StreamWriter permet d'écrire dans un nouveau fichier (ici newFilePath) les données souhaitées
            using (StreamWriter sw = new StreamWriter(newFilePath))
            {
                string subtitle = "";
                string l = "";
                List<string> lines = new List<string>();
                while ((l = sr.ReadLine()) != null)
                {
                    if (l.Contains(" --> "))
                    {
                        //Divise les lignes de timecode en deux variables datetime distinctes
                        string[] subtitleSplit = l.Split(new[] { " --> " }, StringSplitOptions.None);
                        string subtitleStartStr = subtitleSplit[0];
                        subtitleStartStr = subtitleStartStr.Replace(",", ".");

                        string subtitleEndStr = subtitleSplit[1];
                        subtitleEndStr = subtitleEndStr.Replace(",", ".");

                        subtitleStart = DateTime.Parse(subtitleStartStr);
                        subtitleEnd = DateTime.Parse(subtitleEndStr);
                        subLength = subtitleEnd - subtitleStart;
                        sw.WriteLine(subtitleStartStr + " - " + subtitleEndStr);
                        if (premierSoustitre == 1)
                        {
                            subLength = subtitleStart - defaultTime;
                        }
                        continue;
                    }
                    // Tant que la ligne n'est pas vide (soit la ligne qui suit les sous-titres),
                    // la variable subtitle la stocke et l'affiche dans la console et dans le newFile,
                    // ainsi que les timecode de début et de fin.
                    else if (l.Length >= 4)
                    {
                        if (premierSousTitre == 1)
                        {
                            System.Threading.Thread.Sleep(subLength);
                            premierSoustitre = 0;

                            subLength = subtitleEnd - subtitleStart;
                        }
                        sw.WriteLine(l);
                        Console.WriteLine(l);
                        System.Threading.Thread.Sleep(subLength);
                        Console.Clear();
                    }
                    else
                    {
                        continue;
                    }
                }
                endOfSub = 1;
            }
        }

        public async Task Timer()
        {
            curTime = curTimecode - defaultTime;
            string defaultTimeStr = defaultTime.ToString("HH:mm:ss.fff");
            string curTimeStr = curTimecode.ToString("HH:mm:ss.fff");
            while (endOfSub != 1)
            {
                await Task.Delay(1);
                curTimecode = DateTime.Now;
                curTimeStr = curTimecode.ToString("HH:mm:ss.fff");
                compDatesStart = curTime.CompareTo(subtitleStart);
                compDatesEnd = curTime.CompareTo(subtitleEnd);
            }
        }
    }

    //public class Pause
    //{
    //    public static async Task Stop()
    //    {
    //        ConsoleKeyInfo kp;
    //        kp = Console.ReadKey();
    //        bool pause = false;
    //        while (pause == true)
    //        { 
    //            if (kp.Key = ConsoleKey.Spacebar)
    //            {
    //                pause = true;
    //            }
    //        }
    //    }
    //}
}