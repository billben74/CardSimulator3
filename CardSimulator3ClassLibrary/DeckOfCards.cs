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
        private bool shuffled = false;
        private List<Card> cardDeck = new List<Card>();

        private static readonly Random randomNumberGenerator = new Random();
        private static readonly object syncLock = new object(); //Since we are using a static instance we could get thread issues so i'll use sync

        public int deckSize() {
            return cardDeck == null ? 0 : cardDeck.Count + 1;            
        }

        public bool hasCards()
        {
            return cardDeck.Count != 0 ? true : false;
        }
        /// <summary>
        /// Gets IEnumerable, Card,
        /// Use to go through cards in a deck without removing the cards.
        /// Use Deal to remove cards
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<Card> GetCardEnumerator()
        {
            if (initialised == false)
            {
                InitialiseCards();
            }

            for (int i = 0; i < cardDeck.Count; i++)
            {
                yield return cardDeck[i];
            }

        }
        
        /// <summary>
        /// Use to deal (remove) cards 
        /// </summary>
        /// <returns></returns>
        public Card DealCard() 
        {
            if (initialised == false)
            {
                InitialiseCards();
            }
            Card cardToDeal = cardDeck[0];
            cardDeck.RemoveAt(0);
            return cardToDeal;
        } 

        /// <summary>
        /// Set the deck. Setting is also initialising.
        /// </summary>
        /// <param name="deck"></param>
        public void SetDeck (List<Card> deck)
        {
            initialised = true;
            this.cardDeck = deck;
        }

        /// <summary>
        /// This allows the user to get a deck of cards.
        /// This is a deep copy, a clone, of the current  cardDeck (List of cards) in the DeckOfCards
        /// </summary>
        /// <returns>Clone of cardDeck</returns>
        public List<Card> cloneDeck() 
        {
            if (initialised == false)
            {
                InitialiseCards();
            }
            List<Card> returnClonedDeck = new List<Card>();
            foreach (Card cardToClone in cardDeck) 
            {
                returnClonedDeck.Add(new Card(cardToClone));
            }
            return returnClonedDeck;   
        }

        /// <summary>
        /// Fill up the deck with each of the normal 52 cards in order.
        /// </summary>
        public void InitialiseCards()
        {
            cardDeck = makeStandardListOfCards(cardDeck);
            initialised = true;
        }

        public List<Card> Shuffle() 
        {
            if (initialised == false)
            {
                InitialiseCards();
                initialised = true;
            }
            shuffled = true;
            return Shuffle(this.cardDeck);
        } 

        public List<Card> Shuffle(List<Card> cardList)
        {
            return Shuffle(cardList, randomNumberGenerator);
        }

        /// <summary>
        /// Shuffling algorithm based on Fisher-Yates
        /// Adapted from the reference below.
        /// </summary>
        /// <see cref="https://blog.codinghorror.com/the-danger-of-naivete/"/>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to be shuffled</param>
        /// <param name="rnd">Random class instance</param>
        public List<Card> Shuffle(List<Card> cardList, Random rnd)
        {

            int n = 0;
            for (int i = cardList.Count - 1; i > 0; i--)
            {
                lock (syncLock) //I am using a static instance for rnd, 
                //so we should lock the thread in case another thread tries to use the random number gen. 
                {
                    n = rnd.Next(i + 1);
                }
                Swap(cardList, i, n);
            }
     
            shuffled = true;
            return cardList;
        }


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
        /// Take a List of card and modify it to a standards list of cards and return this instance
        /// </summary>
        /// <returns>Your list which is now populated with a standard 52 cards</returns>
        /// <throws>NullReferenceException if a null List of cards if supplied as argument </throws>
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
