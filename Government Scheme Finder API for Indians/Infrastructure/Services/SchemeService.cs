using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Asp_.Net_Web_Api.Infrastructure.Services
{
    public class SchemeService: ISchemeService
    {
        private readonly AppDbContext _context;

        public SchemeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Scheme>> GetAllAsync()
        {
            return await _context.Schemes.ToListAsync();
        }

        public async Task<IEnumerable<Scheme>> RecommendSchemesAsync(User user)
        {
            return await _context.Schemes
                .Where(s => s.State == user.State || s.EligibilityCriteria.Contains(user.Profession))
                .ToListAsync();
        }

        public async Task AddSchemeAsync(Scheme scheme)
        {
            _context.Schemes.Add(scheme);
            await _context.SaveChangesAsync();
        }
    }
}
