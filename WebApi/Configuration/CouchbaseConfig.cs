using Couchbase;
using Couchbase.Configuration.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Configuration
{
    public class CouchbaseConfig
    {

        private const string ServerUri = "http://127.0.0.1:8091";
        private const string BucketName = "HelpToTeachBucket";
        private const string BucketPass = "123456";

        public static void Setup()
        {
            var config = new ClientConfiguration
            {

                Servers = new List<Uri> {
                    new Uri(ServerUri)
                },
                UseSsl = false,
                BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    {
                        "HelpToTeachBucket",new BucketConfiguration
                        {
                            Username = "andranik",
                            BucketName = BucketName,
                            Password = BucketPass,
                            UseSsl = false
                        }
                    }
                }
            };
            ClusterHelper.Initialize(config);
        }
        public static void Cleanup()
        {
            ClusterHelper.Close();
        }

    }
}
