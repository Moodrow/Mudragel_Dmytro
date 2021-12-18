using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using RestSharp;
using AventStack.ExtentReports;

namespace WebAPI
{

    public static class DropBox
    {

        public static class BaseDropBoxInfo
        {

            private static readonly string accesstoken = "Bearer sl.A-aQcbvtHqUazNPN2b0Y19iZbzSMj-D45lRugucyzE_EPKDRCowG3TUyDMgn6q1T2T_IbckksYKTPe__A2RxCJgw9ScYUVIpWcYJIXeZWG-o58FoaR2Ss8jPI10pPYik6WH0I6gnSR_L";

            private static readonly string url1 = "https://content.dropboxapi.com/2/files/upload";

            private static readonly string url2 = "https://content.dropboxapi.com/2/files/upload";

            private static readonly string url3 = "https://content.dropboxapi.com/2/files/upload";

            public static string Accesstoken { get => accesstoken; }

            public static string Url1 { get => url1; }

            public static string Url2 { get => url2; }

            public static string Url3 { get => url3; }
        }

        public static string GetFileId(string response)
        {
            int index = response.IndexOf("id:");
            string res = "";
            char line = '\"';
            int i = index + 3;
            while ((i < response.Length) && (response[i] != line))
            {
                res += response[i];
                i++;
            }
            return res;
        }
    }


    public class Tests
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestForUpload()
        {
            var client = new RestClient(DropBox.BaseDropBoxInfo.Url1)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Dropbox-API-Arg", "{\"path\": \"/File.txt\",\"mode\": \"add\",\"autorename\": true,\"mute\": false,\"strict_conflict\": false}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddHeader("Authorization", DropBox.BaseDropBoxInfo.Accesstoken);
            var body = @"text";
            request.AddParameter("application/octet-stream", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string res = DropBox.GetFileId(response.Content);
            Assert.IsTrue(response.Content.Contains(res));
        }

        [Test]
        public void TestForMetadata()
        {
            var client = new RestClient(DropBox.BaseDropBoxInfo.Url2)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("data", "{\"path\": \"/File.txt\",\"include_media_info\": false,\"include_deleted\": false,\"include_has_explicit_shared_members\": false}");
            request.AddHeader("Authorization", DropBox.BaseDropBoxInfo.Accesstoken);
            var body = @"{" + "\n" + @"    ""path"": ""/File.txt""," + "\n" + @"    ""include_media_info"": false," + "\n" + @"    ""include_deleted"": false," + "\n" + @"    ""include_has_explicit_shared_members"": false" + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string res = DropBox.GetFileId(response.Content);
            Assert.IsTrue(response.Content.Contains(res));
        }

        [Test]
        public void TestForDeleting()
        {
            var client = new RestClient(DropBox.BaseDropBoxInfo.Url3)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("data", "{\"path\": \"/File.txt\"}");
            request.AddHeader("Authorization", DropBox.BaseDropBoxInfo.Accesstoken);
            var body = @"{" + "\n" + @"    ""path"": ""/File.txt""" + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string res = DropBox.GetFileId(response.Content);
            Assert.IsTrue(response.Content.Contains(res));
        }

    }
}