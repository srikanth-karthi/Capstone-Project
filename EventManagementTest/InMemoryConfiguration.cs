using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace EventManagementTest
{
    public class InMemoryConfiguration
    {
        public IConfiguration Configuration { get; }

        public InMemoryConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "TokenKey:JWT", "9667cb52368dc29f17b2dbdfcaeebc3afeeea4bfb7c3ca665f1cf5da770286319e39f212c203bc5bd33785dac4da6bed7861eb1cfd022796361dfc190f2b2748" },
                { "AzureStorage:ConnectionString", "DefaultEndpointsProtocol=https;AccountName=eventmanagementacc;AccountKey=X1wUlxJVIA2Ni1Y5aV6Er+h3dIBlJxOE/DnFCGUXGflT2+vhpGaCDWpbe6PCnOROIYsQMIVdnzpW+AStIz7ZeA==;EndpointSuffix=core.windows.net" },
                { "AzureStorage:ContainerName", "images" }
            };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
    }
}
