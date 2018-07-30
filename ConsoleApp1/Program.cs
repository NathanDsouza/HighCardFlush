using System;
using System.Collections;
using System.Linq;

namespace ConsoleApp1
{
    class HighCardFlush
    {
        int card, money, pLargeF, dLargeF, sTracker;
        int straightBet, straightProfit, flushBet, flushProfit;
        int curStraight, largeStraight;
        int pIndexL, pIndexR, dIndexL, dIndexR;
        bool pQualify, dQualify;
        Random rnd = new Random();
        int[] pH = new int[7];
        int[] dH = new int[7];
        int[] pFCounter = new int[4];
        int[] dFCounter = new int[4];

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
                    break;

                case 5:
                    money += flushBet * 11;
                    flushProfit += flushBet * 10;
                    break;
                case 6:
                    money += flushBet * 26;
                    flushProfit += flushBet * 25;
                    break;
                case 7:
                    money += flushBet * 301;
                    flushProfit += flushBet * 300;
                    break;

                default:
                    break;
            }
        }

        private void checkStraight()
        {

            for (int i = 0; i < 7; i++)
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
            if (pLargeF > 3) pQualify = true;

        }

        public void play()
        {
            deal();
            checkFlush();
            payFlush();
            checkStraight();
            payStraight();
            playerQualify();
            dealerQualify();
        }
    }

    class Program
    {
       
        static void Main(string[] args)
        {
            HighCardFlush game = new HighCardFlush;
            game.play();

        }
    }
}
