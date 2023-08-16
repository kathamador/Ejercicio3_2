using Ejercicio_2ultimoparcial;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ejercicio_2._3
{
   
    public partial class MainPage : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile photo;
      

        public MainPage()
        {
            InitializeComponent();
        

        }

        private async void btntomarfoto_Clicked(object sender, EventArgs e)
        {
            photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "imagen",
                Name = "Foto.jpg",
                SaveToAlbum = true
            });

            if (photo != null)
            {
                foto.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }


        }

        private async void btnverlista_Clicked(object sender, EventArgs e)
        {




            await Navigation.PushAsync(new Lista());


        }

      
    }

  
}
