using App1.Servicos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IMessage>().ShortAlert("Inicia Chamada");
            DependencyService.Get<ILogin>().SigninButton_Click();

        }
    }
}
