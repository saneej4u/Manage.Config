namespace Manage.Config.Service.Services.FileManager
{
    public class FileManagerService : IFileManagerService
    {
        public async Task<List<List<string>>> ReadFileAndGroupByEmptyLineAsync(string filePath)
        {
            List<List<string>> groups = new List<List<string>>();
            List<string> currentGroup = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (currentGroup.Count > 0)
                        {
                            groups.Add(currentGroup);
                            currentGroup = new List<string>();
                        }
                    }
                    else if (!line.StartsWith(";"))
                    {
                        currentGroup.Add(line);
                    }
                }

                // Add the last group if it's not empty
                if (currentGroup.Count > 0)
                {
                    groups.Add(currentGroup);
                }
            }

            return groups;
        }
    }
}
