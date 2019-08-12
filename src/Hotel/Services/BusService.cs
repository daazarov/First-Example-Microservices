using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus.Logging;
using NServiceBus;
using Messages.Commands;

namespace Hotel
{
    public class BusService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILog _logger;

        private IEndpointInstance _instance;
        private EndpointConfiguration _endpointConfiguration;

        public BusService(IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = LogManager.GetLogger<BusService>();
            Init();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Service stared.");
            _instance = await Endpoint.Start(_endpointConfiguration).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Service stoped.");
            await _instance.Stop();
        }

        private void Init()
        {
            _endpointConfiguration = new EndpointConfiguration(_configuration["NServiceBus:EndPoint:Name"]);

            var transport = _endpointConfiguration.UseTransport<RabbitMQTransport>();

            string connectionString = String.Empty;
            connectionString += String.IsNullOrEmpty(_configuration["NServiceBus:Connection:host"])
                ? "host=localhost;"
                : "host=" + _configuration["NServiceBus:Connection:host"] + ";";

            connectionString += String.IsNullOrEmpty(_configuration["NServiceBus:Connection:vhost"])
                ? ""
                : "virtualhost=" + _configuration["NServiceBus:Connection:vhost"] + ";";

            connectionString += String.IsNullOrEmpty(_configuration["NServiceBus:Connection:username"])
                ? "username=guest;"
                : "username=" + _configuration["NServiceBus:Connection:username"] + ";";

            connectionString += String.IsNullOrEmpty(_configuration["NServiceBus:Connection:password"])
                ? "password=guest;"
                : "password=" + _configuration["NServiceBus:Connection:password"] + ";";

            connectionString += String.IsNullOrEmpty(_configuration["NServiceBus:Connection:port"])
                ? ""
                : "port=" + _configuration["NServiceBus:Connection:port"] + ";";

            transport.ConnectionString(connectionString);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(MakeReservation), "reservation");

            _endpointConfiguration.EnableInstallers();


            _endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            _endpointConfiguration.UsePersistence<InMemoryPersistence>();
            _endpointConfiguration.SendFailedMessagesTo(_configuration["NServiceBus:EndPoint:ErrorQueue"]);
            _endpointConfiguration.AuditProcessedMessagesTo(_configuration["NServiceBus:EndPoint:AuditQueue"]);
        }
    }
}
