using eCommerce.Helpers.Logging;
using eCommerce.Helpers.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.Helpers.Domain
{
    public class DomainEventHandle<TDomainEvent> : Handles<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        private IDomainEventRepository _domainEventRepository;
        private IRequestCorrelationIdentifier _requestCorrelationIdentifier;

        public DomainEventHandle(IDomainEventRepository domainEventRepository, 
            IRequestCorrelationIdentifier requestCorrelationIdentifier)
        {
            this._domainEventRepository = domainEventRepository;
            this._requestCorrelationIdentifier = requestCorrelationIdentifier;
        }

        public void Handle(TDomainEvent @event)
        {
            @event.Flatten();
            @event.CorrelationID = this._requestCorrelationIdentifier.CorrelationID;
            this._domainEventRepository.Add(@event);
        }
    }
}
