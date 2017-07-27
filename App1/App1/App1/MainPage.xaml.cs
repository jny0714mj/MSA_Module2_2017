using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
            
            YourAge.Text = "Let me guess...";
            await LetsGuess(file);

        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task LetsGuess(MediaFile file)
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
                        var myage = infos[0].FaceAttributes.Age;
                        var mygender = infos[0].FaceAttributes.Gender;
                        
                        YourAge.Text = "your age is:" + myage + "and i am : " + mygender;

                        AgeList lists = new AgeList()
                        {
                            Age = myage,
                            Gender = mygender
                        };

                        await AzureManager.AzureManagerInstance.InsertInfo(lists);
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