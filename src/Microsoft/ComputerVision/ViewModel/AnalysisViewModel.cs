using System.Collections.Generic;

namespace ComputerVision.ViewModel
{
    public class AnalysisViewModel
    {
        public string UrlImage { get; set; }
        public List<Category> Categories { get; set; }
        public Adult Adult { get; set; }
        public List<Tag> Tags { get; set; }
        public Description Description { get; set; }
        public string RequestId { get; set; }
        public Metadata Metadata { get; set; }
        public List<Face> Faces { get; set; }
        public Color Color { get; set; }
        public ImageType ImageType { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public Detail Detail { get; set; }
    }

    public class Detail
    {
        public List<Celebrity> Celebrities { get; set; }
        public List<Landmark> Landmarks { get; set; }
    }

    public class Celebrity
    {
        public string Name { get; set; }
        public Position FaceRectangle { get; set; }
        public double Confidence { get; set; }
    }

    public class Position
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Landmark
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

    public class Adult
    {
        public bool IsAdultContent { get; set; }
        public bool IsRacyContent { get; set; }
        public double AdultScore { get; set; }
        public double RacyScore { get; set; }
    }

    public class Tag
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

    public class Caption
    {
        public string Text { get; set; }
        public double Confidence { get; set; }
    }

    public class Description
    {
        public List<string> Tags { get; set; }
        public List<Caption> Captions { get; set; }
    }

    public class Metadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }
    }

    public class Face
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public Position FaceRectangle { get; set; }
    }

    public class Color
    {
        public string DominantColorForeground { get; set; }
        public string DominantColorBackground { get; set; }
        public List<string> DominantColors { get; set; }
        public string AccentColor { get; set; }
        public bool IsBWImg { get; set; }
    }

    public class ImageType
    {
        public int ClipArtType { get; set; }
        public int LineDrawingType { get; set; }
    }
}