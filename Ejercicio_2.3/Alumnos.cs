using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Ejercicio_2ultimoparcial
{
    public class Alumnos
    {
        [PrimaryKey]
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public string Sexo { set; get; }
        public string Direccion { set; get; }
        public string Foto { set; get; }
        public ImageSource Image
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(Foto)));
            }
        }



    }
}
