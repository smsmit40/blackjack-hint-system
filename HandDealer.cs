/* BLACKJACK V5
 * MAY 2017
 * Copyright (C) Gerardo Vela (DEYOX). All rights reserved
 * 
 * HANDDEALER CLASS:
 * - Creates a List of Cards
 * - Uses List methods to clear, add, remove, and count elements inside the List of Cards.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ5
{
    public class HandDealer
    {
        static List<Card> hand; 

        /* Constructor to initialize hand*/
        public HandDealer()
        {
            hand = new List<Card>();
        }

        /* Removes all elements in the List */
        public void clear()
        {
            hand.Clear(); // RemoveAll
        }
        /* Adds a card to the List(hand) */
        public void addCard(Card card)
        {
            if (card != null)
            {
                hand.Add(card);
            }
        }
        /* Removes an element of the List */
        public void removeCard(Card card)
        {
            hand.Remove(card);
        }
        /* Remove a card at a given position */
        public void removeCard(int position)
        {
            if (position >= 0 && position < hand.Count()) // hand.size
                hand.RemoveAt(position);
        }

        /* Counter of size in hand */
        public static int getCounter()
        {
            return hand.Count();
        }
        /* Gets card at a given position */
        public Card getCard(int position)
        {
            if (position >= 0 && position < hand.Count())
                return (Card)hand.ElementAt(position);
            else
                return null;
        }

        /* Get value of cards in hand */
        public int getValue()
        {
            int value = 0;
            bool ace = false;
            int cards = getCounter(); // getCounter is static

            for (int i = 0; i < cards; i++)
            {
                Card card;
                int cardValue;
                card = getCard(i);
                cardValue = card.getValue();

                if (cardValue > 10)
                {
                    cardValue = 10;
                }
                if (cardValue == 1)
                {
                    ace = true;
                }
                value = value + cardValue;
            }
            if (ace == true && value + 10 <= 21)
                value = value + 10;

            return value;
        }

        /* Use te set value of hand back to zero UNUSED*/
        public int setToZero(int value)
        {
            value -= value; // must be 0 again

            return value;
        }
    }
}
