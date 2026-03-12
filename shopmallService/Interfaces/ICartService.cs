using publicClassLibrary.Models;

namespace shopmallService.Interfaces
{
    public interface ICartService
    {
        ResultObject getCartByIndex(int personalID, int appType);
    }
}
