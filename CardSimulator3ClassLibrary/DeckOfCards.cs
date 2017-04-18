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
        private List<Card> cardDeck = new List<Card>();

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
            cardDeck = makeStandardListOfCards(cardDeck);
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
        private List<Card> Shuffle<Card>(List<Card> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                Swap(list, i, rnd.Next(i, list.Count));
            return list;
        }

        /// <summary>
        /// Simple swap utility for a List<Card> 
        /// </summary>
        /// <typeparam name="Card"></typeparam>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Swap<Card>(List<Card> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// A utility factory-like method to make a list of cards.
        /// We need List's random access for shuffle. 
        /// </summary>
        public static List<Card> makeStandardListOfCards()
        {
            List<Card> cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                if (suit != Suit.NoSuit)
                {
                    foreach (FaceValue fv in Enum.GetValues(typeof(FaceValue)))
                    {
                        if (fv != FaceValue.NoFaceValue)
                        {
                            cards.Add(new Card(suit, fv));
                        }
                    }
                }
            }
            return cards;
        }

        /// <summary>
        /// Take a List<Card> and modify it to a standards list of cards and return this instance
        /// </summary>
        /// <returns>Your list which is now populated with a standard 52 cards</returns>
        /// <throws>NullReferenceException if a null List<Card> if supplied as argument </throws>
        public static List<Card> makeStandardListOfCards(List<Card> toBeModifiedAsStandard)
        {
            if (Object.ReferenceEquals(toBeModifiedAsStandard,null))
            {
                throw new NullReferenceException("You must provide a non-null List<Card> for makeStandardListOfCards");
            }
            List<Card> tmpCards = makeStandardListOfCards();
            foreach (Card c in tmpCards)
            {
                toBeModifiedAsStandard.Add(new Card(c));
            }
            return toBeModifiedAsStandard;
        }
    }
}
