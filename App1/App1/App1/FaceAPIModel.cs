using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class FaceAPIModel
    {
        public string FaceId { get; set; }
        public FaceRectangle FaceRectangles { get; set; }
        public FaceAttribute FaceAttributes { get; set; }

    }
    public class FaceRectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class FaceAttribute
    {
        public string Gender { get; set; }
        public double Age { get; set; }
    }

}
