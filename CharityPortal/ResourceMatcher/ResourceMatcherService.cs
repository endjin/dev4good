using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Configuration;
using System.Web;
using System.Xml.Linq;
using CharityPortal.Data;

namespace ResourceMatcher
{
    public partial class ResourceMatcherService : ServiceBase
    {
        private bool _stopLoop = false;
        public ResourceMatcherService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Action start = StartLoop;
            var result = start.BeginInvoke(null, null);
            Console.ReadLine();
            start.EndInvoke(result);
        }

        protected override void OnStop()
        {
            StopLoop();
        }

        public void StartLoop()
        {
            _stopLoop = false;

            int sleepFrequency;
            if (!int.TryParse(ConfigurationManager.AppSettings["LoopFrequency"], out sleepFrequency))
            {
                sleepFrequency = 5000;
            }

            ImportTweets();

            while (!_stopLoop)
            {
                Console.WriteLine("Still executing");

                Thread.Sleep(sleepFrequency);
            }
        }

        private long _lastTweetId = 0;

        /// <summary>
        /// Update the recorded Id of the most recent tweet
        /// </summary>
        private void UpdateLastTweetId(IEnumerable<Tweet> tweets)
        {
            if (tweets.Any())
            {
                _lastTweetId = Math.Max(_lastTweetId, tweets.Max(t => t.Id));
            }
        }

        static string _twitterUrl = "http://search.twitter.com/search.atom?rpp=100&since_id=";

        static IObservable<string> SearchTwitter(string searchText, long lastId)
        {
            try
            {
                var uri = _twitterUrl + lastId + "&q=" + searchText;

                var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(uri));

                var twitterSearch = Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse);

                return twitterSearch().Select(res => WebResponseToString(res));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return Observable.Empty<string>();
        }

        private static readonly string _MagicHashTag = "#dev4goodgaza";

        private void ImportTweets()
        {
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(30))
                .SelectMany(ticks => SearchTwitter(HttpUtility.UrlEncode(_MagicHashTag), _lastTweetId))
                .Select(ParseTwitterSearch)
                .Subscribe(tweets => { 
                                        UpdateLastTweetId(tweets); 
                                        tweets.ToList().ForEach(SaveResourceFromTweet); 
                                    }
                            );
        }

        private void SaveResourceFromTweet(Tweet tweet)
        {
            Resource resource = null;
            Regex tweetSplitter = new Regex(@"^(" + _MagicHashTag + @")\W+(#(p|o)\W*(\w+))\W+(.*?)((\d+)\W*(.*)){0,1}$");

            MatchCollection matches = tweetSplitter.Matches(tweet.Title);
            if(matches.Count==1)
            {
                //Looks like a good tweet
                resource= new Resource();
                var match = matches[0];
                string ownerType = match.Groups[3].Value; //P or O
                string ownerName = match.Groups[4].Value; //numeric

                int quantity;
                string units;
                if (match.Groups.Count > 7)
                {
                    quantity = int.Parse(match.Groups[7].Value);
                    units = match.Groups[8].Value;
                }
                else
                {
                    quantity = int.Parse(match.Groups[7].Value);
                    units = match.Groups[8].Value;
                }

                Organization organization = null;
                Project project = null;

                if(ownerType=="o")
                {
                    organization = _context.Organizations.FirstOrDefault(o => o.Name == ownerName);
                    if (organization == null)
                    {
                        organization = new Organization();
                        organization.Name = ownerName;
                        organization.ContactEmail = "@"+tweet.Author;
                        _context.Organizations.AddObject(organization);
                    }
                    resource.Organization = organization;
                }
                else if(ownerType=="p")
                {
                    project = _context.Projects.FirstOrDefault(o => o.Name == ownerName);
                    if (project == null)
                    {
                        project = new Project();
                        project.Name = ownerName;
                        project.IsActive = true;
                        project.Description = "Created by " + tweet.Author + " via twitter at " + DateTime.UtcNow.ToString("s");
                        _context.Projects.AddObject(project);

                        //Need to connect to an organisation too
                        organization = _context.Organizations.FirstOrDefault(o => o.ContactEmail == "@" + tweet.Author);
                        if (organization == null)
                        {
                            organization = new Organization();
                            organization.ContactEmail = "@" + tweet.Author;
                            organization.Name = tweet.Author; //default for now?

                            _context.Organizations.AddObject(organization);
                        }

                        project.AdminOrganization = organization;

                        //TODO: Make this correct
                        project.Location.Address = string.Empty;
                        project.Location.Latitude = 51.5;
                        project.Location.Longitude = -1.75;
                    }
                    resource.Project = project;
                }

                string description = match.Groups[5].Value; //description
                resource.Description = description;
                resource.Title = description;
                resource.Quantity = quantity;
                resource.QuantityUnits = units;

                //TODO: Make this correct
                resource.Location.Address = string.Empty;
                resource.Location.Latitude = 51.5;
                resource.Location.Longitude = -1.75;

                _context.AddToResources(resource);
                _context.SaveChanges();
            }
        }

        private static readonly CharityPortal.Data.DataContextContainer _context = new CharityPortal.Data.DataContextContainer();

        private static string WebResponseToString(WebResponse webResponse)
        {
            HttpWebResponse response = (HttpWebResponse)webResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// A tweet!
        /// </summary>
        public class Tweet
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string ProfileImageUrl { get; set; }
            public DateTime Timestamp { get; set; }

            public Tweet()
            { }

            public Tweet(Tweet tweet)
            {
                Id = tweet.Id;
                Title = tweet.Title;
                ProfileImageUrl = tweet.ProfileImageUrl;
                Author = tweet.Author;
                Timestamp = tweet.Timestamp;
            }

            public override string ToString()
            {
                return Title;
            }
        }

        private static string _atomNamespace = "http://www.w3.org/2005/Atom";

        private static XName _entryName = XName.Get("entry", _atomNamespace);

        private static XName _idName = XName.Get("id", _atomNamespace);

        private static XName _linkName = XName.Get("link", _atomNamespace);

        private static XName _publishedName = XName.Get("published", _atomNamespace);

        private static XName _nameName = XName.Get("name", _atomNamespace);

        private static XName _titleName = XName.Get("title", _atomNamespace);

        private static IEnumerable<Tweet> ParseTwitterSearch(string response)
        {
            var doc = XDocument.Parse(response);
            return doc.Descendants(_entryName)
                      .Select(entryElement => new Tweet()
                      {
                          Title = entryElement.Descendants(_titleName).Single().Value,
                          Id = long.Parse(entryElement.Descendants
                        (_idName).Single().Value.Split(':')[2]),
                          ProfileImageUrl = entryElement.Descendants
                        (_linkName).Skip(1).First().Attribute("href").Value,
                          Timestamp = DateTime.Parse(entryElement.Descendants
                        (_publishedName).Single().Value),
                          Author = ParseTwitterName(entryElement.Descendants
                        (_nameName).Single().Value)
                      });
        }

        private static string ParseTwitterName(string name)
        {
            int bracketLocation = name.IndexOf("(");
            return name.Substring(0, bracketLocation - 1);
        }

        public void StopLoop()
        {
            _stopLoop = true;
        }
    }
}

namespace CharityPortal.Utils
{
    namespace Database
    {
        public static class Database
        {
            public static int ExecuteNonQuery(this SqlConnection connection, string sprocName, params SqlParameter[] sqlParameters)
            {
                using (var sqlCommand = new SqlCommand(sprocName, connection))
                {
                    return sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
