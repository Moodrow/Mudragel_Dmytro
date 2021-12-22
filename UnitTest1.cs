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

    

        public class BaseDropBox
        {
            private readonly string accesstoken = "Bearer X9XI8eanNzoAAAAAAAAAAR9nrTPMCe85JrlEA3t1qC5pBltge5UHDoA__6NRutqf";

            public string Accesstoken { get => accesstoken; }

            public string GetFileId(string response)
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

        public class UploadToDropBox : BaseDropBox
        {
            private readonly string url = "https://content.dropboxapi.com/2/files/upload";

            public string Url { get => url; }
        }

        public class MetadataFromDropBoxFile : BaseDropBox
        {
            private readonly string url = "https://api.dropboxapi.com/2/files/get_metadata";

            public string Url { get => url; }
        }

        public class DeleteFileFromDropBox : BaseDropBox
        {
            private readonly string url = "https://api.dropboxapi.com/2/files/delete_v2";

            public string Url { get => url; }
        }

       
    


    public class Tests
    {

        UploadToDropBox uploadtodropbox = new UploadToDropBox();

        MetadataFromDropBoxFile metadataFromDropBoxFile = new MetadataFromDropBoxFile();

        DeleteFileFromDropBox deleteFileFromDropBox = new DeleteFileFromDropBox();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestForUpload()
        {
            //Arrange -

            //Act
            var client = new RestClient(uploadtodropbox.Url)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Dropbox-API-Arg", "{\"path\": \"/File.txt\",\"mode\": \"add\",\"autorename\": true,\"mute\": false,\"strict_conflict\": false}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddHeader("Authorization", uploadtodropbox.Accesstoken );
            var body = @"text";
            request.AddParameter("application/octet-stream", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string res = uploadtodropbox.GetFileId(response.Content);

            //Assert
            Assert.IsTrue(response.Content.Contains(res));
        }

        [Test]
        public void TestForMetadata()
        {
            //Arrange 
            TestForUpload();

            //Act
            var client = new RestClient(metadataFromDropBoxFile.Url)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("data", "{\"path\": \"/File.txt\",\"include_media_info\": false,\"include_deleted\": false,\"include_has_explicit_shared_members\": false}");
            request.AddHeader("Authorization", metadataFromDropBoxFile.Accesstoken);
            var body = @"{" + "\n" + @"    ""path"": ""/File.txt""," + "\n" + @"    ""include_media_info"": false," + "\n" + @"    ""include_deleted"": false," + "\n" + @"    ""include_has_explicit_shared_members"": false" + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string res = metadataFromDropBoxFile.GetFileId(response.Content);

            //Assert
            Assert.IsTrue(response.Content.Contains(res));
        }

        [Test]
        public void TestForDeleting()
        {
            //Arrange 
            TestForUpload();

            //Act
            var client = new RestClient(deleteFileFromDropBox.Url)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("data", "{\"path\": \"/File.txt\"}");
            request.AddHeader("Authorization", deleteFileFromDropBox.Accesstoken);
            var body = @"{" + "\n" + @"    ""path"": ""/File.txt""" + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string res = deleteFileFromDropBox.GetFileId(response.Content);

            //Assert
            Assert.IsTrue(response.Content.Contains(res));
        }

    }
}