namespace Course_Overview.Helper
{
    public class UploadFile
    {
        static readonly string baseFolder = "Upload";

        public static async Task<string> SaveImage(string subFolder, IFormFile formFile)
        {
            string imageName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, subFolder);

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var exactPath = Path.Combine(imagePath, imageName);
            using (var fileStream = new FileStream(exactPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return Path.Combine(baseFolder, subFolder, imageName);
        }

        public static bool DeleteImage( string imagePath)
        {
         //   var imagePath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, imageName);

            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                    return true;
                }
                catch (Exception ex)
                {
                    // Log exception or handle it as needed
                    Console.WriteLine("An error occurred while deleting the image: " + ex.Message);
                    return false;
                }
            }
            else
            {
                // File does not exist
                return false;
            }
        }
    }
}
