using Ejercicio_2._3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ejercicio_2ultimoparcial
{
    public class FileHelperAndroid : IFileHelper
    {

        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }



    }
}
