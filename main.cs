/* BLACKJACK Version 5
 * MAY 2017
 * Copyright (C) Gerardo Vela (DEYOX). All rights reserved
 * All Sound Effects are used under the Fair Use Doctrine in the US Copyright Law.
 * Nintendo holds the copyrights of the Sound Effects.
 * MAIN CLASS:
 *  - Loads table and chips labels.
 *  - Player bets chips and deals to start the game.
 *  - If player has a value greater than the dealer, he will win the round. Otherwise, the dealer will win.
 *  - Game ends when player has no more chips or when player has reached the total amount of available chips.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Timers;
using System.Threading;
using System.Web;

namespace BJ5
{
    public partial class main : Form 
    {
        Deck deck = new Deck();
        HandPlayer handPlayer;
        HandDealer handDealer;
        Card card = null;
        int playerCounter, coinsTotal, bet, surrenderCounter, dealerCounter;

        System.Media.SoundPlayer shuffle = new System.Media.SoundPlayer("Resources/shuffle.wav"); // shuffle sound
        System.Media.SoundPlayer placeCard = new System.Media.SoundPlayer("Resources/cardPlace4.wav"); // placeCard sound
        System.Media.SoundPlayer betChip = new System.Media.SoundPlayer("Resources/chipsStack1.wav"); // betChip sound
        System.Media.SoundPlayer getChips = new System.Media.SoundPlayer("Resources/chipsHandle6.wav"); // getChips sound
        System.Media.SoundPlayer pause = new System.Media.SoundPlayer("Resources/pause.wav"); // rules sound
        System.Media.SoundPlayer quit = new System.Media.SoundPlayer("Resources/quit.wav"); // rules sound
        System.Media.SoundPlayer newGame = new System.Media.SoundPlayer("Resources/newGame.wav"); // newGame sound
        System.Media.SoundPlayer lose = new System.Media.SoundPlayer("Resources/lose.wav"); // lose round sound
        System.Media.SoundPlayer gameOver = new System.Media.SoundPlayer("Resources/gameOver.wav"); // gameOver sound
        System.Media.SoundPlayer blackjack = new System.Media.SoundPlayer("Resources/blackjack.wav"); // blackjack sound
        System.Media.SoundPlayer win = new System.Media.SoundPlayer("Resources/win.wav"); // win sound
        System.Media.SoundPlayer winGame = new System.Media.SoundPlayer("Resources/winGame.wav"); // win/endgame sound
        System.Timers.Timer exitTimer = new System.Timers.Timer(); // timer to exit
        System.Timers.Timer dealerTimer = new System.Timers.Timer(); // timer between each draw 

        System.Threading.Timer timer;
        
        /* Constructor to initialize the fields*/
        public main()
        {
            InitializeComponent();
            // buttons
            dealButton.Visible = false; standButton.Enabled = false;
            hitButton.Enabled = false; doubleButton.Enabled = false;
            betButton.Enabled = false; surrender.Visible = false;
            // coins/chips
            whiteCoin.Enabled = true; blueCoin.Enabled = true;
            redCoin.Enabled = true; blackCoin.Enabled = true;
            greenCoin.Enabled = true;
            // player cards
            playerCard1.Visible = false; playerCard2.Visible = false;
            playerCard3.Visible = false; playerCard4.Visible = false;
            playerCard5.Visible = false; dealerCard1.Visible = false;
             // dealer cards
            dealerCard2.Visible = false; dealerCard3.Visible = false;
            dealerCard4.Visible = false; dealerCard5.Visible = false;
            dealerLabel.Visible = false;
           
            playerLabel.Visible = false;
            // value of cards
            handPlayer = new HandPlayer(); 
            handDealer = new HandDealer();
            playerCounter = 0; // counter to draw more cards 
            coinsTotal = 1500;
            bet = 0; surrenderCounter = 0;
            coinsLabel.Text = " = $" + coinsTotal.ToString();
            betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
            
            exitTimer.Elapsed += new ElapsedEventHandler(CloseEvent); // elapse time to exit application
            exitTimer.Interval = 1500; // set an interval
        }
        /* Start interval to exit the game */
        private void exitButton_Click(object sender, EventArgs e)
        {
            quit.Play(); // plays exit sound
          //  exitTimer.Interval = 1500; // set an interval
            exitTimer.Enabled = true; // start the timer
        }
        /* Exit the game when the Elapsed event is raised. */
        private void CloseEvent(object source, ElapsedEventArgs e)
        {
           // rules.Dispose(); 
            Application.Exit();
        }

        /* TimerEllapsed gives time to the dealer to draw a card if not disposed */
        private delegate void InvokeDelegate();
        private void OnTimerEllapsed(object sender)
        {
            dealerCounter++;
            this.BeginInvoke(new InvokeDelegate(playDealer));
        }

        /* Dealer adds one card to his hand
         * Each card's value is added to the total sum of cards
         */
        public void playDealer()
        {
            if (dealerCounter == 1) /* Dealer gets a 2nd card */
            {
                placeCard.Play();
                card = deck.dealCard();
                handDealer.addCard(card);
                dealerCard2.Visible = true;
                dealerCard2.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                valueDealer.Text = "Dealer's hand total: " + handDealer.getValue().ToString();
                checkDealersHand();
            }
            else if (dealerCounter == 2) /* Dealer gets a 3rd card */
            {
                placeCard.Play();
                card = deck.dealCard();
                dealerCard3.Visible = true;
                handDealer.addCard(card);
                dealerCard3.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                valueDealer.Text = "Dealer's hand total: " + handDealer.getValue().ToString();
                checkDealersHand();
            }
            else if (dealerCounter == 3) /* Dealer gets a 4th card */
            {
                placeCard.Play();
                card = deck.dealCard();
                dealerCard4.Visible = true;
                handDealer.addCard(card);
                dealerCard4.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                valueDealer.Text = "Dealer's hand total: " + handDealer.getValue().ToString();
                checkDealersHand();
            }
            else if (dealerCounter == 4) /* Dealer gets a 5th card */
            {
                placeCard.Play();
                card = deck.dealCard();
                dealerCard5.Visible = true;
                handDealer.addCard(card);
                dealerCard5.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                valueDealer.Text = "Dealer's hand total: " + handDealer.getValue().ToString();
                checkDealersHand();
            }
             
        }

        /* Checks the value of the dealer's hand */
        private void checkDealersHand()
        {
            if (handDealer.getValue() == handPlayer.getValue()) // push happens
            {
                timer.Dispose();
                resetTable();
                getChips.Play();
                coinsTotal = coinsTotal + bet;
                amountWin.Text = " + $" + bet;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                bet = 0;
                betLabel.Text = "Push! Click on chips to bet!";
                dealerEndGame.Text = "PUSH! YOU GET YOUR CHIPS BACK";
                dealerCounter = 0;
                dealerEndGame.Visible = true;
                amountWin.Visible = true;
            }
            if (handDealer.getValue() > 21) // dealer loses
            {
                if (handPlayer.getValue() != 21) // if not blackjack
                {
                    timer.Dispose();
                    win.Play();
                    resetTable();
                    coinsTotal = coinsTotal + bet + bet;
                    coinsLabel.Text = " = $" + coinsTotal.ToString();
                    amountWin.Text = " + $" + (bet * 2);
                    bet = 0;
                    dealerEndGame.Text = "DEALER BUSTED, HE LOSES!";
                    betLabel.Text = "You win the round! Click on chips to bet!";
                    dealerCounter = 0;
                    dealerEndGame.Visible = true;
                    amountWin.Visible = true;
                    game_over(); // checks if chips == 0 or chips > 2000
                }
                if (handPlayer.getValue() == 21) // if blackjack
                {
                    timer.Dispose();
                    win.Play();
                    resetTable();
                    amountWin.Text = " + $" + ((bet * 2) + (bet / 2));
                    coinsTotal = coinsTotal + bet + bet + (bet / 2);
                    coinsLabel.Text = " = $" + coinsTotal.ToString();
                    bet = 0;
                    dealerEndGame.Text = "DEALER BUSTED, HE LOSES!";
                    betLabel.Text = "You win the round! Click on chips to bet!";
                    dealerCounter = 0;
                    dealerEndGame.Visible = true;
                    amountWin.Visible = true;
                    game_over(); // checks if chips == 0 or chips > 2000
                }
            }
            if (handDealer.getValue() > handPlayer.getValue()) // dealer wins
            {
                timer.Dispose();
                resetTable();
                coinsTotal = coinsTotal + bet - bet;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                amountWin.Text = " - $" + bet;
                bet = 0;
                lose.Play();
                betLabel.Text = "You lose the round! Click on chips to bet!";
                dealerEndGame.Text = "DEALER WINS!";
                dealerCounter = 0;
                dealerEndGame.Visible = true;
                amountWin.Visible = true;
                game_over(); // checks if chips == 0 or chips > 2000
            }
            if (handDealer.getValue() >= 17) // dealer loses
            {
                if (handPlayer.getValue() != 21) // if not blackjack
                {
                    timer.Dispose();
                    win.Play();
                    resetTable();
                    coinsTotal = coinsTotal + bet + bet;
                    coinsLabel.Text = " = $" + coinsTotal.ToString();
                    amountWin.Text = " + $" + (bet * 2);
                    bet = 0;
                    dealerEndGame.Text = "DEALER STANDS, HE LOSES!";
                    betLabel.Text = "You win the round! Click on chips to bet!";
                    dealerCounter = 0;
                    dealerEndGame.Visible = true;
                    amountWin.Visible = true;
                    game_over(); // checks if chips == 0 or chips > 2000
                }
                if (handPlayer.getValue() == 21) // if blackjack
                {
                    timer.Dispose();
                    win.Play();
                    resetTable();
                    amountWin.Text = " + $" + ((bet * 2) + (bet / 2));
                    coinsTotal = coinsTotal + bet + bet +(bet/2);
                    coinsLabel.Text = " = $" + coinsTotal.ToString();
                    bet = 0;
                    dealerEndGame.Text = "DEALER STANDS, HE LOSES!";
                    betLabel.Text = "You win the round! Click on chips to bet!";
                    dealerCounter = 0;
                    dealerEndGame.Visible = true;
                    amountWin.Visible = true;
                    game_over(); // checks if chips == 0 or chips > 2000
                }
            }
        }
        /* Checks the value of the player's hand */
        private void checkPlayerHand()
        {
            if (handPlayer.getValue() > 21) // player loses
            {
                lose.Play();
                coinsTotal = coinsTotal + bet - bet;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                amountWin.Text = " - $" + bet;
                bet = 0;
                betLabel.Text = "You busted! Click on chips to bet!: $" + bet.ToString();
                endGame.Text = "YOU LOSE THE ROUND!";
                resetTable(); // resets cards and enables chips and buttons
                game_over(); // checks if chips == 0     
                endGame.Visible = true;
                amountWin.Visible = true;
            }
            else if (handPlayer.getValue() == 21)
            {
                endGame.Text = "You got Blackjack!!";
                blackjack.Play();
                endGame.Visible = true;
                hitButton.Enabled = false;
            }

            else
            {
                placeCard.Play();
            }
        }
        /* Resets labels and images on table for the next round*/
        private void resetTable()
        {
            whiteCoin.Enabled = true; redCoin.Enabled = true;
            blackCoin.Enabled = true; greenCoin.Enabled = true;
            blueCoin.Enabled = true;

            dealButton.Visible = false; betButton.Visible = true;
            betButton.Enabled = false; hitButton.Enabled = false;
            standButton.Enabled = false; amountWin.Visible = false;
          
            handPlayer = new HandPlayer(); // set values in hand back to zero
            handDealer = new HandDealer(); // set values in hand back to zero
            playerCounter = 0;
        }
        /* Identifies if player has no coind or if player has reached total amount of chips available */
        private void game_over()
        {
            if (coinsTotal == 0) // game over, no more chips
            {
                gameOver.Play();
                betLabel.Text = "You got no chips to bet, you lose!";
              //  gameOverLabel.Text = "GAME OVER, START A NEW GAME";
                endGame.Text = "GAME OVER! START A NEW GAME";
                gameOverLabel.Visible = true;
                endGame.Visible = true;
            }
            else if (coinsTotal > 100000) // player wins, can't carry more chips
            {
                winGame.Play();
                betLabel.Text = "You can't carry more chips, you win!";
                endGame.Text = "CONGRATULATIONS YOU WIN!, START A NEW GAME";
                gameOverLabel.Visible = true;
                endGame.Visible = true;
                whiteCoin.Enabled = false;
                blueCoin.Enabled = false;
                redCoin.Enabled = false;
                blackCoin.Enabled = false;
                greenCoin.Enabled = false;
            }
        }
        /* Starts game by shuffling the deck and giving 2 cards to player and 1 to dealer
         * If player gets a blackjack, he stand automatically 
         */
        private void startGame()
        {
            dealerCard1.Visible = true;
            dealerCard2.Visible = true;
            surrender.Visible = true;

            deck.shuffle(); // shuffles cards in deck
            /* Dealer gets a 1st card */
            card = deck.dealCard();
            handDealer.addCard(card);
            dealerCard1.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
            valueDealer.Text = "Dealer's hand total: " + handDealer.getValue().ToString();
            /* Player gets 1st card */
            card = deck.dealCard();
            handPlayer.addCard(card);
            playerCard1.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
            playerCard1.Visible = true;
            /* Player gets 2nd card */
            card = deck.dealCard();
            handPlayer.addCard(card);
            playerCard2.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
            playerCard2.Visible = true;

            valuePlayer.Text = "Player's hand total: " + handPlayer.getValue().ToString();

            if (handPlayer.getValue() == 21)
            {
                endGame.Text = "You got Blackjack!!";
                blackjack.Play();
                endGame.Visible = true;
                hitButton.Enabled = false;
                doubleButton.Enabled = false;
            }
            standDealer.Text = "Dealer must stand on 17";
        }
        /* Disable buttons so dealer can play */
        private void standButton_Click(object sender, EventArgs e)
        {    
            hitButton.Enabled = false; doubleButton.Enabled = false;
            standButton.Enabled = false; surrender.Visible = false;
          
            timer = new System.Threading.Timer(OnTimerEllapsed, new object(), 0, 2000); // dealer plays now
        }
        /* Starts new round */
        private void dealButton_Click(object sender, EventArgs e)
        {
            playerLabel.Visible = true;
            dealerLabel.Visible = true;
            standButton.Enabled = true;
            hitButton.Enabled = true;
            dealButton.Enabled = false;
            doubleButton.Enabled = true;
            valuePlayer.Visible = true;
            valueDealer.Visible = true;
            standDealer.Visible = true;
            dealerCard2.Image = Image.FromFile("Resources/card-deyox.png");
            dealerCard2.Visible = true;

            shuffle.Play();
            startGame();
        }
        /* Show rules about the game */
        private void rulesButton_Click(object sender, EventArgs e)
        {
            Rules rules = new Rules();
            pause.Play();
            rules.Show();
        }
        /* Increment playerCounter and adds one card to the player's hand */
        private void hitButton_Click(object sender, EventArgs e)
        {
            playerCounter++;
            getCard();
        }
        /* Player doubles his chips to bet, only gets one more card */
        private void doubleButton_Click(object sender, EventArgs e)
        {
            if (coinsTotal >= bet)
            {
                // standButton.Enabled = false;
                hitButton.Enabled = false;
                doubleButton.Enabled = false;

                /* Player gets 3rd card */
                card = deck.dealCard();
                handPlayer.addCard(card);
                placeCard.Play();
                playerCard3.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                playerCard3.Visible = true;
                valuePlayer.Text = "Player's hand total: " + handPlayer.getValue().ToString();
                bet *= 2;
                betLabel.Text = "You doubled your bet!: $" + bet.ToString();
                surrender.Visible = false;

                coinsTotal = coinsTotal - (bet / 2);
                coinsLabel.Text = " = $" + coinsTotal.ToString();

                checkPlayerHand();
            }
            else if (coinsTotal < bet)
            {
                MessageBox.Show("Not enough chips to double amount!");
            }
        }
        /* Bets chips and enabled coins to false so the user can't bet more coins */
        private void betButton_Click(object sender, EventArgs e)
        {
            dealButton.Visible = true;
            betButton.Visible = false;
            whiteCoin.Enabled = false;
            blueCoin.Enabled = false;
            redCoin.Enabled = false;
            blackCoin.Enabled = false;
            greenCoin.Enabled = false;
            dealButton.Enabled = true;
            getChips.Play();
           
            betLabel.Text = "You entered: $" + bet.ToString();
        }
        /* Gets one card and adds it to the player's hand. Depending on the counter, it will add to a certain position */
        private void getCard()
        {
            if (playerCounter == 1) // player gets 3rd card
            {
                /* Player gets 3rd card */
                card = deck.dealCard();
                handPlayer.addCard(card);
                playerCard3.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                playerCard3.Visible = true;
                valuePlayer.Text = "Player's hand total: " + handPlayer.getValue().ToString();
                surrender.Visible = false; doubleButton.Enabled = false;
                checkPlayerHand();
            }
            if (playerCounter == 2) // player gets 4th card
            {
                /* Player gets 4th card */
                card = deck.dealCard();
                handPlayer.addCard(card);
                playerCard4.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                playerCard4.Visible = true;
                valuePlayer.Text = "Player's hand total: " + handPlayer.getValue().ToString();
                surrender.Visible = false; doubleButton.Enabled = false;
                checkPlayerHand();
            }
            if (playerCounter == 3) // player gets 5th card
            {
                /* Player gets 5th card */
                card = deck.dealCard();
                handPlayer.addCard(card);
                playerCard5.Image = Image.FromFile("Resources/" + card.toStringCard() + ".jpg");
                playerCard5.Visible = true;
                valuePlayer.Text = "Player's hand total: " + handPlayer.getValue().ToString();
                surrender.Visible = false; doubleButton.Enabled = false;
                checkPlayerHand();
                // dealer plays next
                // disable hitButton
                hitButton.Enabled = false;
            }
        }

        /* White coin label, value of 1 */
        private void whiteCoin_Click(object sender, EventArgs e)
        {
            if (coinsTotal >= 1)
            {
                bet += 1;
                coinsTotal = coinsTotal - 1;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
                betButton.Enabled = true;
                betChip.Play();
                enableCards();
            }
        }
        /* Red coin label, value of 5 */
        private void redCoin_Click(object sender, EventArgs e)
        {
            if (coinsTotal >= 5)
            {
                bet += 5;
                coinsTotal = coinsTotal - 5;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
                betButton.Enabled = true;
                betChip.Play();
                enableCards();
            }
        }
        /* Green coin label, value of 25 */
        private void greenCoin_Click(object sender, EventArgs e)
        {
            if (coinsTotal >= 25)
            {
                bet += 25;
                coinsTotal = coinsTotal - 25;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
                betButton.Enabled = true;
                betChip.Play();
                enableCards();
            }
        }
        /* Black coin label, value of 100 */
        private void blackCoin_Click(object sender, EventArgs e)
        {
            if (coinsTotal >= 100)
            {
                bet += 100;
                coinsTotal = coinsTotal - 100;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
                betButton.Enabled = true;
                betChip.Play();
                enableCards();
            }
        }
        /* Blue coin label, value of 500 */
        private void blueCoin_Click(object sender, EventArgs e)
        {
            if (coinsTotal >= 500)
            {
                bet += 500;
                coinsTotal = coinsTotal - 500;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
                betButton.Enabled = true;
                betChip.Play();
                enableCards();
            }
        }
        /* Disables cards and labels*/
        private void enableCards()
        {
            playerCard1.Visible = false; playerCard2.Visible = false; playerCard3.Visible = false; playerCard4.Visible = false; playerCard5.Visible = false;
            dealerCard1.Visible = false; dealerCard2.Visible = false; dealerCard3.Visible = false; dealerCard4.Visible = false; dealerCard5.Visible = false;
            valueDealer.Visible = false; valuePlayer.Visible = false; standDealer.Visible = false;
            endGame.Visible = false; dealerEndGame.Visible = false; amountWin.Visible = false;
        }
        /* Sets the dealer counter back to 0 and starts a new round */
        private void newGameButton_Click(object sender, EventArgs e)
        {
            dealerCounter = 0;
            newRound();
        }
         /* Player has surrended, return half of the chips he bet and starts a new round */
        private void surrender_Click(object sender, EventArgs e)
        {
            surrenderCounter++;
            newRound();
        }
        /* Starts a new round by enabling/disabling buttons, labels, and cards.
         * Sets values back to 0
         */
        private void newRound()
        { 
        // start new game if players clicks on new game button
            newGame.Play(); // new game sound
            // reset buttons
            dealButton.Visible = false; standButton.Enabled = false;
            hitButton.Enabled = false; doubleButton.Enabled = false;
            betButton.Enabled = false; surrender.Visible = false;
            // rest coins/chips
            whiteCoin.Enabled = true; blueCoin.Enabled = true;
            redCoin.Enabled = true; blackCoin.Enabled = true;
            greenCoin.Enabled = true;
            // reset player cards
            playerCard1.Visible = false; playerCard2.Visible = false;
            playerCard3.Visible = false; playerCard4.Visible = false;
            playerCard5.Visible = false;
            // reset dealer cards
            dealerCard1.Visible = false; dealerCard2.Visible = false;
            dealerCard3.Visible = false; dealerCard4.Visible = false;
            dealerCard5.Visible = false; dealerLabel.Visible = false;

            gameOverLabel.Visible = false;
            endGame.Visible = false; dealerEndGame.Visible = false;
            playerLabel.Visible = false;
            betButton.Visible = true; amountWin.Visible = false;
            // values in hand
            valuePlayer.Visible = false; valueDealer.Visible = false; standDealer.Visible = false;

            handPlayer = new HandPlayer(); // set values in hand back to zero
            handDealer = new HandDealer(); // set values in hand back to zero

            playerCounter = 0; // counter to draw more cards back to zero
            
        // start new round if player surrenders
            if (surrenderCounter == 0)
            {
                coinsTotal = 1500;
                bet = 0;
                coinsLabel.Text = " = $" + coinsTotal.ToString();
                betLabel.Text = "Click on chips to bet!: $" + bet.ToString();
            }
            else if (surrenderCounter == 1)
            {
               coinsTotal = coinsTotal + bet - (bet/2);
               coinsLabel.Text = " = $" + coinsTotal.ToString();
               bet = 0;
               betLabel.Text = "You surrended! Click on chips to bet!: $" + bet.ToString();
               surrenderCounter--; // back to zero
            }
        }
        /* Redirects to my website for feedback */
        private void mainLogo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.deyox.com/contact.php");
        }
        /* Redirects to my website for feedback */
        private void feedbackLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.deyox.com/contact.php");
        }
    }
}
