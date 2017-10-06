using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Common;

namespace FileManagment
{
    public class FileManager : IFileManager
    {
        public Stream GetFile(string fileName)
        {
            return GetAnyFile(_fileStorageSettings.FileStorageFolder, fileName);
        }

        public Stream GetImage(string imageName)
        {
            return GetAnyFile(_fileStorageSettings.ImageStorageFolder, imageName);
        }

        public async Task<string> UploadFileAsync(IFormFile content)
        {
            var fileInfo = await UploadAnyFileAsync(content,
                                                        _fileStorageSettings.AllowedFileExtensions,
                                                        _fileStorageSettings.FileStorageFolder);

            return fileInfo.Name;
        }

        public async Task<Image> UploadImageAsync(IFormFile content)
        {
            var bigImageInfo = await UploadAnyFileAsync(content,
                                                        _fileStorageSettings.AllowedImageExtensions,
                                                        _fileStorageSettings.ImageStorageFolder);

            var smallImageInfo = _imageResizer.ResizeImageByLengthOfLongestSide(bigImageInfo);

            return new Image("image/" + bigImageInfo.Name, "image/" + smallImageInfo.Name);
        }

		private Stream GetAnyFile(string folderPath, string fileName)
		{
			var fullPath = Path.Combine(folderPath, fileName);
			var exists = File.Exists(fullPath);
			if (!exists)
			{
				throw new FileNotFoundException();
			}

			return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
		}

		private async Task<FileInfo> UploadAnyFileAsync(
            IFormFile content,
			string[] allowedExtensions,
			string folderPath)
		{
            var newName = SaltFileNameWithCurrentDate(Path.GetRandomFileName());
			var extension = GetFileExtension(content.FileName);
            var fullFileName = Path.Combine(folderPath, newName) + $".{extension}";

            using (var fileStream = new FileStream(fullFileName,FileMode.CreateNew, FileAccess.Write))	
            {
                await content.CopyToAsync(fileStream);
			}

			if (!allowedExtensions.Contains(extension.ToLower()))
			{
				await Task.Factory.StartNew(() => File.Delete(fullFileName));
                throw new InvalidDataException($"Extension {extension} is not allowed");
			}

			return new FileInfo(fullFileName);
		}

		private string GetFileExtension(string fileName)
		{
			return Path.GetExtension(fileName).TrimStart('.');
		}

		private void CreateFoldersIfNeeded()
		{
			if (!Directory.Exists(_fileStorageSettings.FileStorageFolder))
			{
				Directory.CreateDirectory(_fileStorageSettings.FileStorageFolder);
			}

			if (!Directory.Exists(_fileStorageSettings.ImageStorageFolder))
			{
				Directory.CreateDirectory(_fileStorageSettings.ImageStorageFolder);
			}
		}

		private string GenerateRandomFileName(string fileName)
		{
			var randomFileName = Path.GetRandomFileName();
			return Path.ChangeExtension(randomFileName, GetFileExtension(fileName));
		}

		private string SaltFileNameWithCurrentDate(string fileName)
		{
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			var newFileName = fileNameWithoutExtension
			    + "_"
				+ DateTimeOffset.Now.ToString("s").Replace(":", string.Empty)
				+ Path.GetExtension(fileName);
			return newFileName;
		}

		private void RenameFile(string originalFullName, string newFileFullName)
		{
			File.Move(originalFullName, newFileFullName);
		}

		private readonly FileStorageSettings _fileStorageSettings;
		private readonly IImageResizer _imageResizer;

        public FileManager(IOptions<FileStorageSettings> fileStorageSettings, IImageResizer imageResizer)
        {
            _fileStorageSettings = fileStorageSettings.Value;
            _imageResizer = imageResizer;

            CreateFoldersIfNeeded();
        }
    }
}
