using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

namespace HttpGetRandomImageWebAPI;

public class ImageCacher
{
    IMemoryCache m_memoryCache;
    private ImageObject? m_imageObject;
    private string m_pickedImagePath;

    public ImageCacher(IMemoryCache memoryCache, string pickedImagePath)
    {
        m_memoryCache = memoryCache;
        m_imageObject = null;

        if (File.Exists(pickedImagePath))
        {
            m_pickedImagePath = pickedImagePath;
        }
        else
        {
            throw new FileNotFoundException();
        };
    }

    public string GetImage()
    {
        string msg = string.Empty;
        m_memoryCache.TryGetValue(m_pickedImagePath, out m_imageObject);

        if (m_imageObject == null)
        {
            // Read time stamp of file.
            DateTime ourFileDate = File.GetLastWriteTime(m_pickedImagePath);
            ourFileDate = ourFileDate.AddMilliseconds(-ourFileDate.Millisecond);

            // Open the file and read the contents into a byte array
            byte[] byteArray = File.ReadAllBytes(m_pickedImagePath);
            m_imageObject = new ImageObject(m_pickedImagePath, "image/jpeg", byteArray, ourFileDate);

            // Set cache options
            var memCacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(120));

            // Cache the image
            m_memoryCache.Set(m_pickedImagePath, m_imageObject, memCacheEntryOptions);

            msg = "Loading image from drive.";
        }
        else
        {
            msg = "Loading image from cache.";
        }

        return msg;
    }


}

