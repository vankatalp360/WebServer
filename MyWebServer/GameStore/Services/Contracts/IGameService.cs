using System.Collections.Generic;
using MyWebServer.GameStore.ViewModels;

namespace MyWebServer.GameStore.Services.Contracts
{
    public interface IGameService
    {
        bool AddGame(GameViewModel model);

        bool EditGame(GameViewModel model);

        bool DeleteGame(string title);

        GameViewModel FindGame(string title);

        IEnumerable<GameViewModel> AllGames();

        int? GetGameId(string title);

        List<GameViewModel> FindProductsInCart(List<int> shoppingCartProductIds);
        
    }
}