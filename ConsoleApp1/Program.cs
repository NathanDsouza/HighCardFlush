using System;
using System.Collections;
using System.Linq;

namespace HighCardFlush
{
    class Game
    {
        public int totalHands, money;
        int card, pLargeF, dLargeF, sTracker;
        int ante, raise, straightBet, flushBet;
        int curStraight, largeStraight, straightProfit, flushProfit;
        int pIndexL, pIndexR, dIndexL, dIndexR;
        int hands, wins;
        bool pQualify, dQualify;
        Random rnd = new Random();
        int[] pH = new int[7];
        int[] dH = new int[7];
        int[] pFCounter = new int[4];
        int[] dFCounter = new int[4];

        private void pay()
        {
            money -= (flushBet + straightBet + ante);
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
                    flushProfit += flushBet * 2;
                    raise = ante;
                    break;

                case 5:
                    money += flushBet * 11;
                    flushProfit += flushBet * 10;
                    raise = ante;
                    break;
                case 6:
                    money += flushBet * 26;
                    flushProfit += flushBet * 25;
                    raise = 2 * ante;
                    break;
                case 7:
                    money += flushBet * 301;
                    flushProfit += flushBet * 300;
                    raise = 3 * ante;
                    break;

                default:
                    raise = ante;
                    break;
            }
        }

        private void checkStraight()
        {

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
                    flushProfit += straightBet * 9;
                    break;

                case 4:
                    money += straightBet * 51;
                    flushProfit += straightBet * 50;
                    break;

                case 5:
                    money += straightBet * 101;
                    flushProfit += straightBet * 100;
                    break;
                case 6:
                    money += straightBet * 251;
                    flushProfit += straightBet * 250;
                    break;
                case 7:
                    money += straightBet * 501;
                    flushProfit += straightBet * 500;
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
            if (!pQualify)
            {
                wins--;

            }
            else if (!dQualify)
            {
                money += ante * 2;
                wins++;
            }
            else if (dLargeF > pLargeF)
            {
                money -= raise;
                wins--;
            }
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
                        wins--;
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
                print();
            }

        }

        public void newGame()
        {
            ante = 15;
            money = 150;
            straightBet = 5;
            flushBet = 5;
            wins = 0;
            hands = 0;
            straightProfit = 0;
            flushProfit = 0;
        }
    }

    class Program
    {
       
        static void Main(string[] args)
        {
            int sum = 0;
            Game game = new Game() { totalHands = 30 };
               
            for (int i = 0; i < 5; i++)
            {
                game.newGame();
                game.play();
                sum += game.money;

            }

            Console.WriteLine("Average money is {0}", sum/5);
            Console.ReadLine();

        }
    }
}
