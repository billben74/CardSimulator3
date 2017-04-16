using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSimulator3ClassLibrary
{
    interface ICardGame
    {
        void InitialiseDeck();
        void InitialDealer();
        void InitialisePlayers();
        void InitialiseTable();
        void PlayGame();
    }
}
