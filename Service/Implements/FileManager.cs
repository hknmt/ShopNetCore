using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;


namespace Service.Implements
{
    public class FileManager : IFileMagager
    {
        private readonly IHostingEnvironment _environment;

        public FileManager(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> Upload(IFormFile file, string FolderName, int? height, int? width)
        {
            try
            {
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    var fileUrl = "uploads/";

                    if (string.IsNullOrWhiteSpace(FolderName))
                    {
                        uploads = Path.Combine(uploads, "Temp");
                        fileUrl += "Temp/";
                    }
                    else
                    {
                        uploads = Path.Combine(uploads, FolderName);
                        fileUrl += FolderName + "/";
                    }

                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    if (file.Length > 0)
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        fileUrl += file.FileName;
                    }

                    return fileUrl;
            }
            catch
            {
                return null;
            }
        }
    }
}
