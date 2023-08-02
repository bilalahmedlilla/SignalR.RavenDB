using System;
using Raven.Client.Documents.Session;

namespace SignalR.RavenDB
{
    using System;
    using Raven.Client.Documents.Changes;


    internal class RavenMessageObserver : IObserver<DocumentChange>, IDisposable
    {
        private RavenMessageBus _bus;

        public RavenMessageObserver(RavenMessageBus bus)
        {
            if (bus == null)
                throw new ArgumentNullException("bus");
            _bus = bus;
        }

        public void Dispose()
        {
            _bus = null;
        }

        public void OnNext(DocumentChange value)
        {
            _bus.TraceVerbose("Document change notification of type '{0}' received: {1}, {2}",
                value.Type, value.Id);

            if (value.Type != DocumentChangeTypes.Put)
                return;

            _bus.OnMessage(value.Id);
        }

        public void OnError(Exception error)
        {
            _bus.TraceError("Observer detected an error - {0}", error.GetBaseException());
        }

        public void OnCompleted()
        {
            _bus.TraceInformation("Observer completed.");
        }
    }


}