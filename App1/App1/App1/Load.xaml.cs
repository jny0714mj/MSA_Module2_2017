using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Load : ContentPage
	{
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public Load ()
		{
			InitializeComponent ();
		}
        async void LoadNow(object sender, System.EventArgs e)
        {
            List<AgeList> ageInfo = await AzureManager.AzureManagerInstance.FaceInfo();
            LoadList.ItemsSource = ageInfo;
        }


    }
}