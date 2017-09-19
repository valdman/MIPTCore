using System.IO;
using ImageSharp;
using Microsoft.Extensions.Options;

namespace FileManagment
{
    public class ImageResizer : IImageResizer
    {
        public FileInfo ResizeImageByLengthOfLongestSide(FileInfo imageToResizeInfo)
        {
            var image = Image.Load(File.ReadAllBytes(imageToResizeInfo.FullName));
            var newSize = new Size
            {
                Width = image.Width > image.Height ? _fileStorageSettings.MaxImageSize : 0,
                Height = image.Width > image.Height ? 0 : _fileStorageSettings.MaxImageSize
            };

            image.Resize(new ImageSharp.Processing.ResizeOptions
            {
                Size = newSize,
                Mode = ImageSharp.Processing.ResizeMode.Pad
            });

            var extension = Path.GetExtension(imageToResizeInfo.ToString());
            var newFileName = Path.GetRandomFileName();
            var fullPath = Path.Combine(_fileStorageSettings.ImageStorageFolder, newFileName) + $".{extension}";

            using (var fileStream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
            {
                image.Save(fileStream);
            }

            return new FileInfo(fullPath);
        }

        private readonly FileStorageSettings _fileStorageSettings;

        public ImageResizer(IOptions<FileStorageSettings> fileStorageSettings)
        {
            _fileStorageSettings = fileStorageSettings.Value;
        }
    }
}
