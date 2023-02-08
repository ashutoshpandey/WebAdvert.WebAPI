using AdvertAPI.Models;
using System.Threading.Tasks;

namespace WebAdvert.WebAPI.Services.Contracts
{
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel advertModel);
        Task<bool> Confirm(ConfirmAdvertModel advertModel);
    }
}
