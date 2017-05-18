/* BLACKJACK V5
 * MAY 2017
 * Copyright (C) Gerardo Vela (DEYOX). All rights reserved
 * 
 * DECK CLASS:
 * - Creates 52 Card objects and adds them into an array of cards.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ5
{
    public class Deck
    {
        Card[] deck;
        int cardsUsed;
        /* constructor to initialize the fields */
        public Deck() 
        {
            deck = new Card[52];
            int cardCounter = 0;

            for (int suit = 0; suit <= 3; suit++)
            {
                for (int i = 1; i <= 13; i++)
                {
                    deck[cardCounter] = new Card(i, suit);
                    cardCounter++;
                }
            }
            cardsUsed = 0;
        }

        /* Shuffles card objects */
        public void shuffle() 
        {
            Random rand = new Random();
            for (int i = 51; i > 0; i--)
            {
                int random = rand.Next(i + 1);
                Card temp = deck[i];
                deck[i] = deck[random];
                deck[random] = temp;
            }
            cardsUsed = 0;
        }
        /* Gets the total amount of cards remaining*/
        public int cardsRemaining()
        {
            return 52 - cardsUsed;
        }
        /* Deals a card to add to the hand */
        public Card dealCard()
        {
            if (cardsUsed == 52)
                shuffle();
            cardsUsed++;
            return deck[cardsUsed - 1];
        }
    }
}
