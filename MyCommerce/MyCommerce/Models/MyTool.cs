using System.Text;

namespace MyCommerce.Models
{
    public class MyTool
    {
        public static string GetRandom(int length = 5)
        {
            var pattern = @"1234567890qazwsxedcrfvtgbyhn@#$%";
            var rd = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(pattern[rd.Next(0, pattern.Length)]);

            return sb.ToString();
        }
        public static string UploadFileToFolder(IFormFile file, string folderName)
        {
            try
            {
                var fileName = $"{DateTime.Now.Ticks}_{file.FileName}";
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folderName, fileName);
                using (var myFile = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(myFile);
                }
                return fileName;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
