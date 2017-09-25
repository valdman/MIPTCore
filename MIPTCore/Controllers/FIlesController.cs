using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileManagment;
using Journalist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeMap = MimeTypeMap.List.MimeTypeMap;
using MIPTCore.Models;

namespace MIPTCore.Controllers
{
    public class FilesController : Controller
    {
        private readonly IFileManager _fileManager;

        public FilesController(IFileManager fileManager)
        {
            Require.NotNull(fileManager, nameof(fileManager));

            _fileManager = fileManager;
        }

        [HttpGet("file/{filename}")]
        public IActionResult GetFile(string fileName)
        {
            var fileMimeType = MimeMap.GetMimeType(new FileInfo(fileName).Extension).FirstOrDefault();;
            return GetAnyFile(() => _fileManager.GetFile(fileName), fileMimeType);
        }

        [HttpGet("image/{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var extension = new FileInfo(imageName).Extension;
            var imageMimeType = MimeMap.GetMimeType(extension).FirstOrDefault();
            return GetAnyFile(() => _fileManager.GetImage(imageName), imageMimeType);
        }

        [HttpPost("file")]
        [Authorize("Admin")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
			if (file.Length <= 0)
			{
				return BadRequest();
			}

			var createdFileName = await _fileManager.UploadFileAsync(file);
            return Ok(createdFileName);
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
			if (file.Length <= 0)
			{
                return BadRequest();
			}

            var createdImage = await _fileManager.UploadImageAsync(file);
            var imageModel = new ImageModel{
                  Original = createdImage.Original,
                  Small = createdImage.Small
                };
            return Ok(imageModel);
        }

        private IActionResult GetAnyFile(Func<Stream> getStream, string mimeType)
        {
            Stream stream;
            try
            {
                stream = getStream();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }

            
            var response = new FileStreamResult(stream, mimeType);
            return response;
        }
    }
}
