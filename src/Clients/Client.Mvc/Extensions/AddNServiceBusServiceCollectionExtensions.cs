using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Messages.Commands;
using System;
using System.Threading;

namespace Client.Mvc.Extensions
{
    public static class AddNServiceBusServiceCollectionExtensions
    {
        public static EndpointConfiguration AddNServiceBus(this IServiceCollection services, IConfiguration Configuration)
        {
            IEndpointInstance endpointInstance = null;
            services.AddSingleton<IMessageSession>(_ => endpointInstance);

            var endpointConfiguration = new EndpointConfiguration(Configuration["NServiceBus:EndPoint:Name"]);
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

            transport.ConnectionString(Configuration["NServiceBus:ConnectionString"]);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(BookAppartment), "hotel");
            routing.RouteToEndpoint(typeof(PayInvoice), "billing");


            endpointConfiguration.EnableInstallers();

            endpointConfiguration.EnableUniformSession();

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(Configuration["NServiceBus:EndPoint:ErrorQueue"]);

            return endpointConfiguration;
        }
    }
}
