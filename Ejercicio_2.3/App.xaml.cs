using Ejercicio_2ultimoparcial;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ejercicio_2._3
{
    public partial class App : Application
    {
        public static string DBPath;

        public App()
        {
            InitializeComponent();
            IFileHelper fileHelper = DependencyService.Get<IFileHelper>();
            if (fileHelper != null)
            {
                DBPath = fileHelper.GetLocalFilePath("database.db");
                using (SQLiteConnection connection = new SQLiteConnection(DBPath))
                {
                    connection.CreateTable<Alumnos>();
                }
            }

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
