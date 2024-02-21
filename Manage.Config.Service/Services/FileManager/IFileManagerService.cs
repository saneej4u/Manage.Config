using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Config.Service.Services.FileManager
{
    public interface IFileManagerService
    {
        Task<List<List<string>>> ReadFileAndGroupByEmptyLineAsync(string filePath);
    }
}
