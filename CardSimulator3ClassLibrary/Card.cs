using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardSimulator3ClassLibrary
{
    /// <summary>
    /// Represent face value of playing card. Ace is high.
    /// </summary>
    public enum FaceValue 
    {
        NoFaceValue = 0,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    /// <summary>
    /// Represents suit of a playing card. Uses traditional ranking found in bridge bidding
    /// </summary>
    public enum Suit
    {
        NoSuit = 0,
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3,
        Spades = 4
    }

    /// <summary>
    /// Represents a suit color.
    /// </summary>
    public enum SuitColor
    {
        NoColor = 0,
        Black = 1,
        Red = 2
    }

    /// <summary>
    /// Represents a playing card
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Make a default card with NoSuit and NoFaceValue
        /// </summary>
        public Card()
        {
            this.suit = Suit.NoSuit;
            this.faceValue = FaceValue.NoFaceValue;
        }

        /// <summary>
        /// Make a new instance of a card by copying the values of another card
        /// </summary>
        /// <param name="c"></param>
        public Card(Card otherCard) 
        {
            this.suit = otherCard.suit;
            this.faceValue = otherCard.faceValue;
        }

        /// <summary>
        /// Make a card using Suit and FaceValues
        /// </summary>
        /// <param name="suit"></param>
        /// <param name="fv"></param>
        public Card(Suit suit, FaceValue fv)
        {
            this.suit = suit;
            this.faceValue = fv;
        }

        /// <summary>
        /// The playing card's suit
        /// </summary>
        public Suit suit { get; set; }
        /// <summary>
        /// The playing card's face value
        /// </summary>
        public FaceValue faceValue { get; set; }
        /// <summary>
        /// The suit colour of the playing card. No color is returned for no suit.
        /// </summary>
        public SuitColor suitColor
        {
            get
            {
                if (suit == Suit.NoSuit)
                {
                    return SuitColor.NoColor;
                }
                return suit == Suit.Hearts || suit == Suit.Diamonds ? SuitColor.Red : SuitColor.Black;
            }
        }

        /// <summary>
        /// Equals method that takes a Card as rhs.
        /// </summary>
        /// <param name="rhs">Afer checking that rhs is not null this is used as rhs</param>
        /// <returns>true if FaceValue and Suit are the same for both objects, this includes NoSuit and NoFaceValue, false otherwise</returns>
        public bool Equals(Card rhs)
        {
            if (Object.ReferenceEquals(rhs, null))
            {
                return false;
            }
            if (this.suit == rhs.suit && this.faceValue == rhs.faceValue)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Override of Object Equals method. 
        /// Use Card's member version of Equals that uses a Card
        /// </summary>
        /// <param name="obj">Check not null and use GetType to check its a Card</param>
        /// <returns>true if FaceValue and Suit are the same for both objects, this includes NoSuit and NoFaceValue, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null) && GetType() != obj.GetType())
                return false;
            Card rhs = (Card)obj;
            return this.Equals(rhs);
        }

        /// <summary>
        /// Simple GetHashCode implementation
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode>"/>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = (int)suit * 23 + suit.GetHashCode();
            hash = (int)faceValue * 23 + faceValue.GetHashCode();
            return hash;
        }

        /// <summary>
        /// After checking that both Cards are not null, the method uses Equals method
        /// </summary>
        /// <param name="lhs">Left hand Card to check if equal</param>
        /// <param name="rhs">Right hand Card to check if equal</param>
        /// <returns>true if FaceValue and Suit are the same for both objects, this includes NoSuit and NoFaceValue, false otherwise</returns>
        public static bool operator ==(Card lhs, Card rhs)
        {
            if (Object.ReferenceEquals(lhs, null) || Object.ReferenceEquals(rhs, null))
            {
                return false;
            }
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// We must overload != as well as ==
        /// Simply returns not the value of operator==
        /// </summary>  
        /// <param name="lhs">Left hand Card to check if not equal</param>
        /// <param name="rhs">Right hand Card to check if not equal</param>
        /// <returns>false if FaceValue and Suit are the same for both objects, this includes NoSuit and NoFaceValue, true otherwise</returns>
        public static bool operator !=(Card lhs, Card rhs)
        {
            return !(lhs == rhs);
        }

    }
}
 