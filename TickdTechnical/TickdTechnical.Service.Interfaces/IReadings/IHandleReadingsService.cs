using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TickdTechnical.Models.Results;

namespace TickdTechnical.Service.Interfaces
{
    public interface IHandleReadingsService
    {
        Task<Results> HandleReadings(IFormFile file);
    }
}
