using Ejercicio_2._3;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ejercicio_2ultimoparcial
{
    public class MainViewModel : BaseViewModel
    {

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public bool IsAlumnoSeleccionado
        {
            get
            {
                return EditingAlumno != null;
            }
        }

        private string _nombre;
        private string _apellido;
        private string _sexo;
        private string _direccion;
        private string _imageBase64;
        private string _foto;
        private string _id;

        private ICommand _insertCommand;
      
        private ICommand _insertCommand2;
        public event PropertyChangedEventHandler _PropertyChanged;
        private ObservableCollection<Alumnos> _people;
        public ObservableCollection<Alumnos> People
        {
            get { return _people; }
            set
            {
                _people = value;
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(People)));
            }
        }

        public ICommand LoadDataCommand => new Command(LoadData);

        private void LoadData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DBPath))
            {
                People = new ObservableCollection<Alumnos>(connection.Table<Alumnos>().ToList());
            }
            IsBusy = false;
        }

        public bool HasDataInAlumnosTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DBPath))
            {
                try
                {
                    int count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Alumnos");
                    return count > 0;
                }
                catch (Exception ex)
                {
                    // Manejar excepciones si ocurre algún error
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public ICommand DeleteAlumnoCommand => new Command<Alumnos>(DeleteAlumno);

        private void DeleteAlumno(Alumnos alumno)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DBPath))
            {
                connection.Delete(alumno);
                People.Remove(alumno); // Remover de la colección también
            }
        }// eliminar registro


        private Alumnos _editingAlumno;

        public Alumnos EditingAlumno
        {
            get => _editingAlumno;
            set
            {
                _editingAlumno = value;
                OnPropertyChanged(nameof(EditingAlumno));
            }
        }

        private void EditAlumno(Alumnos alumno)
        {
            EditingAlumno = new Alumnos
            {
                Id = alumno.Id,
                Nombre = alumno.Nombre,
                Apellido = alumno.Apellido,
                Direccion = alumno.Direccion,
                Sexo = alumno.Sexo
                // Copia las demás propiedades que necesitas editar
            };
        }

        public ICommand SaveEditedAlumnoCommand => new Command(SaveEditedAlumno);

        private async void SaveEditedAlumno()
        {
            if (EditingAlumno == null)
            {
                return;
            }
            // Aquí realizas la lógica para guardar los cambios en el alumno editado
            using (SQLiteConnection connection = new SQLiteConnection(App.DBPath))
            {
                connection.Update(EditingAlumno);
                People[People.IndexOf(People.FirstOrDefault(a => a.Id == EditingAlumno.Id))] = EditingAlumno;
            }
            EditingAlumno = null; // Reinicia el objeto de edición
        }


        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }
        public string Apellido
        {
            get => _apellido;
            set => SetProperty(ref _apellido, value);
        }
        public string Sexo
        {
            get => _sexo;
            set => SetProperty(ref _sexo, value);
        }
        public string Direccion
        {
            get => _direccion;
            set => SetProperty(ref _direccion, value);
        }

        public string ImageBase64
        {
            get => _imageBase64;
            set {
                SetProperty(ref _imageBase64, value);
                OnPropertyChanged(nameof(Image));
            }
        }

        public ImageSource Image
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(ImageBase64)) );
            }
        }



        public ICommand SelectImageCommand => new Command(SelectImage);
        public ICommand TakeImageCommand => new Command(TakePicture);

        public ICommand InsertCommand2 => _insertCommand2 ?? (_insertCommand2 = new Command(InsertData2));

        private string ConvertImageToBase64(Stream imageStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private async void TakePicture()
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "imagen",
                Name = "Foto.jpg",
                SaveToAlbum = true
            });

            if (photo != null)
            {
                ImageBase64 = ConvertImageToBase64(photo.GetStream());
            }
        }

        private async void SelectImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Picking a photo is not supported on this device", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.PickPhotoAsync();

            if (file != null)
            {
                ImageBase64 = ConvertImageToBase64(file.GetStream());
            }
        }


      

        private void InsertData2()
        {
            if (int.TryParse(Id, out int id) && !string.IsNullOrWhiteSpace(Nombre) && !string.IsNullOrWhiteSpace(Apellido) && !string.IsNullOrWhiteSpace(Sexo) && !string.IsNullOrWhiteSpace(Direccion) && !string.IsNullOrWhiteSpace(ImageBase64))
            {
                InsertPersonWithImage(id,ImageBase64, Nombre, Apellido, Sexo, Direccion);

                Id = string.Empty;
                ImageBase64 = string.Empty;
                Nombre = string.Empty;
                Apellido = string.Empty;
                Sexo = string.Empty;
                Direccion = string.Empty;

                Application.Current.MainPage.DisplayAlert("Success", "Datos Guardados con Exito", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "No Pudieron Guardar Datos", "OK");
            }
        }
        private void InsertPersonWithImage(int id ,string imageBase64, string nombre, string apellido, string sexo, string direccion)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.DBPath))
            {
                connection.CreateTable<Alumnos>(); 

                connection.Insert(new Alumnos
                {
                    Id = id,
                    Nombre = nombre,
                    Apellido = apellido,
                    Sexo = sexo,
                    Direccion = direccion,
                  
                    Foto = imageBase64
                });
            }

        }
}
}
