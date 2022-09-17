using Consul;
using Microsoft.Extensions.Options;

namespace OrderApi.Services
{
    public class ConsulRegisterServices : IHostedService
    {
        private IConsulClient _client;
        private IOptions<ConsulConfigration> _consulConfigration;
        private IOptions<OrderConfigration> _orderConfigration;
        private ILogger<ConsulRegisterServices> _logger;

        public ConsulRegisterServices(
            IConsulClient client,
            IOptions<ConsulConfigration> ConsulConfigration,
            IOptions<OrderConfigration> OrderConfigration,
            ILogger<ConsulRegisterServices> logger
            )
        {
            _client=client;
            _consulConfigration = ConsulConfigration;
            _orderConfigration = OrderConfigration;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var orderUrl = new Uri(_orderConfigration.Value.Url);
            var serviceRegistration = new AgentServiceRegistration
            {
                Address = orderUrl.Host,
                Name = _orderConfigration.Value.ServiceName,
                ID = _orderConfigration.Value.ServiceId,
                Port= orderUrl.Port,
                Tags = new[] { _orderConfigration.Value.ServiceName }
            };

           await _client.Agent.ServiceDeregister(_orderConfigration.Value.ServiceId, cancellationToken);
           await _client.Agent.ServiceRegister(serviceRegistration, cancellationToken);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _client.Agent.ServiceDeregister(_orderConfigration.Value.ServiceId, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
