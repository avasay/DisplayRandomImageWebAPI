using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Caching.Memory;

namespace HttpGetRandomImageWebAPI.Controllers;

[Route("[controller]/[action]")]
public class ImageController : Controller
{
    IMemoryCache m_memoryCache;
    public ImageController(IMemoryCache memoryCache)
    {
        m_memoryCache = memoryCache;
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
            try
            {
                string localPath = Path.Combine(Environment.CurrentDirectory, "Images", dirPath);
                DirectoryCacher dirCacher = new DirectoryCacher(m_memoryCache, localPath);

                List<string> listCache = dirCacher.GetListCache();

                // Get a random filename from the cache
                string pickedRandomImage = PickRandomFromCache(listCache);

                // Instantiate the ImageCacher object
                ImageCacher imageCacher = new ImageCacher(m_memoryCache, pickedRandomImage);

                msg = imageCacher.GetImage() + " -- " + pickedRandomImage;
            }
            catch (Exception ex)
            {
                // Log something
            }
            return Content(msg);
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
