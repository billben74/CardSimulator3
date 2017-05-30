using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSimulator3ClassLibrary
{
    /// <summary>
    /// Card Games should know how to deal
    /// </summary>
    public interface IGameRules
    {
        void Deal (List<Player> player, DeckOfCards deck);
    }
}
