using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;

/// <summary>
/// Summary description for API
/// </summary>
public class API
{
    private string _host; // https://xxxx.xxxx *No end with slash
    private string _auth;

    public API(string host, string authorization)
    {
        _host = host.Trim().TrimEnd('/') + "/";
        _auth = authorization.Trim();
    }

    public string Get(string endpoint)
    {
        try
        {
            using(var client = new WebClient())
            {
                client.BaseAddress = _host;
                client.Headers.Add("Authorization", _auth);

                var data =  client.DownloadData(SetEndpoint(endpoint));


                return Encoding.UTF8.GetString(data);
            }

        }
        catch (WebException ex)
        {
            throw ex;
        }
    }


    // Private method(s)
    private string SetEndpoint(string endpoint)
    {
        return endpoint.Trim().TrimStart('/');
    } 

}