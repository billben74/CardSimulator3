using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardSimulator3ClassLibrary;

namespace CardSimulator3UnitTest
{
    [TestClass]
    public class DeckOfCardsUnitTest
    {        
        [TestMethod]
        public void TestDeckOfCardsGetNextCard ()
        {
            DeckOfCards deck = new DeckOfCards();
           
            while (deck.hasCards())
            {                
                Card c = deck.getNextCard();
                ///TODO finish this test. Check each card is correct.
                ///Then check randomise works. Not sure if its worth being totally 
                ///rigerous about distribution 
            }
        }
    }
}
