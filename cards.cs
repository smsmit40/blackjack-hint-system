/* BLACKJACK V5
 * MAY 2017
 * Copyright (C) Gerardo Vela (DEYOX). All rights reserved
 * 
 * CARD CLASS:
 * - Assigns values to each card and combines them to create a Card object.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ5
{
    public class Card
    {
        public static int SPADES = 0;
        public static int HEARTS = 1;
        public static int DIAMONDS = 2;
        public static int CLUBS = 3;

        public static int ACE = 1;
        public static int JACK = 10; //11
        public static int QUEEN = 10; //12
        public static int KING = 10; //13

        private int suit;
        private int value;
        
        /* Constructor to initialize values of card*/
        public Card(int newValue, int newSuit)
        {
            value = newValue;
            suit = newSuit;
        }
        /* Getter to return suit of card */
        public int getSuit()
        {
            return suit;
        }
        /* Getter to return value of card */
        public int getValue()
        {
            return value;
        }
        /* toString to combine suits and values */
        public string getSuitString()
        {

            if (suit == SPADES)
                return "Spades";
            else if (suit == HEARTS)
                return "Hearts";
            else if (suit == DIAMONDS)
                return "Diamonds";
            else if (suit == CLUBS)
                return "Clubs";
            return "--";
        }
        /* toString that clasifies Ranks of each card */
        public string getRankString()
        {
            switch (value)
            {
                case 1: return "Ace";
                case 2: return "2";
                case 3: return "3";
                case 4: return "4";
                case 5: return "5";
                case 6: return "6";
                case 7: return "7";
                case 8: return "8";
                case 9: return "9";
                case 10: return "10";
                case 11: return "Jack";
                case 12: return "Queen";
                case 13: return "King";
                default: return "unknown";
            }
        }
        /* toString that retuns name of card */
        public string toStringCard()
        {
            return "_"+ getRankString() + "_of_" + getSuitString();
        }
        /* toString that returns the string rank UNUSED */
        public string toStringRank()
        {
            return getRankString();
        }
    }
}
