using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Pagination
{
    public int Current { get; set; }
    public int Pages { get; set; }
    public string BaseUrl { get; set; }

    public string GetPageUrl(int page)
    {
        if (BaseUrl.Contains("?"))
            return BaseUrl + "&p=" + page;
        else
            return BaseUrl + "?p=" + page;
    }
}
