using ImageMagick;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace API_Gateway.Common
{
    public class ImageUpload
    {
        private readonly IConfiguration _configuration;
        private readonly bool saveInS3 = false;
        public ImageUpload(IConfiguration configuration)
        {
            _configuration = configuration;
            saveInS3 = Convert.ToBoolean(_configuration.GetSection("AWS").GetSection("save_images_in_s3").Value);
        }

        public string UploadImageAndDocs(string fileName, string folderPath, IFormFile formFile)
        {
            var file = formFile;
            var _fileName = fileName;

            if (file != null)
            {
                _fileName = _fileName + Path.GetExtension(file.FileName);
                if (saveInS3 != true)
                {
                    try
                    {
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        //var folderName = Path.Combine("Resources", "Brandcertificate");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);

                        var fullPath = Path.Combine(pathToSave, _fileName);
                        var dbPath = Path.Combine(folderPath, _fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            if (Path.GetExtension(file.FileName).ToLower() == ".png" || Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                            {
                                // Load the image using ImageMagick
                                using (var image = new MagickImage(file.OpenReadStream()))
                                {
                                    // Set compression options for reducing file size to KB
                                    image.Quality = 80; // Adjust the quality value as needed

                                    // Save the compressed image
                                    image.Write(stream);
                                }
                            }
                            else
                            {
                                file.CopyTo(stream);
                            }
                        }
                        //return fileName;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                _fileName = null;
            }
            return _fileName;
        }

        public string UpdateUploadImageAndDocs(string OldfileName, string fileName, string folderPath, IFormFile formFile)
        {
            var file = formFile;
            var _fileName = fileName;

            if (file != null)
            {
                _fileName = _fileName + Path.GetExtension(file.FileName);
                if (saveInS3 != true)
                {
                    try
                    {
                        //if (!System.IO.Directory.Exists(folderPath))
                        //{
                        //    System.IO.Directory.CreateDirectory(folderPath);
                        //}



                        var Isdelete = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
                        if (!string.IsNullOrEmpty(OldfileName))
                        {
                            var _fullPath = Path.Combine(Isdelete, OldfileName);
                            if (System.IO.File.Exists(_fullPath))
                            {
                                System.IO.File.Delete(_fullPath);
                            }
                        }

                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        //var folderName = Path.Combine("Resources", "Brandcertificate");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);

                        var fullPath = Path.Combine(pathToSave, _fileName);
                        var dbPath = Path.Combine(folderPath, _fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            if (Path.GetExtension(file.FileName).ToLower() == ".png" || Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                            {
                                // Load the image using ImageMagick
                                using (var image = new MagickImage(file.OpenReadStream()))
                                {
                                    // Set compression options for reducing file size to KB
                                    image.Quality = 80; // Adjust the quality value as needed

                                    // Save the compressed image
                                    image.Write(stream);
                                }
                            }
                            else
                            {
                                file.CopyTo(stream);
                            }
                        }
                        //return fileName;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                if (OldfileName != null)
                {
                    _fileName = OldfileName;
                }
            }
            return _fileName;
        }

        public bool UploadDocs(string name, string file)
        {
            try
            {
                var folderName = string.Empty;

                switch (name)
                {
                    case "PanCardDoc":
                        folderName = Path.Combine("Resources", "Kyc", "PanCardDoc");
                        break;
                    case "AadharCardBackDoc":
                        folderName = Path.Combine("Resources", "Kyc", "AadharCardBackDoc");
                        break;
                    case "AadharCardFrontDoc":
                        folderName = Path.Combine("Resources", "Kyc", "AadharCardFrontDoc");
                        break;
                    case "MSMEDoc":
                        folderName = Path.Combine("Resources", "Kyc", "MSMEDoc");
                        break;
                    case "CancelCheque":
                        folderName = Path.Combine("Resources", "Kyc", "CancelCheque");
                        break;
                    case "Logo":
                        folderName = Path.Combine("Resources", "Kyc", "Logo");
                        break;
                    case "DigitalSign":
                        folderName = Path.Combine("Resources", "Kyc", "DigitalSign");
                        break;
                    case "Images":
                        folderName = Path.Combine("Resources", "Images");
                        break;
                    case "GSTInfo":
                        folderName = Path.Combine("Resources", "Kyc", "GSTInfo");
                        break;
                    case "ProductImage":
                        folderName = Path.Combine("Resources", "ProductImage");
                        break;
                    default:
                        return false;
                }

                var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Resources", "Temp"));
                var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (saveInS3 != true)
                {
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }

                    var sourceFilePath = Path.Combine(sourcePath, file);
                    var destinationFilePath = Path.Combine(destinationPath, file);
                    File.Delete(destinationFilePath);
                    File.Move(sourceFilePath, destinationFilePath);

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TempUploadMethod(string sellerId, string docName, IFormFile Image, string? sellerLeagleName)
        {
            try
            {
                var file = Image;
                if (!System.IO.Directory.Exists("Resources" + "\\Temp"))
                {
                    System.IO.Directory.CreateDirectory("Resources" + "\\Temp");
                }

                var folderName = Path.Combine("Resources", "Temp");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file != null)
                {
                    var tempName = sellerId + "_" + docName;

                    if (docName.ToLower() == "logo")
                    {
                        tempName = sellerId + "_" + sellerLeagleName + "_" + docName;
                    }
                    // Here we check the files is exist or not. If exist delete the file.
                    var existingFile = Directory.GetFiles(pathToSave, tempName + ".*").FirstOrDefault();
                    if (existingFile != null)
                    {
                        System.IO.File.Delete(existingFile);
                    }

                    var fileName = tempName + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TempProductUploadMethod(IFormFile Image, string productName, int sequence)
        {
            try
            {
                var file = Image;
                if (!System.IO.Directory.Exists("Resources" + "\\Temp"))
                {
                    System.IO.Directory.CreateDirectory("Resources" + "\\Temp");
                }

                var folderName = Path.Combine("Resources", "Temp");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file != null)
                {
                    string a = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                    var tempName = productName.Replace(" ", "-") + "_" + sequence + "_" + a;

                    // Here we check the files is exist or not. If exist delete the file.
                    var existingFile = Directory.GetFiles(pathToSave, tempName + ".*").FirstOrDefault();
                    if (existingFile != null)
                    {
                        System.IO.File.Delete(existingFile);
                    }

                    var fileName = tempName + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> TempProductUploadMethodFromUrl(string url, string Name)
        {
            try
            {
                if (!System.IO.Directory.Exists("Resources" + "\\Temp"))
                {
                    System.IO.Directory.CreateDirectory("Resources" + "\\Temp");
                }

                // Define a regular expression pattern to match special characters
                string pattern = "[^a-zA-Z0-9]+";

                // Replace special characters with a hyphen
                Name = Regex.Replace(Name, pattern, "-");

                Name = Name.Replace(" ", "-");
                var folderName = Path.Combine("Resources", "Temp");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!string.IsNullOrEmpty(url))
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var tempName = Name + "_" + a;

                    // Here we check the files is exist or not. If exist delete the file.
                    var existingFile = Directory.GetFiles(pathToSave, tempName + ".*").FirstOrDefault();
                    if (existingFile != null)
                    {
                        System.IO.File.Delete(existingFile);
                    }

                    var fileName = tempName + Path.GetExtension(url);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                        var response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            using (var s = await response.Content.ReadAsStreamAsync())
                            {
                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    await s.CopyToAsync(stream);
                                }
                            }
                        }
                        else
                        {
                            fileName = null;
                        }
                    }
                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public string RemoveDocFile(string OldName, string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(OldName))
                {

                    var folderName = string.Empty;

                    switch (name)
                    {
                        case "PanCardDoc":
                            folderName = Path.Combine("Resources", "Kyc", "PanCardDoc");
                            break;
                        case "AadharCardBackDoc":
                            folderName = Path.Combine("Resources", "Kyc", "AadharCardBackDoc");
                            break;
                        case "AadharCardFrontDoc":
                            folderName = Path.Combine("Resources", "Kyc", "AadharCardFrontDoc");
                            break;
                        case "MSMEDoc":
                            folderName = Path.Combine("Resources", "Kyc", "MSMEDoc");
                            break;
                        case "CancelCheque":
                            folderName = Path.Combine("Resources", "Kyc", "CancelCheque");
                            break;
                        case "Logo":
                            folderName = Path.Combine("Resources", "Kyc", "Logo");
                            break;
                        case "DigitalSign":
                            folderName = Path.Combine("Resources", "Kyc", "DigitalSign");
                            break;
                        case "Images":
                            folderName = Path.Combine("Resources", "Images");
                            break;
                        case "GSTInfo":
                            folderName = Path.Combine("Resources", "Kyc", "GSTInfo");
                            break;
                        case "ProductImage":
                            folderName = Path.Combine("Resources", "ProductImage");
                            break;
                        case "CategoryImage":
                            folderName = Path.Combine("Resources", "Images");
                            break;
                        case "HomePages":
                            folderName = Path.Combine("Resources", "HomePages");
                            break;
                        case "LendingPageSections":
                            folderName = Path.Combine("Resources", "LendingPageSections");
                            break;
                        case "ManageHeaderMenu":
                            folderName = Path.Combine("Resources", "ManageHeaderMenu");
                            break;
                        case "ManageSubMenu":
                            folderName = Path.Combine("Resources", "ManageSubMenu");
                            break;
                        case "ManageChildMenu":
                            folderName = Path.Combine("Resources", "ManageChildMenu");
                            break;
                    }


                    if (saveInS3 != true)
                    {
                        var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        var destinationFilePath = Path.Combine(destinationPath, OldName);

                        var existingFile = Directory.GetFiles(destinationPath, OldName).FirstOrDefault();
                        if (existingFile != null)
                        {
                            File.Delete(destinationFilePath);
                        }
                    }

                }
                return "File Removed Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CopyImage(string name, string file)
        {
            try
            {
                var folderName = string.Empty;

                switch (name)
                {
                    case "PanCardDoc":
                        folderName = Path.Combine("Resources", "Kyc", "PanCardDoc");
                        break;
                    case "AadharCardBackDoc":
                        folderName = Path.Combine("Resources", "Kyc", "AadharCardBackDoc");
                        break;
                    case "AadharCardFrontDoc":
                        folderName = Path.Combine("Resources", "Kyc", "AadharCardFrontDoc");
                        break;
                    case "MSMEDoc":
                        folderName = Path.Combine("Resources", "Kyc", "MSMEDoc");
                        break;
                    case "CancelCheque":
                        folderName = Path.Combine("Resources", "Kyc", "CancelCheque");
                        break;
                    case "Logo":
                        folderName = Path.Combine("Resources", "Kyc", "Logo");
                        break;
                    case "DigitalSign":
                        folderName = Path.Combine("Resources", "Kyc", "DigitalSign");
                        break;
                    case "Images":
                        folderName = Path.Combine("Resources", "Images");
                        break;
                    case "GSTInfo":
                        folderName = Path.Combine("Resources", "Kyc", "GSTInfo");
                        break;
                    case "ProductImage":
                        folderName = Path.Combine("Resources", "ProductImage");
                        break;
                    default:
                        return false;
                }

                var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var destinationPath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Resources", "Orders", "ProductImage"));

                if (saveInS3 != true)
                {
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }

                    var sourceFilePath = Path.Combine(sourcePath, file);
                    var destinationFilePath = Path.Combine(destinationPath, file);
                    File.Delete(destinationFilePath);
                    File.Copy(sourceFilePath, destinationFilePath);

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}