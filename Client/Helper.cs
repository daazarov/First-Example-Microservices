using Messages.Commands;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    internal static class Helper
    {
        private static IEndpointInstance _instance;
        private static EndpointConfiguration _endpointConfiguration;

        internal static EndpointConfiguration ConfigureEndpoint(IConfiguration configuration)
        {

            _endpointConfiguration = new EndpointConfiguration(configuration["NServiceBus:EndPoint:Name"]);

            var transport = _endpointConfiguration.UseTransport<RabbitMQTransport>();

            string connectionString = String.Empty;
            connectionString += String.IsNullOrEmpty(configuration["NServiceBus:Connection:host"])
                ? "host=localhost;"
                : "host=" + configuration["NServiceBus:Connection:host"] + ";";

            connectionString += String.IsNullOrEmpty(configuration["NServiceBus:Connection:vhost"])
                ? ""
                : "virtualhost=" + configuration["NServiceBus:Connection:vhost"] + ";";

            connectionString += String.IsNullOrEmpty(configuration["NServiceBus:Connection:username"])
                ? "username=guest;"
                : "username=" + configuration["NServiceBus:Connection:username"] + ";";

            connectionString += String.IsNullOrEmpty(configuration["NServiceBus:Connection:password"])
                ? "password=guest;"
                : "password=" + configuration["NServiceBus:Connection:password"] + ";";

            connectionString += String.IsNullOrEmpty(configuration["NServiceBus:Connection:port"])
                ? ""
                : "port=" + configuration["NServiceBus:Connection:port"] + ";";

            transport.ConnectionString(connectionString);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(BookAppartment), "hotel");
            routing.RouteToEndpoint(typeof(PayInvoice), "billing");

            _endpointConfiguration.EnableInstallers();


            _endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            _endpointConfiguration.UsePersistence<InMemoryPersistence>();
            _endpointConfiguration.SendFailedMessagesTo(configuration["NServiceBus:EndPoint:ErrorQueue"]);
            _endpointConfiguration.AuditProcessedMessagesTo(configuration["NServiceBus:EndPoint:AuditQueue"]);

            /*_endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
            var metrics = _endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));*/

            return _endpointConfiguration;
        }
    }
}
