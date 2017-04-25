using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CardSimulator3ClassLibrary;
using CardSimUnitTests;

namespace CardSimulator3UnitTest
{

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

        /// <summary>
        /// Test that getNextCard produces a standard List<Cards> by testing against this unit tests array
        /// of standard cards.
        /// Also test 52 cards.
        /// </summary>
        [TestMethod]
        public void TestDeckOfCardsGetNextCard ()
        {
            int index = 0;
            DeckOfCards deck = new DeckOfCards();
            deck.InitialiseCards();
            while (deck.hasCards())
            {
                Card c = deck.getNextCard();
                Assert.AreEqual(completeDeck[index++], c);
            }
            Assert.AreEqual(index, 52); // correct number of cards.
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
        /// The Listof cardss made by cloneDeck and the private field DeckOfCards.cardDeck should be different references.
        /// <summary>
        /// <remarks>
        /// I realise that testing private methods or fields is on of those contraversal issues that programmers like to argue over
        /// I would simply like to make sure that the private field cardDeck is a different reference as the cloneDeck 
        /// as this is the whole point of cloneDeck. I don't won't to make it protected and create a wrapper class....
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
            //But that the references are different.
            foreach (Card c in clonedDeck) 
            {
                Card getNextCard = deck.getNextCard();
                Assert.AreEqual(getNextCard, c);

                Assert.IsFalse(Object.ReferenceEquals(getNextCard, c));
            }
            
            
        }


        /// <summary>
        /// Test SetDeck
        /// </summary>
        [TestMethod]
        public void TestSetDeck()
        {
            DeckOfCards deck = new DeckOfCards();
            List<Card> cards = new List<Card>();
            Card[] threeCards = { new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Four) };
            cards.Add(new Card(Suit.Spades, FaceValue.Two));
            cards.Add(new Card(Suit.Spades, FaceValue.Three));
            cards.Add(new Card(Suit.Spades, FaceValue.Four));
            deck.SetDeck(cards);
            for (int i = 0; i < threeCards.Length; i++) 
            {
                Assert.AreEqual(threeCards[i], deck.getNextCard());
            }

        }

        /// <summary>
        /// Testing if a pack of cards is randomised.
        /// You are trying to create a counter for each of the different permutations of a three pack deck
        /// You will then  run this maybe 10000. 
        /// You will then use some criteria to decide that the result is a good distribtion
        /// Perhaps you might reearch more on how to decide if a distribution is good.
        /// THe sequenceEquals doesn't seem to work so look to that first
        /// YOu may want to research more 
        /// See https://blog.codinghorror.com/the-danger-of-naivete/
        /// </summary>
        [TestMethod]
        public void TestDeckOfCardsShuffle()
        {
            DeckOfCards deck = new DeckOfCards();
            List<Card> cards = new List<Card>()
            cards.Add(new Card(Suit.Spades, FaceValue.Two));
            cards.Add(new Card(Suit.Spades, FaceValue.Three));
            cards.Add(new Card(Suit.Spades, FaceValue.Four));
            deck.SetDeck(cards);

            List<List<Card>> binOfDecks = new List<List<Card>>();
            List<Card> temp = new List<Card>();

            int TWO_THREE_FOUR = 0;
            int TWO_FOUR_THREE = 0;
            int THREE_TWO_FOUR = 0;
            int THREE_FOUR_TWO = 0;
            int FOUR_TWO_FOUR = 0;
            int FOUR_THREE_FOUR = 0;

            for (int i = 0; i < 1000; i++) 
            {

                deck.RandomiseCards();
                temp = deck.cloneDeck();
                
                //each permumtation
                Card [] twoThreeFour = new Card[]  {new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Four)};
                Card [] twoFourThree = new Card[]  {new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Three)};
                Card [] threeTwoFour = new Card[]  {new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Four)};
                Card [] threeFourTwo = new Card[]  {new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Two)};
                Card [] fourTwoThree = new Card[]  {new Card(Suit.Spades, FaceValue.Four), new Card(Suit.Spades, FaceValue.Two), new Card(Suit.Spades, FaceValue.Three)};
                Card [] fourThreeFour = new Card[]  {new Card(Suit.Spades, FaceValue.Four),  new Card(Suit.Spades, FaceValue.Three), new Card(Suit.Spades, FaceValue.Two)};

                //bins
                TWO_THREE_FOUR += temp.SequenceEqual(twoThreeFour) == true ? 1 : 0;
                TWO_FOUR_THREE += temp.SequenceEqual(twoFourThree) == true ? 1 : 0;
                THREE_TWO_FOUR += temp.SequenceEqual(threeTwoFour) == true ? 1 : 0;
                THREE_FOUR_TWO += temp.SequenceEqual(threeFourTwo) == true ? 1 : 0;
                FOUR_TWO_FOUR += temp.SequenceEqual(fourTwoThree) == true ? 1 : 0;
                FOUR_THREE_FOUR += temp.SequenceEqual(fourThreeFour) == true ? 1 : 0;

              
            
            }
            int look = 1;


        }



        [TestMethod]
        public void TestRandomiseCards()
        {
            int index = 0;
            
        }
    }
}
