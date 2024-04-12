namespace Tracker.Application.Repositories;

public interface ITenantRepositoryFactory
{
    ITenantRepository GetTenant();
}