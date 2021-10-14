using Microsoft.AspNetCore.Mvc;

namespace HttpGetRandomImageWebAPI.Controllers;

[Route("[controller]/[action]")]
public class ImageController : Controller
{
    IImageCacher m_imageCacher;
    IDirectoryCacher m_directoryCacher;
    public ImageController(IDirectoryCacher dirCacher ,IImageCacher imageCacher)
    {
        m_imageCacher = imageCacher;
        m_directoryCacher = dirCacher;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public ActionResult Random(string dirPath)
    {
        string msg = string.Empty;

        if (dirPath != null)
        {
            byte[] byteArray = { byte.MinValue };
            try
            {
                string localPath = Path.Combine(Environment.CurrentDirectory, "Images", dirPath);
               
                List<string> listCache = m_directoryCacher.GetListCache(localPath);

                // Get a random filename from the cache
                string pickedRandomImage = PickRandomFromCache(listCache);

                byteArray = m_imageCacher.GetImage(pickedRandomImage);
            }
            catch (Exception ex)
            {
                // Log something
            }
            return File(byteArray, "image/jpeg");
        }
        else
        {
            return Content("Specify directory!");
        }    
    }
    private string PickRandomFromCache(List<string> collection)
    {
        // Pick random file
        Random R = new Random();
        string imagePath = string.Empty;
        int randomNumber = R.Next(0, collection.Count());
        imagePath = collection.ElementAt(randomNumber);
        return imagePath;
    }

}
