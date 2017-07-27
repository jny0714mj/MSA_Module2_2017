using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public class AzureManager
    {
            
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<AgeList> ageListTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://guessmyage.azurewebsites.net");
            this.ageListTable = this.client.GetTable<AgeList>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<AgeList>> FaceInfo()
        {
            return await this.ageListTable.ToListAsync();
        }


        public async Task InsertInfo(AgeList faceModel)
        {
            await this.ageListTable.InsertAsync(faceModel);
        }


    }
}
