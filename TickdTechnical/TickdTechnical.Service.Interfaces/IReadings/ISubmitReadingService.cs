using System.Threading.Tasks;

namespace TickdTechnical.Service.Interfaces
{
    public interface ISubmitReadingService
    {
        public Task<bool> SubmitReading(string[] values);
    }
}
