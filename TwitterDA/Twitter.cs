using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Net;


namespace Spaetzel.TwitterDA
{
    public static class Twitter
    {
        public static List<TwitterUser> GetFriends(string username)
        {
            return GetFriends(username, 1);
        }

        public static List<TwitterUser> GetFriends(string username, int numPages)
        {
            List<TwitterUser> output = new List<TwitterUser>();
            List<TwitterUser> curPageOutput;

            int curPage = 1;

            do
            {
                curPageOutput = GetFriendsPage(username, curPage);

                output.AddRange(curPageOutput);

                curPage++;
            } while (curPage <= numPages && curPageOutput.Count <= 100 && curPageOutput.Count > 0 );

            return output;
        }


        private static List<TwitterUser> GetFriendsPage(string username, int pageNum)
        {
            string url = String.Format( "http://twitter.com/statuses/friends/{0}.xml?page={1}", username, pageNum );

            XDocument doc = XDocument.Load(url);

            var users = from user in doc.Descendants("user")
                        select new TwitterUser()
                        {
                            Id = Convert.ToInt32(user.Element("id").Value),
                            Name = user.Element("name").Value,
                            ScreenName = user.Element("screen_name").Value,
                            Location = user.Element("location").Value,
                            Description = user.Element("description").Value,
                            ProfileImageUrl = user.Element("profile_image_url").Value,
                            Url = user.Element("url").Value,
                            Protected = (user.Element("protected").Value == "true")


                        };

            /* Status = 
                            Status = from status in user.Descendants("status").First(
                                     select new TwitterStatus()
                                     {
                                         CreatedAt = DateTime.Parse(status.Element("created_at").Value),
                                         Id = Convert.ToInt32(status.Element("id").Value),
                                         Text = status.Element("text").Value,
                                         Source = status.Element("source").Value,
                                         Truncated = status.Element("truncated").Value == "true"
                                     })
             * */

            return users.ToList();
        }

        private static List<TwitterStatus> GetDocStatuses( XDocument doc )
        {
            var statuses = from status in doc.Descendants("status")
                                     select new TwitterStatus()
                                     {
                                        // CreatedAt = DateTime.Parse(status.Element("created_at").Value),
                                         Id = Convert.ToInt32(status.Element("id").Value),
                                         Text = status.Element("text").Value,
                                         Source = status.Element("source").Value,
                                         Truncated = status.Element("truncated").Value == "true"
                                     };

            return statuses.ToList();
        }

        /// <summary>
        /// Creates a friendship between the current user and the given user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The user that was added as a friend</returns>
        public static TwitterUser CreateFriendship(string username)
        {
            string url = "http://twitter.com/friendships/create/" + username + ".xml";


            var result = Utilities.HttpPost(url, Config.Username, Config.Password, "");


            XDocument doc = XDocument.Parse(result);

            var users = GetDocUsers(doc);

          

            TwitterUser addedUser = users.FirstOrDefault();

            if( addedUser == null )
            {
                throw new ArgumentException(String.Format("Could not added user %1 to friends list", username ) );
            }else{
                return addedUser;
            }


        }

        private static List<TwitterUser> GetDocUsers( XDocument doc )
        {
              var users = from user in doc.Descendants("user")
                        select new TwitterUser()
                        {
                            Id = Convert.ToInt32(user.Element("id").Value),
                            Name = user.Element("name").Value,
                            ScreenName = user.Element("screen_name").Value,
                            Location = user.Element("location").Value,
                            Description = user.Element("description").Value,
                            ProfileImageUrl = user.Element("profile_image_url").Value,
                            Url = user.Element("url").Value,
                            Protected = (user.Element("protected").Value == "true")
                           
                                     
                        };

              return users.ToList();

        }

        public static bool NewDirectMessage(string recipient, string message)
        {
            if (message.Length > Config.MaxMessageLength)
            {
                throw new ArgumentException(String.Format("Message must be {0} characters or less", Config.MaxMessageLength));
            }

            string uri = "http://twitter.com/direct_messages/new.xml";

            var parameters = String.Format("user={0}&text={1}", recipient, message );



            var result = Utilities.HttpPost(uri, Config.Username, Config.Password, parameters);


          /*  XDocument doc = XDocument.Load(result);

            var statuses = GetDocStatuses(doc);



            TwitterStatus addedStatus = statuses.FirstOrDefault();


            return addedStatus;
            */
            return true;
        }

        public static TwitterStatus Update(string status)
        {
            if (status.Length > Config.MaxMessageLength)
            {
                throw new ArgumentException(String.Format("Message must be {0} characters or less", Config.MaxMessageLength));
            }


            string uri = "http://twitter.com/statuses/update.xml";

            var parameters = String.Format("status={0}", status);



            var result = Utilities.HttpPost(uri, Config.Username, Config.Password, parameters);


            XDocument doc = XDocument.Parse(result);

            var statuses = GetDocStatuses(doc);



            TwitterStatus addedStatus = statuses.FirstOrDefault();

           
                return addedStatus;



        }


        public static bool VerifyCredentials()
        {


            string uri = "http://twitter.com/account/verify_credentials.xml";

            try
            {
                GetAuthenticatedUriReader(uri);
                return true;
            }
            catch
            {
                return false;
            }



        }


   /*     private static string LoggedInUrl
        {
            get
            {
                
                string url = "http://" + Config.Username + ":" + Config.Password + "@twitter.com";
                Uri uri = new Uri(url);

             //   uri.us
                return url;
            }
        }*/

      private static StreamReader GetAuthenticatedUriReader(string uri)
        {
            WebRequest request = WebRequest.Create(uri);

          

            if (Config.Username != String.Empty)
            {
                request.UseDefaultCredentials = false;
                request.Credentials = new NetworkCredential(Config.Username, Config.Password);
            }

            

            var response = request.GetResponse();

            var stream = response.GetResponseStream();

            var reader = new StreamReader(stream);

            return reader;


        }

    }
}
