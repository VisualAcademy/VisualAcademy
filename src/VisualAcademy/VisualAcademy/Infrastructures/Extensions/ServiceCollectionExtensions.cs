using Azunt.Web.Infrastructure.MultiTenancy;

namespace Azunt.Web.Infrastructure.Extensions;

/// <summary>Azunt.Web 공통 서비스 등록 확장</summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzuntWeb(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<TenantPoliciesOptions>(config.GetSection("TenantPolicies"));
        services.AddSingleton<ITenantPolicyService, TenantPolicyService>();

        // 여기에 DbContext/Repository/Identity 등 공통 등록 확장 가능
        return services;
    }
}
