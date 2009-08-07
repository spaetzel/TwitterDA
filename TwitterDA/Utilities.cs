using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace Spaetzel.TwitterDA
{
    public static class Utilities
    {
        public static bool IsNullOrEmpty(this string test)
        {
            if (test == null)
            {
                return true;
            }
            else
            {
                return String.IsNullOrEmpty(test.Trim());
            }
        }

        public static string HttpPost(string uri, string parameters)
        {

            return HttpPost(uri, "", "", parameters);

        } // end HttpPost



        public static string HttpPost(string uri, string username, string password, string parameters)
        {

            string uriString;

            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();

            if (!username.IsNullOrEmpty())
            {
                myWebClient.Credentials = new NetworkCredential(username, password);
            }

            myWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


            // Apply ASCII Encoding to obtain the string as a byte array.
            byte[] byteArray = Encoding.ASCII.GetBytes( parameters  );


            // Upload the input string using the HTTP 1.0 POST method.
            byte[] responseArray;

            ServicePointManager.Expect100Continue = false;

            try
            {
                responseArray = myWebClient.UploadData(uri, "POST", byteArray);

               
                // Decode and display the response.
                return Encoding.ASCII.GetString(responseArray);
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex2)
            {
                throw ex2;
            }


            //System.Net.WebClient client = new WebClient();

            //client.
            //WebRequest webRequest = WebRequest.Create(uri);

            //webRequest.ContentType = "application/x-www-form-urlencoded";
            //webRequest.Method = "POST";

            //if (!username.IsNullOrEmpty())
            //{
            //    webRequest.Credentials = new NetworkCredential(username, password);
            //}

            //byte[] bytes = Encoding.ASCII.GetBytes(parameters);
            //Stream os = null;
            //try
            //{ // send the Post
            //    webRequest.ContentLength = bytes.Length;   //Count bytes to send
            //    os = webRequest.GetRequestStream();
            //    os.Write(bytes, 0, bytes.Length);         //Send it
            //}
            //catch (WebException ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    if (os != null)
            //    {
            //        os.Close();
            //    }
            //}

            //try
            //{ // get the response
            //    WebResponse webResponse = webRequest.GetResponse();
            //    if (webResponse == null)
            //    { return null; }
            //    StreamReader sr = new StreamReader(webResponse.GetResponseStream());
            //    return sr;
            //}
            //catch (WebException ex)
            //{
            //    throw ex;
            //}

        }
    }
}
