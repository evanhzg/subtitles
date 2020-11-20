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
        // définition des variables globales utilisées dans plusieurs fonctions/Tasks.
        public DateTime defaultTime = DateTime.Now;
        public DateTime curTimecode = new DateTime();
        public DateTime subtitleEnd = new DateTime();
        public DateTime subtitleStart = new DateTime();
        public TimeSpan subLength = new TimeSpan();
        public TimeSpan between = new TimeSpan();
        public TimeSpan curTime = new TimeSpan();

        public string subtitleEndStr = "00:00:00.000"; // première valeur déterminant l'attente avant le premier sous-titre
        public int endOfSub = 0; // si affiché, fin du timer.
        public string format = "HH:mm:ss,fff"; // détermine le format d'affichage du timecode dans le fichier srt.

        // Tâche Stream qui affiche et écrit les sous-titres dans la console et le fichier new_subs.srt
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
                string l = "";
                List<string> lines = new List<string>();
                subtitleEnd = DateTime.Parse(subtitleEndStr);
                while ((l = sr.ReadLine()) != null)
                {
                    if (l.Contains(" --> "))
                    {
                        // Divise les lignes de timecode en deux variables datetime distinctes
                        // et stocke l'attente entre et pendant les sous-titres dans des TimeSpan
                        string[] subtitleSplit = l.Split(new[] { " --> " }, StringSplitOptions.None);
                        string subtitleStartStr = subtitleSplit[0];
                        subtitleStartStr = subtitleStartStr.Replace(",", ".");

                        subtitleEndStr = subtitleSplit[1];
                        subtitleEndStr = subtitleEndStr.Replace(",", ".");

                        subtitleStart = DateTime.Parse(subtitleStartStr);
                        between = subtitleStart - subtitleEnd;
                        subtitleEnd = DateTime.Parse(subtitleEndStr);

                        subLength = subtitleEnd - subtitleStart;

                        sw.WriteLine("");
                        sw.WriteLine(subtitleStartStr + " - " + subtitleEndStr);
                        continue;
                    }
                    // Si la variable contient plus de 4 caractères (les sous-titres),
                    // l'affiche et l'écrit dans le fichier
                    else if (l.Length >= 4)
                    {
                        System.Threading.Thread.Sleep(between);
                        sw.WriteLine(l);
                        Console.WriteLine(l);
                        System.Threading.Thread.Sleep(subLength);
                        Console.Clear();
                        continue;
                    }
                    else
                        continue;
                }
                // détermine la fin du timer (inutilisé  mais fonctionnel)
                endOfSub = 1;
            }
        }

        // Fonction/Task déterminant le temps actuel, n'est pas utilisée dans le programme
        // mais détermine un timer en continu qui peut déterminer l'avancée.
        public async Task Timer()
        {
            string defaultTimeStr = defaultTime.ToString(format);
            string curTimeStr = curTimecode.ToString(format);
            while (endOfSub != 1)
            {
                await Task.Delay(1);
                curTimecode = DateTime.Now;
                curTimeStr = curTimecode.ToString(format);
            }
        }
    }

    // fonction pause mise de côté car non fonctionnelle et j'ai préféré me concentrer sur le reste.

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