using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardSimulator3ClassLibrary;

namespace CardSimulator3UnitTest
{

    


    [TestClass]
    public class DeckOfCardsUnitTest
    {

        Card[] completeDeck;

        /// <summary>
        /// Although Deck of cards has utilty to make standard lists of cards
        /// I am independently making a standard array of cards so that 
        /// these tests do not rest on DeckOfCards, which I mean to test here.
        /// </summary>
        [TestInitialize]
        public void TestInitialize ()
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
        public void TestDeckOfCardsGetNextCard ()
        {
            DeckOfCards deck = new DeckOfCards();
            deck.InitialiseCards();
            while (deck.hasCards())
            {
                int index = 0;
                Card c = deck.getNextCard();
                Assert.AreEqual(completeDeck[index++], c);
                //Assert.AreEqual()
                ///TODO finish this test. Check each card is correct.
                ///Then check randomise works. Not sure if its worth being totally 
                ///rigerous about distribution 
            }
        }
    }
}
