using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        async void OpenCamera(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            YourAge.Text = "hmmm";
            await MakePredictionRequest(file);

        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
  
            var client = new HttpClient();

            string url = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
            string uri = url + "?" + requestParameters;

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "acd46cd21e25495db5f5e8340550247b");

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);
            
            using (var content = new ByteArrayContent(byteData))
            {
                
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if(responseString != "[]")
                    {
                        var infos = JsonConvert.DeserializeObject<FaceAPIModel[]>(responseString);
                        var moreinfos = infos[0].FaceAttributes.Age;
                        var ami = infos[0].FaceAttributes.Gender;
                        //string ggen = infos[0].gender;

                        
                        YourAge.Text = "your age is:" + moreinfos + "and i am : " + ami ;
                    }
                    else
                    {
                        YourAge.Text = "face??";
                    }
                    

                }
                else
                {
                    YourAge.Text = "NOOO";
                }



                //Get rid of file once wse have finished using it
                file.Dispose();
            }



        }
    }
}