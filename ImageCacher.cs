using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

namespace HttpGetRandomImageWebAPI;

public interface IImageCacher
{
    byte[] GetImage(string pickedImagePath);
}

public class ImageCacher : IImageCacher
{
    IMemoryCache m_memoryCache;
    private ImageObject? m_imageObject;

    public ImageCacher(IMemoryCache memoryCache)
    {
        m_memoryCache = memoryCache;
        m_imageObject = null;
    }

    public byte[] GetImage(string pickedImagePath)
    {

        //string msg = string.Empty;
        if (File.Exists(pickedImagePath))
        {
            m_memoryCache.TryGetValue(pickedImagePath, out m_imageObject);

            if (m_imageObject == null)
            {
                // Read time stamp of file.
                DateTime ourFileDate = File.GetLastWriteTime(pickedImagePath);
                ourFileDate = ourFileDate.AddMilliseconds(-ourFileDate.Millisecond);

                // Open the file and read the contents into a byte array
                byte[] byteArray = File.ReadAllBytes(pickedImagePath);
                m_imageObject = new ImageObject(pickedImagePath, "image/jpeg", byteArray, ourFileDate);

                // Set cache options
                var memCacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(120));

                // Cache the image
                m_memoryCache.Set(pickedImagePath, m_imageObject, memCacheEntryOptions);

                System.Console.WriteLine(pickedImagePath + " read from drive");

            }
            return m_imageObject.Content;
        }
        else
        {
            throw new FileNotFoundException();
        }
        
    }


}

