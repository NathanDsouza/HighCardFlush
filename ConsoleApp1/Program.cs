using System;
using System.Collections;
using System.Linq;

namespace HighCardFlush
{
    class Game
    {
        public int totalHands, money, wins, maxCash, straightProfit, flushProfit;
        int card, pLargeF, dLargeF, sTracker;
      public  int ante, raise, straightBet, flushBet;
        int curStraight, largeStraight;
        int pIndexL, pIndexR, dIndexL, dIndexR;
        int hands;
        bool pQualify, dQualify;
        Random rnd = new Random();
        int[] pH = new int[7];
        int[] dH = new int[7];
        int[] pFCounter = new int[4];
        int[] dFCounter = new int[4];

        private void pay()
        {
            money -= (flushBet + straightBet + ante);
            flushProfit -= flushBet;
            straightProfit -= straightBet;
        }

        private void deal()
        {
            for (int i = 0; i < 14; i++)
            {
                card = rnd.Next(52);
                if (pH.Contains(card) || dH.Contains(card))
                {
                    i--;
                    continue;
                }
                if (i < 7) pH[i] = card;
                else dH[i - 7] = card;                
            }
            Array.Sort(pH);
            Array.Sort(dH);
        }

        private void checkFlush()
        {
            pLargeF = 0;
            dLargeF = 0;
            for (int i = 0; i < 7; i++)
            {
                pFCounter[(pH[i] / 13)]++;
                dFCounter[(dH[i] / 13)]++;
            }
            pLargeF = pFCounter.Max();
            dLargeF = dFCounter.Max();
        }

        private void payFlush()
        {
            switch (pLargeF)
            {
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
                    raise = 2 * ante;
                    break;
                case 7:
                    money += flushBet * 301;
                    flushProfit += flushBet * 301;
                    raise = 3 * ante;
                    break;

                default:
                    raise = ante;
                    break;
            }
        }

        private void checkStraight()
        {
            curStraight = 0;
            largeStraight = 0;
            for (int i = 0; i < 6; i++)
            {
                if (pH[i] + 1 == pH[i + 1]) curStraight++;
                else
                {
                    if (curStraight > largeStraight) largeStraight = curStraight;
                    curStraight = 0;
                }
            }            
        }

        private void payStraight()
        {
            switch (largeStraight)
            {
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

                default:
                    break;
            }
        }

        private void playerQualify()
        {
            if (pLargeF < 3) pQualify = false;
            else if (pLargeF > 3) pQualify = true;
            else
            {
                int k = Array.FindIndex(pFCounter, x => x == pLargeF);
                for (int i = 0; i < k; i++)
                {
                    pIndexL += pFCounter[i];                    
                }
                pIndexR = pIndexL + pLargeF - 1;
                pQualify = (pH[pIndexR] / (k + 1) > 8);
            }
        }

        private void dealerQualify()
        {
            if (dLargeF < 3) dQualify = false;
            else if (dLargeF > 3) dQualify = true;
            else
            {
                int k = Array.FindIndex(dFCounter, x => x == dLargeF);
                for (int i = 0; i < k; i++)
                {
                    dIndexL += dFCounter[i];
                }
                dIndexR = dIndexL + dLargeF - 1;
                dQualify = (dH[dIndexR] / (k + 1) > 6);
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
            else if (dLargeF > pLargeF) money -= raise;
            else if (pLargeF > dLargeF)
            {
                money += raise + ante * 2;
                wins++;
            }
            else
            {
                for (int i = 0; i < dLargeF; i++)
                {

                    if (dH[dIndexL + i] > pH[pIndexL + i])
                    {
                        money -= raise;
                        break;
                    }
                    if (pH[pIndexL + i] > dH[dIndexL + i])
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
            Console.WriteLine("Money is {0}", money);
            Console.WriteLine("Straight profit is {0}", straightProfit);
            Console.WriteLine("Flush profit is {0}\n", flushProfit);

        }

        private void newTurn()
        {
            hands++;
            dIndexL = dIndexR = pIndexL = pIndexR = 0;
                for (int i = 0; i < 4; i++)
            {
                pFCounter[i] = 0;
                dFCounter[i] = 0;
            }
        }

        public void play()
        {
            while (money > 40 && hands < totalHands)
            {
                
                newTurn();
                pay();
                deal();
                checkFlush();
                payFlush();
                checkStraight();
                payStraight();
                playerQualify();
                dealerQualify();
                payRaise();
                if (maxCash < money) maxCash = money;

                // print();
            }

        }

        public void newGame()
        {
            ante = 15;
            money = 150;
            straightBet = 0;
            flushBet = 0;
            wins = 0;
            hands = 0;
            straightProfit = 0;
            flushProfit = 0;
            maxCash = money;
            pLargeF = 0;
        }
    }

    class Program
    {
       
        static void Main(string[] args)
        {
            double sum = 0;
            Game game = new Game() { totalHands = 30 };
            Console.WriteLine("30 hands played in one game, 100000 games ran\nAverage money at the end of each game is shown below");
               
            for (int i = 0; i < 100000; i++)
            {
                game.newGame();
                game.play();

                //Console.WriteLine("money for game is {0}", game.money);
                sum += game.maxCash;

            }
            Console.WriteLine("money w no bonuses played is {0}", sum / 100000);
            sum = 0;
            for (int i = 0; i < 100000; i++)
            {
                game.newGame();
                game.flushBet = 5;
                game.play();

                //Console.WriteLine("money for game is {0}", game.money);
                sum += game.maxCash;

            }
            Console.WriteLine("money w flush bonus at $5 is {0}", sum / 100000);

            sum = 0;
            for (int i = 0; i < 100000; i++)
            {
                game.newGame();
                game.flushBet = 10;
                game.play();

                //Console.WriteLine("money for game is {0}", game.money);
                sum += game.maxCash;

            }
            Console.WriteLine("money w flush bonus at $10 is {0}", sum / 100000);
            sum = 0;
            for (int i = 0; i < 100000; i++)
            {
                game.newGame();
                game.straightBet = 5;
                game.play();

                //Console.WriteLine("money for game is {0}", game.money);
                sum += game.maxCash;

            }
            Console.WriteLine("money w straight bonus at $5 is {0}", sum / 100000);

            game.flushBet = 0;
            sum = 0;
            for (int i = 0; i < 100000; i++)
            {
                game.newGame();
                game.straightBet = 5;
                game.flushBet = 5;
                game.play();

                //Console.WriteLine("money for game is {0}", game.money);
                sum += game.maxCash;

            }
            Console.WriteLine("money w both bonuses at $5 is {0}", sum / 100000);
            Console.ReadLine();

        }
    }
}
