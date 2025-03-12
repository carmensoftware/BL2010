using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using Newtonsoft.Json;

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
            using (var client = new WebClient())
            {
                client.BaseAddress = _host.TrimEnd('/') + "/";
                client.Headers.Add("Authorization", _auth);

                var data = client.DownloadData(SetEndpoint(endpoint));


                return Encoding.UTF8.GetString(data);
            }

        }
        catch (WebException ex)
        {
            throw ex;
        }
    }

    public string Post(string endpoint, string data)
    {

        var result = "";

        try
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json"; //application/x-www-form-urlencoded";
                wc.BaseAddress = _host.TrimEnd('/') + "/";
                wc.Headers.Add("Authorization", _auth);

                result = wc.UploadString(SetEndpoint(endpoint), data);

            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


    // Private method(s)
    private string SetEndpoint(string endpoint)
    {
        return endpoint.Trim().TrimStart('/');
    }

}