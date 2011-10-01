using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace GCA2
{
    class GameSettings
    {
        public static override string Filename = "GameSettings.dat";
        bool UseMusic;
        bool UseSFX;

        public GameSettings()
        {
            UseMusic = true;
            UseSFX = true;
        }
    }
}