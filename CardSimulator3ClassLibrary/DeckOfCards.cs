using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardSimulator3ClassLibrary
{
    public class DeckOfCards
    {
        ///TODO : make methods have upper case start
        private bool initialised = false;

        private List<Card> cardDeck = new List<Card>();

        private static readonly Random randomNumberGenerator = new Random();
        private static readonly object syncLock = new object(); //Since we are using a static instance we could get thread issues so i'll use sync

        public DeckOfCards() 
        {
            
        }
        public DeckOfCards(List<Card> cards) 
        {
            if (cards == null) 
            {
                throw new ArgumentNullException();
            } 
            initialised = true;
            this.CardDeck = cards;
        }

        public List<Card> CardDeck
        {
            get { return cardDeck; }
            set { SetDeck(value);}
        }
            

        public int DeckSize() {
            return CardDeck == null ? 0 : CardDeck.Count + 1;            
        }

        public bool HasCards()
        {
            return CardDeck.Count != 0 ? true : false;
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

            for (int i = 0; i < CardDeck.Count; i++)
            {
                yield return CardDeck[i];
            }

        }

        /// <summary>
        /// Sometimes you need to look at cards without taking them.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public Card Peek(int cardNumber) 
        {
            if (initialised == false)
            {
                InitialiseCards();
            }
            if (cardNumber < CardDeck.Count)
            {
                return CardDeck[cardNumber];
            }
            else 
            {
                throw new IndexOutOfRangeException(); 
            }            
        }

        public bool ContainsFaceValue(FaceValue faceValue) 
        {
            foreach (Card card in GetCardEnumerator()) 
            {
                if (card.faceValue == faceValue) { return true; }
            }
            return false;
        }
        
        public DeckOfCards GetCardsWithSameFaceValue(FaceValue faceValue) 
        {
            DeckOfCards deckToReturn = new DeckOfCards();
            foreach (Card card in GetCardEnumerator()) 
            {
                if (card.faceValue == faceValue) {
                    deckToReturn.CardDeck.Add(card);
                }
            }
            return deckToReturn;
        }  


        /// <summary>
        /// Use to deal (remove) cards.  It will throw ArgumentOutOfRange if you try to remove when empty.
        /// I would prefer the client to take responsibilty here.
        /// </summary>
        /// <returns></returns>
        public Card DealCard() 
        {
            if (initialised == false)
            {
                InitialiseCards();
            }
            Card cardToDeal = CardDeck[0];
            CardDeck.RemoveAt(0);
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
        public DeckOfCards CloneDeck() 
        {
            if (initialised == false)
            {
                InitialiseCards();
            }
            List<Card> returnClonedDeck = new List<Card>();
            foreach (Card cardToClone in CardDeck) 
            {
                returnClonedDeck.Add(new Card(cardToClone));
            }
            return new DeckOfCards(returnClonedDeck);   
        }

        /// <summary>
        /// Fill up the deck with each of the normal 52 cards in order.
        /// </summary>
        public void InitialiseCards()
        {
            CardDeck = MakeStandardListOfCards().CardDeck;
            initialised = true;
        }

        public DeckOfCards Shuffle() 
        {
            if (initialised == false)
            {
                InitialiseCards();
                initialised = true;
            }
            return Shuffle(this);
        } 

        public DeckOfCards Shuffle(DeckOfCards deck)
        {
            return Shuffle(deck, randomNumberGenerator);
        }

        /// <summary>
        /// Shuffling algorithm based on Fisher-Yates
        /// Adapted from the reference below.
        /// </summary>
        /// <see cref="https://blog.codinghorror.com/the-danger-of-naivete/"/>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to be shuffled</param>
        /// <param name="rnd">Random class instance</param>
        public DeckOfCards Shuffle(DeckOfCards deck, Random rnd)
        {

            int n = 0;
            for (int i = deck.CardDeck.Count - 1; i > 0; i--)
            {
                lock (syncLock) //I am using a static instance for rnd, 
                //so we should lock the thread in case another thread tries to use the random number gen. 
                {
                    n = rnd.Next(i + 1);
                }
                Swap(deck.CardDeck, i, n);
            }
  
            return deck;
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
        public static DeckOfCards MakeStandardListOfCards()
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
            return new DeckOfCards(cards);
        }

        /// <summary>
        /// Take a List of card and modify it to a standards list of cards and return this instance
        /// </summary>
        /// <returns>Your list which is now populated with a standard 52 cards</returns>
        /// <throws>NullReferenceException if a null List of cards if supplied as argument </throws>
        public static DeckOfCards MakeStandardListOfCards(DeckOfCards toBeModifiedAsStandard)
        {
            if (Object.ReferenceEquals(toBeModifiedAsStandard,null))
            {
                throw new NullReferenceException("You must provide a non-null List<Card> for makeStandardListOfCards");
            }
            DeckOfCards tmpDeck = MakeStandardListOfCards();
            foreach (Card c in tmpDeck.CardDeck)
            {
                toBeModifiedAsStandard.CardDeck.Add(new Card(c));
            }
            return toBeModifiedAsStandard;
        }
    }
}
