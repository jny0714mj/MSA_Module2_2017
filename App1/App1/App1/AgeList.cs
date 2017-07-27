using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace App1
{
	public class AgeList
	{
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public double Age { get; set; }

        [JsonProperty(PropertyName = "Latitude")]
        public string Gender { get; set; }
    }
}