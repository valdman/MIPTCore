using System.IO;
using System.Threading.Tasks;
using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace FileManagment
{
    public interface IFileManager
    {
		Stream GetFile(string fileName);

		Stream GetImage(string imageName);

        Task<string> UploadFileAsync(IFormFile content);

        Task<Image> UploadImageAsync(IFormFile content);
    }
}
