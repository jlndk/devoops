using System.Threading.Tasks;

namespace MiniTwit.Models
{
    public interface ILatestRepository
    {
        Task<long> ReadLatestAsync();
        Task UpdateLatestAsync(long value);
    }
}