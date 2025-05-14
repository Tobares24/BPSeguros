using Microsoft.Extensions.DependencyInjection;

namespace Common.Services
{
    public class DbContextFactoryService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextFactoryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateDbContext<T>() where T : notnull
        {
            return _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
        }
    }
}