using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class FaceAPIModel
    {
        public FaceRectangle FaceRectangles { get; set; }
        public List<FaceAttributes> FaceAttribute { get; set; }

    }
    public class FaceRectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class FaceAttributes
    {
        public string Gender { get; set; }
        public double Age { get; set; }

    }
}
