﻿using _0_Framework.Application;
using Directory = System.IO.Directory;

namespace ServiceHost.Extension
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string Upload(IFormFile file ,string path)
        {
            if (file == null)
            {
                return "";
            }
            

            var directoryPath = $"{_webHostEnvironment.WebRootPath}//UploadedFiles//{path}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
            var filePath = $"{directoryPath}//{fileName}";
            
            using var output = File.Create(filePath);
            file.CopyTo(output);

            return $"{path}/{fileName}";
        }

        public string UploadProductPicture(IFormFile file, string path)
        {
            if (file == null)
            {
                return "";
            }


            var directoryPath = $"{_webHostEnvironment.WebRootPath}//UploadedFiles//{path}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{file.FileName}";
            var filePath = $"{directoryPath}//{fileName}";

            using var output = File.Create(filePath);
            file.CopyTo(output);

            return $"{path}/{fileName}";
        }
    }
}
