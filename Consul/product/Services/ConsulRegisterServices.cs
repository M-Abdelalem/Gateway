using Consul;
using Microsoft.Extensions.Options;

namespace ProductApi.Services
{
    public class ConsulRegisterServices : IHostedService
    {
        private IConsulClient _client;
        private IOptions<ConsulConfigration> _consulConfigration;
        private IOptions<ProductConfigration> _productConfigration;
        private ILogger<ConsulRegisterServices> _logger;

        public ConsulRegisterServices(
            IConsulClient client,
            IOptions<ConsulConfigration> ConsulConfigration,
            IOptions<ProductConfigration> ProductConfigration,
            ILogger<ConsulRegisterServices> logger
            )
        {
            _client=client;
            _consulConfigration = ConsulConfigration;
            _productConfigration = ProductConfigration;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var orderUrl = new Uri(_productConfigration.Value.Url);
            var serviceRegistration = new AgentServiceRegistration
            {
                Address = orderUrl.Host,
                Name = _productConfigration.Value.ServiceName,
                ID = _productConfigration.Value.ServiceId,
                Port= orderUrl.Port,
                Tags = new[] { _productConfigration.Value.ServiceName }
            };

           await _client.Agent.ServiceDeregister(_productConfigration.Value.ServiceId, cancellationToken);
           await _client.Agent.ServiceRegister(serviceRegistration, cancellationToken);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _client.Agent.ServiceDeregister(_productConfigration.Value.ServiceId, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
