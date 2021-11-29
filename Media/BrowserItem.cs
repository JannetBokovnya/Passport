using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Media.Resources;

namespace Media
{
    public class BrowserItem
    {
        /// <summary>
        /// Gets the name of the Photograph.
        /// </summary>
        public string Name { get; private set; }
        public string Comment { get; private set; }
        public double Key { get; private set;}
        public string KeyName { get; private set; }
        public int Number{get; private set; }
        public int Count { get; private set; }
        public string TypeMedia { get; private set; }
        public string NameFile { get; private set; }
        public string TypeFile { get; private set; }
        public string CommentT { get; private set; }

        /// <summary>
        /// Gets an Image control containing the Photograph.
        /// </summary>
        public BitmapImage Image { get; private set; } // Image -> BitmapImage

        public BrowserItem(string name, BitmapImage image, string comment, double key, string keyName, int number, int count, string typeMedia) // Image -> BitmapImage
        {
            Name = name;
            Image = image;
            Comment = comment;
            Key = key;
            KeyName = keyName;
            Number = number;
            Count = count;
            TypeMedia = typeMedia;
            NameFile = ProjectResources.FileUploadNameFile;
            TypeFile = ProjectResources.FileUploadTypeFile;
            CommentT = ProjectResources.mediaComment;
        }
    }
}
