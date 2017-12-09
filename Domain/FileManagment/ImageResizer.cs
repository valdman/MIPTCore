using System.IO;
using SixLabors.ImageSharp;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

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

            image.Mutate(i =>
                i.Resize(new ResizeOptions
                {
                    Size = newSize,
                    Mode = ResizeMode.Pad
                }));

            var extension = Path.GetExtension(imageToResizeInfo.ToString());
            var newFileName = Path.GetRandomFileName();
            var fullPath = Path.Combine(_fileStorageSettings.ImageStorageFolder, newFileName) + $".{extension}";

            image.Save(fullPath);

            return new FileInfo(fullPath);
        }

        private readonly FileStorageSettings _fileStorageSettings;

        public ImageResizer(IOptions<FileStorageSettings> fileStorageSettings)
        {
            _fileStorageSettings = fileStorageSettings.Value;
        }
    }
}
