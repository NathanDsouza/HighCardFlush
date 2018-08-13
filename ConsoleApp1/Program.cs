using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HighCardFlush
{
    class Game
    {
        public int totalHands, money, wins, maxCash, straightProfit, flushProfit, a, b;

        int card, pLargeFlush, dLargeFlush, pK, dK;
        public  int ante, raise, straightBet, flushBet;
        int curStraight, largeStraight;
        int pIndexL, dIndexL;
        int hands;
        bool pQualify, dQualify;
        Random rnd = new Random();
        List<int> deck = new List<int>(52);
        int[] pH = new int[7];
        int[] dH = new int[7];
        int[] pSuitCounter = new int[4];
        int[] dSuitCounter = new int[4];

        private void placeBets()
        {
            //Function to pay the bonuses and ante 
            money -= (flushBet + straightBet + ante);
            flushProfit -= flushBet;
            straightProfit -= straightBet;
        }

        private void deal()
        {
            //Deals out cards. To avoid duplicates I remove the dealt card each time from deck
            for (int i = 0; i < 14; i++)
            {
                card = rnd.Next(deck.Count());
                if (i < 7) pH[i] = deck[card];
                else dH[i - 7] = deck[card];
                deck.RemoveAt(card);
            }
            Array.Sort(pH);
            Array.Sort(dH);

        }

        private void checkFlush()
        {
            //Divides the cards in each hand by 13, uses it as an index, then increments the counter at that index by 1
            //I treat every 13 cards as a suit, so dividing will give me the numbers 0 - 3
            for (int i = 0; i < 7; i++)
            {
                pSuitCounter[(pH[i] / 13)]++;
                dSuitCounter[(dH[i] / 13)]++;
            }
            pLargeFlush = pSuitCounter.Max();
            dLargeFlush = dSuitCounter.Max();
        }

        private void payFlush()
        {
            //Pays out the flush bonus if played
            switch (pLargeFlush)
            {
                case 2:
                case 3:
                    break;
                case 4:
                    money += flushBet * 3;
                    flushProfit += flushBet * 3;
                    raise = ante;
                    break;

                case 5:
                    money += flushBet * 11;
                    flushProfit += flushBet * 11;
                    raise = ante;
                    break;
                case 6:
                    money += flushBet * 26;
                    flushProfit += flushBet * 26;
                    raise = 3 * ante;
                    break;
                case 7:
                    money += flushBet * 301;
                    flushProfit += flushBet * 301;
                    raise = 3 * ante;
                    break;
                    
            }
        }

        private void checkStraight()
        {
            //fix logic on this
            curStraight = 0;
            largeStraight = 0;
            for (int i = 0; i < 6; i++)
            {
                if (curStraight > largeStraight) largeStraight = curStraight;
                if (pH[i] + 1 == pH[i + 1]) curStraight++;
                else curStraight = 0;                
            }
            if (curStraight > largeStraight) largeStraight = curStraight;
        }

        private void payStraight()
        {
            switch (largeStraight)
            {
                case 1:
                case 2:
                    break;
                case 3:
                    money += straightBet * 10;
                    straightProfit += straightBet * 10;
                    break;

                case 4:
                    money += straightBet * 51;
                    straightProfit += straightBet * 51;
                    break;

                case 5:
                    money += straightBet * 101;
                    straightProfit += straightBet * 101;
                    break;
                case 6:
                    money += straightBet * 251;
                    straightProfit += straightBet * 251;
                    break;
                case 7:
                    money += straightBet * 501;
                    straightProfit += straightBet * 501;
                    break;
            }
        }

        private void playerQualify()
        {
            if (pLargeFlush < 3)
            {
                pQualify = false;
                raise = 0;
            }           
            else
            {
                pK = Array.FindIndex(pSuitCounter, x => x == pLargeFlush);
                for (int i = 0; i < pK; i++)
                {
                    pIndexL += pSuitCounter[i];
                }
                a = pH[pIndexL] - (pK * 13);
                if (pLargeFlush > 3) pQualify = true;
                else pQualify = (pH[pIndexL] - (pK * 13) < 4);
            }
        }

        private void dealerQualify()
        {
            if (dLargeFlush < 3) dQualify = false;
            else
            {
                dK = Array.FindIndex(dSuitCounter, x => x == dLargeFlush);
                for (int i = 0; i < dK; i++)
                {
                    dIndexL += dSuitCounter[i];
                }
                b = dH[dIndexL] - (dK * 13);
                if (dLargeFlush > 3) dQualify = true;
                else  dQualify = (dH[dIndexL] - (dK*13) < 7 );
            }
        }

        private void payRaise()
        {
            if (!pQualify) return;
            else if (!dQualify)
            {
                money += ante * 2;
                wins++;
            }
            else if (dLargeFlush > pLargeFlush) money -= raise;
            else if (pLargeFlush > dLargeFlush)
            {
                money += raise + ante * 2;
                wins++;
            }
            else
            {
                for (int i = 0; i < dLargeFlush; i++)
                {

                    if (dH[dIndexL + i] - dK * 13 < pH[pIndexL + i] - pK * 13)
                    {
                        money -= raise;
                        break;
                    }
                    if (pH[pIndexL + i] - pK * 13 < dH[dIndexL + i] - dK * 13)
                    {
                        money += raise + ante * 2;
                        wins++;
                        break;
                    }
                }
            }
        }

        private void print()
        {
            // Console.WriteLine("Straight profit is {0}", straightProfit);
            // Console.WriteLine("Flush profit is {0}\n", flushProfit);
            Console.Write("Player Hand is:");
            for (int i = 0; i < 7; i++)
            {
                Console.Write(" {0}", pH[i]);
            }
            Console.WriteLine("\nPlayer largest flush is {0}, player qualify {1}", pLargeFlush, pQualify);
            Console.WriteLine("High card is {0}", a);
            Console.Write("\nDealer hand is:");
            for (int i = 0; i < 7; i++)
            {
                Console.Write(" {0}", dH[i]);
            }
            Console.WriteLine("\nDealer largest flush is {0}, dealer qualify {1}", dLargeFlush, dQualify);
            Console.WriteLine("High card is {0}", b);

            Console.WriteLine("Wins is {0}, hands are {1}", wins, hands);
            Console.WriteLine("Money is {0}", money);

            Console.ReadLine();
        }

        private void newTurn()
        {
            deck = Enumerable.Range(0, 52).ToList();
            hands++;
            dIndexL = pIndexL = 0;
            pLargeFlush = 0;
            dLargeFlush = 0;
            for (int i = 0; i < 4; i++)
            {
                pSuitCounter[i] = 0;
                dSuitCounter[i] = 0;
            }
        }

        public void play()
        {
            while (hands < totalHands)
            {
                
                newTurn();
                placeBets();
                deal();
                checkFlush();
                payFlush();

                checkStraight();
                payStraight();
                playerQualify();
                dealerQualify();
                payRaise();
                maxCash += raise + ante;

                //Console.WriteLine("{0} ", money);
                // if (maxCash < money) maxCash = money;


                //  print();
            }

        }

        public void newGame()
        {
            ante = 15;
            money = 0;
            straightBet = 0;
            flushBet = 0;
            wins = 0;
            hands = 0;
            straightProfit = 0;
            flushProfit = 0;
        }
    }

    class Flip
    {
        Random rnd = new Random();
        int blub, money;
        public void ya()
        {
            money = 0;
            for (int i = 0; i < 1000000; i++)
            {
                blub = rnd.Next(0, 2);
                //                Console.WriteLine("{0}", blub);
                money -= 10;
                if (blub == 0) money += 20;
               // else if (blub == 1) money -= 10;
            }
            Console.Write("Money is {0}", money);
            Console.ReadLine();

        }
    }
    class Program
    {
       
        static void Main(string[] args)
        {
           // Flip test = new Flip();
           // test.ya();

            decimal sum = 0;
            Game game = new Game() { totalHands = 1000000 };
               
            for (int i = 0; i < 1; i++)
            {
                game.newGame();
                game.play();

                //Console.WriteLine("money for game is {0}", game.money);
                
            }
            sum = (decimal)game.money/game.maxCash;
            Console.WriteLine("cash spent is {0}", game.maxCash);
            Console.WriteLine("money at end of game is {0}", game.money);
            Console.WriteLine("Player advanatage is {0}", sum*100);
            Console.ReadLine();
            //sum = 0;
            //for (int i = 0; i < 100000; i++)
            //{
            //    game.newGame();
            //    game.flushBet = 5;
            //    game.play();

            //    //Console.WriteLine("money for game is {0}", game.money);
            //    sum += game.maxCash;

            //}
            //Console.WriteLine("money w flush bonus at $5 is {0}", sum / 100000);

            //sum = 0;
            //for (int i = 0; i < 100000; i++)
            //{
            //    game.newGame();
            //    game.flushBet = 10;
            //    game.play();

            //    //Console.WriteLine("money for game is {0}", game.money);
            //    sum += game.maxCash;

            //}
            //Console.WriteLine("money w flush bonus at $10 is {0}", sum / 100000);
            //sum = 0;
            //for (int i = 0; i < 100000; i++)
            //{
            //    game.newGame();
            //    game.straightBet = 5;
            //    game.play();

            //    //Console.WriteLine("money for game is {0}", game.money);
            //    sum += game.maxCash;

            //}
            //Console.WriteLine("money w straight bonus at $5 is {0}", sum / 100000);

            //game.flushBet = 0;
            //sum = 0;
            //for (int i = 0; i < 100000; i++)
            //{
            //    game.newGame();
            //    game.straightBet = 5;
            //    game.flushBet = 5;
            //    game.play();

            //    //Console.WriteLine("money for game is {0}", game.money);
            //    sum += game.maxCash;

            //}
            //Console.WriteLine("money w both bonuses at $5 is {0}", sum / 100000);
            //Console.ReadLine();

        }
    }
}
