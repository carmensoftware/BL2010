using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

public class CarmenApi
{
    const string PATH_VENDOR = "api/interface/vendor";
    const string PATH_ACCOUNTCODE = "api/interface/accountcode";
    const string PATH_DEPARTMENT = "api/interface/department";

    private readonly string _host;
    private readonly string _token;


    public CarmenApi(string host, string token)
    {
        _host = host.Trim('/');
        _token = token;
    }

    public string Get(string path)
    {
        var json = "";

        var endpoint = string.Format("{0}/{1}", _host, path);

        using (var client = new WebClient())
        {
            client.Headers.Add("Authorization", _token);
            client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";

            json = client.DownloadString(endpoint);
        }

        return json;
    }


    public IEnumerable<Vendor> GetVendors()
    {
        var json = Get(PATH_VENDOR);
        var res = JsonConvert.DeserializeObject<VendorData>(json);

        return res.Data;
    }

    public IEnumerable<AccountDepartment> GetDepartment()
    {
        var json = Get(PATH_DEPARTMENT);
        var res = JsonConvert.DeserializeObject<DepartmentData>(json);

        return res.Data;
    }

    public IEnumerable<AccountCode> GetAccountCode()
    {
        var json = Get(PATH_ACCOUNTCODE);
        var res = JsonConvert.DeserializeObject<AccountCodeData>(json);

        return res.Data;
    }


    // Internal Classs(es)

    internal class VendorData
    {
        public IEnumerable<Vendor> Data { get; set; }
    }

    internal class AccountCodeData
    {
        public IEnumerable<AccountCode> Data { get; set; }
    }

    internal class DepartmentData
    {
        public IEnumerable<AccountDepartment> Data { get; set; }
    }


    // Public class(es)

    [Serializable]
    public class Vendor
    {
        public int Id { get; set; }
        public string VnCode { get; set; }
        public string VnName { get; set; }
        public string VnAdd1 { get; set; }
        public string VnAdd2 { get; set; }
        public string VnAdd3 { get; set; }
        public string VnAdd4 { get; set; }
        public string VnTel { get; set; }
        public string VnFax { get; set; }
        public string VnCateCode { get; set; }
        public string VnTaxNo { get; set; }
        public string BranchNo { get; set; }
        public string VnVat1 { get; set; }
        public decimal? VnTaxR1 { get; set; }
        public int? VnTerm { get; set; }
        public bool Active { get; set; }
        public DateTime? LastModified { get; set; }
        public string UserModified { get; set; }


        //public string VnCateDesc { get; set; }
        //public string VnEmail { get; set; }
        //public int VnDisTrm { get; set; }
        //public decimal VnDisPct { get; set; }
        //public string VnRegNo { get; set; }
    }

    [Serializable]
    public class AccountCode
    {
        public int Id { get; set; }
        public string AccCode { get; set; }
        public string AccDesc1 { get; set; }
        public string AccDesc2 { get; set; }
        public string AccNature { get; set; }
        public string AccType { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    [Serializable]
    public class AccountDepartment
    {
        public int Id { get; set; }
        public string DeptCode { get; set; }
        public string DeptDesc { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }





}