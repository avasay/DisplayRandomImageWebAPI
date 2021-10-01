using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpGetRandomImageWebAPI;

public class ImageObject
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime SubmitDate { get; set; }

    public ImageObject(string fn, string tp, byte[] ct, DateTime dt)
    {
        FileName = fn;
        ContentType = tp;
        Content = ct;
        SubmitDate = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, DateTimeKind.Utc);
    }

}

