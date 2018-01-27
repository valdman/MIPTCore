using Common.Abstractions;

namespace Common.Entities
{
    public class Image : AbstractIdentifyable
    {
        public Image(){}
        
        public Image(string bigPhotoUri, string smallPhotoUri)
        {
            Original = bigPhotoUri;
            Small = smallPhotoUri;
        }

        public string Original { get; set; }
        public string Small { get; set; }
    }
}