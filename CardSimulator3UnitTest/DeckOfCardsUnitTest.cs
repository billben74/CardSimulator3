using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CardSimulator3ClassLibrary;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace CardSimUnitTests
{

// TODO remove most comments
    [TestClass]
    public class DeckOfCardsUnitTest
    {

        private Card[] completeDeck;

        /// <summary>
        /// Although Deck of cards has utilty to make standard lists of cards
        /// I am independently making a standard array of cards so that 
        /// these tests do not rest on DeckOfCards, which I mean to test in these unit tests.
        /// </summary>
        [TestInitialize]
        public void TestInitializeCards ()
        {
            int index = 0;
            completeDeck = new Card [52];
            foreach(Suit suit in Enum.GetValues(typeof(Suit)))
            {              
                foreach(FaceValue fv in Enum.GetValues(typeof(FaceValue)))
                {
                    if (suit != Suit.NoSuit && fv != FaceValue.NoFaceValue) 
                    {
                        completeDeck[index++] = new Card(suit, fv);
                    }                   
                }
            }
        }

        [TestMethod]
        public void TestDeckOfCardsGetEnumerator ()
        {
            int index = 0;
            DeckOfCards deck = new DeckOfCards();
            deck.InitialiseCards();
            foreach(Card c in deck.GetCardEnumerator())
            {
                Assert.AreEqual(completeDeck[index++], c);
            }
            Assert.AreEqual(index, 52);// correct number of cards.
        }

        /// <summary>
        /// Test that the overload of makeStandardListOfCards that makes a new List of cards
        /// will produce a standard 52 deck of cards in correct order.
        /// </summary>
        [TestMethod]
        public void TestmakeStandardListOfCards() 
        {
            List<Card> testCards = DeckOfCards.makeStandardListOfCards();
            int index = 0;
            foreach (Card c in testCards)
            {
                Assert.AreEqual(this.completeDeck[index], testCards[index]);
                index++;
            }
        }


        [TestMethod]
        public void TestDealCard()
        {
            DeckOfCards deck = new DeckOfCards();
            int deckSize = 52;
            foreach (Card testCard in this.completeDeck)
            {
                Card c = deck.DealCard();
      
                Assert.AreEqual(testCard, c);
                Assert.AreEqual(deckSize--, deck.deckSize());
            }
        }

        /// <summary>
        /// Test that the overload of makeStandardListOfCards that takes an already made List of cards 
        /// will reset this list if cards to a standard 52 deck of cards in correct order.
        /// The test first tests that the reference that is returned is the same as the Parameter
        /// Then the test checks that each card is in the correct, standard, order
        /// </summary>
        [TestMethod]
        public void TestmakeStandardListOfCardsWithListofCardsArgumentParameter()
        {
            List<Card> testCards = new List<Card>();
            int index = 0;
            //This is not how you should normally use this function.
            //You should use it to "reset" a deck to standard with creating
            //two identical references, but I'm doing this to make sure that 
            //the method does modify the parameter and return a reference to it.
            List<Card> checkReferencesAreEqual = new List<Card>();
            checkReferencesAreEqual = DeckOfCards.makeStandardListOfCards(testCards);

            Assert.IsTrue(Object.ReferenceEquals(checkReferencesAreEqual, testCards));


            foreach (Card c in testCards)
            {
                Assert.AreEqual(this.completeDeck[index], testCards[index]);
                index++;
            }
        }

        /// <summary>
        /// Test cloneDeck by seeing if a standard deck is produced.
        /// The Cards should be equal by value but should be different references.
        /// The Listof cards made by cloneDeck and the private field DeckOfCards.cardDeck should be different references.
        /// <remarks>
        /// I realise that testing private methods or fields is on of those controversial issues that programmers like to argue over...
        /// I would simply like to make sure that the private field cardDeck is a different reference as the cloneDeck 
        /// as this is the whole point of cloneDeck. I don't won't to make it protected and create a wrapper class...
        /// I just want to test and PrivateObject does the relfection work for me.
        /// </remarks>
        [TestMethod]
        public void TestCloneDeck()
        {
            //completeDeck
            DeckOfCards deck = new DeckOfCards();
            List<Card> clonedDeck = deck.cloneDeck();

            //Check that the cloneDeck is a different reference to private field DeckOfCards.cardDeck
            PrivateObject pObj = new PrivateObject(deck);
            List<Card> useToCheckCloneMakesADifferentReference = (List<Card>)pObj.GetField("cardDeck");
            Assert.IsFalse(Object.ReferenceEquals(useToCheckCloneMakesADifferentReference, clonedDeck));

            //Check values of cards in the clonedDeck are the same as in the DeckOfCards.cardDeck
            IEnumerable<Tuple<Card,Card>> tupledEnumerator = MultipleIterate.Over(clonedDeck,deck.GetCardEnumerator());
            foreach (var decksTuple in tupledEnumerator)
            {
                Assert.AreEqual(decksTuple.Item1, decksTuple.Item2);
                Assert.IsFalse(Object.ReferenceEquals(decksTuple.Item1, decksTuple.Item2));
            }
        }
  
        [TestMethod]
        public void TestSetDeck()
        {
            DeckOfCards deck = new DeckOfCards();
            List<Card> cards = new List<Card>();
            Card[] threeCards = { new Card(Suit.Spades, FaceValue.Two), 
                                  new Card(Suit.Spades, FaceValue.Three), 
                                  new Card(Suit.Spades, FaceValue.Four) };

            cards.Add(new Card(Suit.Spades, FaceValue.Two));
            cards.Add(new Card(Suit.Spades, FaceValue.Three));
            cards.Add(new Card(Suit.Spades, FaceValue.Four));

            deck.SetDeck(cards);
            IEnumerable<Tuple<Card, Card>> deckAndTestCardListMultpleEnumerator =
            MultipleIterate.Over(deck.GetCardEnumerator(), threeCards);
            foreach (var decksTuple in deckAndTestCardListMultpleEnumerator)
            {
                Assert.AreEqual(decksTuple.Item1, decksTuple.Item2);
            }
                
           
        }

        /// <summary>
        /// See https://blog.codinghorror.com/the-danger-of-naivete/
        /// For rational for using this algorithm        
        /// Testing if a pack of cards is randomised is what is needed but this is quite involved.
        /// One possiblilty is to create a counter for each of the different permutations of a three pack deck
        /// You will then  run this maybe 10000. 
        /// You will then use some criteria to decide that the result is a good distribtion
        /// This resource has some information
        ///https://math.stackexchange.com/questions/1189519/chi-square-goodness-of-fit-test-for-uniform-distribution-using-matlab
        ///https://msdn.microsoft.com/en-us/magazine/mt795190.aspx
        /// Has a chi-squared test code which could be used. Right now I think this is overkill but if 
        /// we were going to use this on a poker site then we would need to get to work here. 
        /// I would also talk to an old friend, Antranig Baseman, who has a PhD in statistics (or someone else with similar skills). 
        /// </summary>
        [TestMethod]
        public void TestDeckOfCardsShuffle()
        {
            DeckOfCards deck = new DeckOfCards();
            List<Card> cards = new List<Card>();
            cards.Add(new Card(Suit.Spades, FaceValue.Two));
            cards.Add(new Card(Suit.Spades, FaceValue.Three));
            cards.Add(new Card(Suit.Spades, FaceValue.Four));
            deck.SetDeck(cards);
            Random anotherRandom = new Random();  
            List<List<Card>> binOfDecks = new List<List<Card>>();
            List<Card> temp = new List<Card>();

            int TWO_THREE_FOUR = 0;
            int TWO_FOUR_THREE = 0;
            int THREE_TWO_FOUR = 0;
            int THREE_FOUR_TWO = 0;
            int FOUR_TWO_FOUR = 0;
            int FOUR_THREE_FOUR = 0;
            for (int shuffleOverload = 0; shuffleOverload < 3; shuffleOverload++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    switch (shuffleOverload)
                    {
                        case 0:
                            deck.Shuffle();
                            break;
                        case 1:
                            deck.Shuffle(cards);
                            break;
                        case 2:
                            deck.Shuffle(cards, anotherRandom);
                            break;
                        default:
                            throw new IndexOutOfRangeException("Shuffle only has three overloads but the shuffleOverload index is not 0,1 or 2");
                    }
                    temp = deck.cloneDeck();
                    //each permumtation
                    Card[] twoThreeFour = new Card[] { new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Four) };
                    Card[] twoFourThree = new Card[] { new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Three) };
                    Card[] threeTwoFour = new Card[] { new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Four) };
                    Card[] threeFourTwo = new Card[] { new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Two) };
                    Card[] fourTwoThree = new Card[] { new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Three) };
                    Card[] fourThreeFour = new Card[] { new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Two) };

                    //bins
                    TWO_THREE_FOUR += temp.SequenceEqual(twoThreeFour) == true ? 1 : 0;
                    TWO_FOUR_THREE += temp.SequenceEqual(twoFourThree) == true ? 1 : 0;
                    THREE_TWO_FOUR += temp.SequenceEqual(threeTwoFour) == true ? 1 : 0;
                    THREE_FOUR_TWO += temp.SequenceEqual(threeFourTwo) == true ? 1 : 0;
                    FOUR_TWO_FOUR += temp.SequenceEqual(fourTwoThree) == true ? 1 : 0;
                    FOUR_THREE_FOUR += temp.SequenceEqual(fourThreeFour) == true ? 1 : 0;

            }
                ///TODO find a good way to test randomness. This is not a good way. See commments above this function.
                Assert.IsTrue((TWO_THREE_FOUR < 333) && (TWO_THREE_FOUR > 88));
                Assert.IsTrue((TWO_FOUR_THREE < 333) && (TWO_FOUR_THREE > 88));
                Assert.IsTrue((THREE_TWO_FOUR < 333) && (THREE_TWO_FOUR > 88));
                Assert.IsTrue((THREE_FOUR_TWO < 333) && (THREE_FOUR_TWO > 88));
                Assert.IsTrue((FOUR_TWO_FOUR < 333) && (FOUR_TWO_FOUR > 88));
                Assert.IsTrue((FOUR_THREE_FOUR < 333) && (FOUR_THREE_FOUR > 88));

                TWO_THREE_FOUR = 0;
                TWO_FOUR_THREE = 0;
                THREE_TWO_FOUR = 0;
                THREE_FOUR_TWO = 0;
                FOUR_TWO_FOUR = 0;
                FOUR_THREE_FOUR = 0;
                ///TODO go here and read and finish data base connection
                /* https://msdn.microsoft.com/en-us/library/bb384428.aspx
                using (var db = new TestShuffleResults())
                 {
                     db.(new TestDeckOfCardsShuffleUnitTestResult 
                    { tWO_THREE_FOUR = TWO_THREE_FOUR, tWO_FOUR_THREE  = TWO_FOUR_THREE , tHREE_TWO_FOUR = THREE_TWO_FOUR, 
                        tHREE_FOUR_TWO = THREE_TWO_FOUR, fOUR_TWO_FOUR = FOUR_TWO_FOUR, fOUR_THREE_FOUR = FOUR_THREE_FOUR });
                     db.SaveChanges();
              
            
            */
            }

        }
    }
}
