using Asp_.Net_Web_Api.Model.Domain;

namespace Asp_.Net_Web_Api.Interface
{
    public interface ISchemeService
    {
        Task<IEnumerable<Scheme>> GetAllAsync();
        Task<IEnumerable<Scheme>> RecommendSchemesAsync(User user);
        Task AddSchemeAsync(Scheme scheme);
    }
}
