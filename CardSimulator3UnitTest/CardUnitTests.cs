using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardSimulator3ClassLibrary;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace CardSimUnitTests
{
    /// <summary>
    /// Class for the unit tests of the Card Sim
    /// </summary>
    [TestClass]
    public class CardUnitTests
    {
        /// <summary>
        /// Test each Card face value by iterating over each possible FaceValue
        /// </summary>
        [TestMethod]
        public void TestCardFaceValues()
        {
            Card card = new Card();
            foreach (FaceValue fv in Enum.GetValues(typeof(FaceValue)))
            {
                card.faceValue = fv;
                Assert.AreEqual(fv,card.faceValue);
            }            
        }

        /// <summary>
        /// Tests that each card has expected Suit when Suit is set
        /// Iterates of each suit including no suit
        /// </summary>
        [TestMethod]
        public void TestCardSuits()
        {
            Card card = new Card();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                card.suit = suit;
                Assert.AreEqual(suit, card.suit);
            }
        }

        /// <summary>
        /// Small utility to make the TestSuitColor more DRY
        /// Iterates over each face value to test if each suit type evaluates it color correctly.
        /// </summary>
        /// <param name="card">The Card which should have its Suit type set.</param>
        /// <param name="color">The suitColor that the Suit should evaluate to. </param>
        public void FaceValueEnumerationTestSuitColorUtility(Card card,  SuitColor color)
        {
            foreach (FaceValue fv in Enum.GetValues(typeof(FaceValue)))
            {
                card.faceValue = fv;
                Assert.AreEqual(color, card.suitColor);
            }
        }

        /// <summary>
        /// This will test to see of each of the Suit types will correctly evalutate to correct color.
        /// Spades, clubs to black. Hearts, diamonds evaluate to red. No suit should evaluate to no color. 
        /// </summary>
        [TestMethod]
        public void TestSuitColor()
        {
            Card card = new Card();

            // null suits are black
            FaceValueEnumerationTestSuitColorUtility(card, SuitColor.NoColor);

            card.suit = Suit.Clubs;
            FaceValueEnumerationTestSuitColorUtility(card, SuitColor.Black);
            
            card.suit = Suit.Spades;
            FaceValueEnumerationTestSuitColorUtility(card, SuitColor.Black);
            
            card.suit = Suit.Diamonds;
            FaceValueEnumerationTestSuitColorUtility(card, SuitColor.Red);
            
            card.suit = Suit.Hearts;
            FaceValueEnumerationTestSuitColorUtility(card, SuitColor.Red);
        }


        /// <summary>
        /// Test the Equals override that takes a card as a paramter 
        /// </summary>
        /// <remarks>
        /// Stratergy Pattern used to make unit tests more Don't Repeat Yourself
        /// </remarks>
        [TestMethod]
        public void TestOverrideEqualsCardBased()
        {

            Action<Card, Card> cardEqualsShouldGiveFalse = delegate(Card arg1, Card arg2) { Assert.IsFalse(arg1.Equals(arg2)); };
            UtililtyTestingAllDifferentNonNullCardPairsDelegate(cardEqualsShouldGiveFalse);

            Action<Card, Card> cardEqualsDelegate = delegate(Card arg1, Card arg2) { Assert.IsTrue(arg1.Equals(arg2)); };
            UtilityTestingAllPossibleIdenticalPairsOfCards(cardEqualsDelegate);

        }

        /// <summary>
        /// Test that the overriden base class (object) Equals(object obj) 
        /// will correctly return true for all possible values of two (equal) non-null Cards
        /// Will also test if null values give false as although we could do this in say operator !=
        /// which at present uses equals. 
        /// It seems better to test null giving false in this test method not assuming coverage by other test methods
        /// in case someone changed the implementation/connection between methods. 
        /// </summary>
        /// <see>some advice taken from http://codinghelmet.com/?path=howto/testing-equals-and-gethashcode </see>
        /// <remarks>Written before the utility functions. As this is the Equals that other methods test I decided to leave it
        /// as is. Perhaps testing via a slightly different procedure may guard and systematic errors in the testing methods; 
        /// that is perhaps if changes cause problems having different testing implementations is a defence in depth idea. Perhaps...</remarks>
        [TestMethod]
        public void TestOverrideEqualsObjectBased() 
        {
            Card lhs = new Card();
            Card rhs = null;
            Assert.IsFalse(lhs.Equals(rhs));
        
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach(FaceValue fv in Enum.GetValues(typeof(FaceValue))) 
                {
                    ///check via contructor
                    lhs = new Card(suit, fv);
                    rhs = new Card(suit, fv);
                    Assert.IsTrue(lhs.Equals((Object)rhs));
                    // check via constructor and member assignment mix
                    lhs = new Card(suit, fv);
                    rhs = new Card();
                    rhs.suit = suit;
                    rhs.faceValue = fv; 
                    Assert.IsTrue(lhs.Equals((Object)rhs));
                    //check via member assignment in both cases.
                    lhs = new Card();
                    rhs = new Card();
                    lhs.suit = suit;
                    lhs.faceValue = fv;
                    rhs.suit = suit;
                    rhs.faceValue = fv;
                    Assert.IsTrue(lhs.Equals((Object)rhs));                   
                }
            }
        }

        /// <summary>
        /// Utility that can iterate over forward and reverse cards.
        /// This will ignore similar cards as happens when they forward and reverse iterations collide.
        /// This uses a delegate that has two cards as paramaters to actually carry out the test.
        /// </summary>
        /// <remarks>
        /// Stratergy Pattern used to make unit tests more Don't Repeat Yourself
        /// </remarks>
        /// <param name="testingDelegate">non returning delagate that has two Cards and arguments to be used to carry out the specific test</param>
        [TestMethod]
        public static void UtililtyTestingAllDifferentNonNullCardPairsDelegate(Action<Card,Card> testingDelegate)
        {

            ///todo change cast to normal
            //to use the Linq extention method Reverse() to generate a reverse iterator we must cast the Array
            //created by Enum.GetValues to IEnumerable<Suit> 
            foreach (Suit suitReverse in ((IEnumerable<Suit>) Enum.GetValues(typeof(Suit))).Reverse())
            {
                //to use the Linq extention method Reverse() to generate a reverse iterator we must cast the Array
                //created by Enum.GetValues to IEnumerable<FaceValue> 
                foreach (FaceValue fvReverse in ((IEnumerable<FaceValue>) Enum.GetValues(typeof(FaceValue))).Reverse())
                {
                    foreach (Suit suitForward in Enum.GetValues(typeof(Suit)))
                    {
                        foreach (FaceValue fvForward in Enum.GetValues(typeof(FaceValue)))
                        {
                            if (suitReverse != suitForward && fvReverse != fvForward) //ignore cards with similar values
                            {
                                Card reverseOrder = new Card(suitReverse, fvReverse);
                                Card forwardOrder = new Card(suitForward, fvForward);

                                testingDelegate(reverseOrder, forwardOrder);
                            }
                        }
                    }
 
                }            
             }
         }


        /// <summary>
        /// Utility that can create all possible identical pairs of cards.
        /// This uses a delegate that has two cards as paramaters to actually carry out the test.
        /// </summary>
        /// <remarks>
        /// Stratergy Pattern used to make unit tests more Don't Repeat Yourself
        /// </remarks>
        [TestMethod]
        public static void UtilityTestingAllPossibleIdenticalPairsOfCards(Action<Card, Card> testingDelegate) 
        {
             foreach (Suit suitForward in Enum.GetValues(typeof(Suit)))
             {
                 foreach (FaceValue fvForward in Enum.GetValues(typeof(FaceValue)))
                 {
                     Card card1 = new Card(suitForward, fvForward);
                     Card card2 = new Card(suitForward, fvForward);
                     testingDelegate(card1, card2);
                 }      
            }

        }


        /// <summary>
        /// Test to see if equality operator will correctly identify all similar cards as equal
        /// Also tests that any null cards will cause != to return
        /// </summary>
        /// <remarks>
        /// Stratergy Pattern used to make unit tests more Don't Repeat Yourself
        /// </remarks>
        [TestMethod]
        public void TestEqualityOperator()
        {
            Card lhs = null;
            Card rhs = null;
            Assert.IsFalse(lhs == rhs);
            lhs = new Card();
            Assert.IsFalse(lhs == rhs);
            lhs = null;
            rhs = new Card();
            Assert.IsFalse(lhs == rhs);

            Action<Card, Card> EqualityDelegateTrue = delegate(Card arg1, Card arg2) { Assert.IsTrue(arg1 == arg2); };
            UtilityTestingAllPossibleIdenticalPairsOfCards(EqualityDelegateTrue);

            Action<Card, Card> EqualityDelegateFalse = delegate(Card arg1, Card arg2) { Assert.IsFalse(arg1 == arg2); };
            UtililtyTestingAllDifferentNonNullCardPairsDelegate(EqualityDelegateFalse);


        }

        /// <summary>
        /// Test to see if inequality operator will correctly identify all dissimilar cards as unequal
        /// and that all similar cards give false with != operator
        /// Also tests that any null cards will cause != to return
        /// </summary>
        /// <remarks>
        /// Stratergy Pattern used to make unit tests more Don't Repeat Yourself
        /// </remarks>
        [TestMethod]
        public void TestInEqualityOperator() 
        {

            Card lhs = null;
            Card rhs = null;
            Assert.IsTrue(lhs != rhs);
            lhs = new Card();
            Assert.IsTrue(lhs != rhs);
            lhs = null;
            rhs = new Card();
            Assert.IsTrue(lhs != rhs);

            Action<Card, Card> inEqualityDelegate = delegate(Card arg1, Card arg2) { Assert.IsTrue(arg1 != arg2); };

            UtililtyTestingAllDifferentNonNullCardPairsDelegate(inEqualityDelegate);
            Action<Card, Card> EqualityDelegate = delegate(Card arg1, Card arg2) { Assert.IsFalse(arg1 != arg2); };

            UtilityTestingAllPossibleIdenticalPairsOfCards(EqualityDelegate);
          
        }

        /// <summary>
        /// Test that identically valued cards give the same hash code.
        /// </summary>
             /// <remarks>
        /// Stratergy Pattern used to make unit tests more Don't Repeat Yourself
        /// </remarks>
        [TestMethod]
        public void TestGetHashCode()
        {       
            Action<Card, Card> hashCodeLogicNegativeDelegate 
                = delegate (Card arg1, Card arg2) { Assert.IsTrue(arg1.GetHashCode() != arg2.GetHashCode());};

            UtililtyTestingAllDifferentNonNullCardPairsDelegate(hashCodeLogicNegativeDelegate);

            Action<Card, Card> hashCodeLogicPositiveResult
                = delegate(Card arg1, Card arg2) { Assert.IsTrue(arg1.GetHashCode() == arg2.GetHashCode()); };
            UtilityTestingAllPossibleIdenticalPairsOfCards(hashCodeLogicPositiveResult);
        }
         
    }
    
}
