using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;

namespace TraceRacer
{
    public class Highscore
    {
        private static string filename = "highscore.dat";

        public static void Save(Score score)
        {
            Save(((score.Points / 100) * 10).ToString());
        }

        public static void Save(long score)
        {
            Save(score.ToString());
        }

        public static void Save(string score)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Create, store))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(score);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
        }

        public static long Load()
        {
            long score = 0L;

            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(filename))
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, store))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            try
                            {
                                score = Int64.Parse(reader.ReadToEnd(), System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (FormatException)
                            {
                                // Score remains 0L
                            }
                            finally
                            {
                                if (reader != null)
                                    reader.Close();
                            }
                        }
                    }
                }
            }

            return score;
        }
    }
}
