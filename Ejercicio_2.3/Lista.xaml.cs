using Ejercicio_2._3;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ejercicio_2ultimoparcial
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lista : ContentPage
    {

        public Lista()
        {
            BindingContext = new  MainViewModel();
            InitializeComponent();
        }

     
    }
}