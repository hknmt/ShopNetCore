using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IFileMagager
    {
        Task<string> Upload(IFormFile files, string FolderName, int? height, int? width);
    }
}
