using System;
using Microsoft.AspNet.SignalR.Messaging;
using Raven.Client;
using Raven.Client.Documents;

namespace SignalR.RavenDB
{
    public class RavenScaleoutConfiguration : ScaleoutConfiguration
    {
        private readonly Func<IDocumentStore> _documentStoreFactory;

        public RavenScaleoutConfiguration(string connectionStringNameOrUrl)
            : this(CreateConnectionFactory(connectionStringNameOrUrl)) { }
        public RavenScaleoutConfiguration(string databaseName, string url)
           : this(CreateConnectionFactory(databaseName, url)) { }

        public RavenScaleoutConfiguration(Func<IDocumentStore> documentStoreFactory)
        {
            if (documentStoreFactory == null)
                throw new ArgumentNullException("documentStoreFactory");

            _documentStoreFactory = documentStoreFactory;

            this.ReconnectDelay = TimeSpan.FromSeconds(2);
            this.Expiration = TimeSpan.Zero;
        }

        public TimeSpan ReconnectDelay { get; set; }

        public TimeSpan Expiration { get; set; }

        internal Func<IDocumentStore> DocumentStoreFactory
        {
            get { return _documentStoreFactory; }
        }
        private static Func<IDocumentStore> CreateConnectionFactory(string databasename, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("connectionStringNameOrUrl");

            try
            {

                return CreateConnectionFactoryFromDbNameNUrl(databasename, url);

                var uri = new Uri(url, UriKind.Absolute);
                return () => new DocumentStore { Database = databasename, Urls = new[] { uri.AbsoluteUri } };
            }
            catch (UriFormatException)
            {
                return CreateConnectionFactoryFromDbNameNUrl(databasename, url);
            }
        }
        private static Func<IDocumentStore> CreateConnectionFactory(string connectionStringNameOrUrl)
        {
            if (string.IsNullOrWhiteSpace(connectionStringNameOrUrl))
                throw new ArgumentNullException("connectionStringNameOrUrl");

            try
            {
                if (!Uri.IsWellFormedUriString(connectionStringNameOrUrl, UriKind.Absolute))
                    return CreateConnectionFactoryFromConnectionStringName(connectionStringNameOrUrl);

                var uri = new Uri(connectionStringNameOrUrl, UriKind.Absolute);
                return () => new DocumentStore { Urls = new[] { uri.AbsoluteUri } };
            }
            catch (UriFormatException)
            {
                return CreateConnectionFactoryFromConnectionStringName(connectionStringNameOrUrl);
            }
        }

        private static Func<IDocumentStore> CreateConnectionFactoryFromConnectionStringName(string connectionStringName)
        {
            return () => new DocumentStore { Database = connectionStringName };
        }

        private static Func<IDocumentStore> CreateConnectionFactoryFromDbNameNUrl(string databaseName, string Url)
        {
            return () => new DocumentStore { Database = databaseName, Urls = new[] { Url } };
        }
    }
}