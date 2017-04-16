using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardSimulator3ClassLibrary
{
    public class DeckOfCards
    {
        private bool initialised = false;
        private bool randomised = false;
        private List<Card> cardDeck;

        /// <summary>
        /// States if there are any cards left
        /// </summary>
        /// <returns></returns>
        public bool hasCards()
        {
            return cardDeck.Count != 0 ? true : false;
        }
        /// <summary>
        /// Removes the next card from the deck.
        /// If the deck has not been initialised with cards
        /// this method will initialise the cards.
        /// </summary>
        /// <returns></returns>
        public Card getNextCard() 
        {
            if (initialised == false) 
            {
                InitialiseCards();
            }

            Card tmp = cardDeck[0];
            cardDeck.RemoveAt(0);
            return tmp;    
        }

        /// <summary>
        /// Fill up the deck with each of the normal 52 cards in order.
        /// </summary>
        public void InitialiseCards() 
        {
            cardDeck = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                if (suit != Suit.NoSuit) 
                {
                    foreach (FaceValue fv in Enum.GetValues(typeof(FaceValue)))
                    {
                        if (fv != FaceValue.NoFaceValue)
                        {
                            cardDeck.Add(new Card(suit, fv));
                        }        
                    }
                }
            }
            initialised = true;
        }

        /// <summary>
        /// Simple way to randomise the cards in a Deck
        /// </summary>
        public void RandomiseCards()
        {
            Random rnd = new Random();
            cardDeck = Shuffle(cardDeck, rnd);
            randomised = true;
        }


        /// <summary>
        /// Shuffling algorithm based on Fisher-Yates
        /// Adapted from the reference below.
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/273313/randomize-a-listt"/>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to be shuffled</param>
        /// <param name="rnd">Random class instance</param>
        private  List<Card> Shuffle<Card>(List<Card> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                Swap(list, i, rnd.Next(i, list.Count));
            return list;
        }
         
        private void Swap<Card>(List<Card> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

    }
}
