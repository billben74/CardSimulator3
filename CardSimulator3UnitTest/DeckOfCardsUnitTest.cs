using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CardSimulator3ClassLibrary;

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
        /// Test that getNextCard produce a standard List<Cards> by testing against this unit tests array
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
                //Assert.AreEqual()
                ///TODO finish this test. Check each card is correct.
                ///Then check randomise works. Not sure if its worth being totally 
                ///rigerous about distribution 
            }
            Assert.AreEqual(index, 52); // correct number of cards.
        }

        /// <summary>
        /// Test that the overload of makeStandardListOfCards that makes a new List<Card>
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
        /// 
        /// </summary>
        [TestMethod]
        public void TestDifferenceBetweenBothOverloadsOfmakeStandardListOfCards() 
        {

        }



        /// <summary>
        /// Test that the overload of makeStandardListOfCards that takes an already made List<Card> 
        /// will produce a standard 52 deck of cards in correct order.
        /// </summary>
        [TestMethod]
        public void TestmakeStandardListOfCardsWithListofCardsArgumentParameter()
        {
            List<Card> testCards = new List<Card>();
            int index = 0;
            testCards = DeckOfCards.makeStandardListOfCards(testCards);
            foreach (Card c in testCards)
            {
                Assert.AreEqual(this.completeDeck[index], testCards[index]);
                index++;
            }
        }

        [TestMethod]
        public void TestRandomiseCards()
        {
            int index = 0;
            
        }
    }
}
