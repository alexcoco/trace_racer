using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.


using System.IO.IsolatedStorage;namespace GCA2
{
    class Storage
    {
        public static bool Save(GameSettings settings)
        {
            bool result = false;

            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = null;
            stream = store.OpenFile(GameSettings.Filename, System.IO.FileMode.Create);

            if (stream != null) {
                //stream.Write(System.
                //stream.Close();
            }

            return result;
        }
    }
}
