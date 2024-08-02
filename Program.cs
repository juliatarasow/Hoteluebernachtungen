using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoteluebernachtungen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            const double mwst = 19d;
            double zimmerpreis = 50d;

            int saison = 0;
            int zimmerkategorie = 0;
            int aufenhaltsdauer = 0;
            string anreisetag = "";
            int anzahlPersonen = 0;
            double nettopreis = 0d;
            double gesamtpreis = 0d;
            double aufschlag = 20d;
            double betragMwst = 0d;

            int anzahlKinder = 0;            
            int alterKind = 0;
            int rabattKind = 0;
            double preisKind = 0d;
            
            int rabattKunde = 0;
            int kundenkategorie = 0;
            double rabattSonntag = 0;

            string antwort = "";

            //double[] Saisonpreis = new double[4];
            //Saisonpreis[0] = 60; //Fruehling
            //Saisonpreis[1] = 70; //Sommer
            //Saisonpreis[2] = 80; //Herbst
            //Saisonpreis[3] = 65; //Winter

            ////alternative Deklaration Array:
            ////double[] Preisliste2 = new double[] { 60, 70, 80, 65 };

            //for (int i = 0; i < Saisonpreis.Length; i++)
            //{
            //    Console.WriteLine("Saisonpreis {0}", Saisonpreis[i]);
            //}


            double[,] Zimmerpeise = new double[,]
                {
                    { 60, 70, 80, 65 },
                    { 70, 80, 90, 75 },
                    { 100, 110, 120, 105 }
                };

            //             | Fruehling(1) | Sommer(2) | Herbst(3) | Winter(4)
            // Standard(1) | 60           | 70        | 80        | 65
            // Komfort(2)  | 70           | 80        | 80        | 75
            // Suite(3)    | 100          | 110       | 120       | 105

            for (int i = 0; i < Zimmerpeise.GetLength(0); i++)
            {
                for (int j = 0; j < Zimmerpeise.GetLength(1); j++)
                {
                    Console.WriteLine("{0}\t", Zimmerpeise[i, j]); // \t = Tabulator
                }
            }

            do
            {
                Console.WriteLine("Saison? 1=Fruehling 2=Sommer 3=Herbst 4=Winter");
                string eingabeSaison = Console.ReadLine();  
                saison = Convert.ToInt32(eingabeSaison);

                Console.WriteLine("Zimmerkategorie? 1=Standard 2=Komfort 3=Suite");
                string eingabeZimmerkategorie = Console.ReadLine();
                zimmerkategorie = Convert.ToInt32(eingabeZimmerkategorie);

                zimmerpreis = Zimmerpeise[zimmerkategorie-1,saison-1]; 
                Console.WriteLine(zimmerpreis);

                Console.WriteLine("Anreisetag?");
                anreisetag = Console.ReadLine();

                Console.WriteLine("Aufenthalsdauer?");
                string eingabeAufenthaltsdauer = Console.ReadLine();
                aufenhaltsdauer = Convert.ToInt32(eingabeAufenthaltsdauer);

                if (aufenhaltsdauer == 5 && anreisetag == "Sonntag")
                {
                    rabattSonntag = 30;
                };

                Console.WriteLine("Anzahl Personen?");
                string eingabeAnzahlPersonen = Console.ReadLine();
                anzahlPersonen = Convert.ToInt32(eingabeAnzahlPersonen);

                Console.WriteLine("Kundenkategorie? ( 1=Stammkunde; 2=Firmenkunde; 3=Reisebüro )");
                string eingabeKundenkategorie = Console.ReadLine();
                kundenkategorie = Convert.ToInt16(eingabeKundenkategorie);

                switch (kundenkategorie)
                {
                    case 1:
                        rabattKunde = 2; break;
                    case 2:
                        rabattKunde = 4; break;
                    case 3:
                        rabattKunde = 6; break;
                    default:
                        rabattKunde = 0; break;
                }

                Console.WriteLine("Anzahl Kinder? 0-2!");
                string eingabeAnzahlKinder = Console.ReadLine();
                anzahlKinder = Convert.ToInt32(eingabeAnzahlKinder);

                while (anzahlKinder > 0)
                {
                    Console.WriteLine("Alter Kind?");
                    string eingabeAlterKind = Console.ReadLine();
                    alterKind = Convert.ToInt32(eingabeAlterKind);

                    if (alterKind < 7)
                    {
                        rabattKind = 100;
                    }
                    else
                    {
                        rabattKind = 70;
                    }

                    preisKind = preisKind + aufenhaltsdauer * zimmerpreis * rabattKind / 100;
                    anzahlKinder = anzahlKinder - 1;
                }

                //Nettopreis berechnen
                if (rabattSonntag != 0)
                {
                    nettopreis = (zimmerpreis * anzahlPersonen * aufenhaltsdauer + preisKind) * (1 - (double)rabattKunde / 100) * (1-rabattSonntag/100);
                }
                else
                {
                    nettopreis = (zimmerpreis * anzahlPersonen * aufenhaltsdauer + preisKind) * (1 - (double)rabattKunde / 100);
                }

                if (aufenhaltsdauer == 1)
                {
                    nettopreis = nettopreis + aufschlag;
                }

                betragMwst = nettopreis * mwst / 100;

                gesamtpreis = Math.Round(nettopreis + betragMwst, 2);

                Console.WriteLine($"Der Gesamtpeis beträgt: {gesamtpreis} €");
                Console.WriteLine("Weitere Angebote berechen? (j/n)");
                antwort = Console.ReadLine();
            } while (antwort != "n");

        }
    }
}
