using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hoteluebernachtungen
{
    internal class Program
    {
        public enum Kundenkategorie
        {
            Stammkunde = 1,
            Firmenkunde = 2,
            Reisebüro = 3
        }
        public enum Wochentag
        {
            Montag = 1,
            Dienstag = 2,
            Mittwoch = 3,
            Donnerstag = 4,
            Freitag = 5,
            Samstag = 6,
            Sonntag = 7
        }
        public enum Saison
        {
            Fruehling = 1,
            Sommer = 2, 
            Herbst = 3,
            Winter = 4
        }

        public enum Zimmerkategorie
        {
            Standard = 1,
            Comport = 2,
            Suite = 3
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            const double mwst = 19d;
            double zimmerpreis = 50d;

            int saison = 0;
            int zimmerkategorie = 0;
            int aufenthaltsdauer = 0;
            int anreisetag = 0;
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
            Kundenkategorie kundenkategorie;

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

            //-----Tabelle mit Zimmerpreisen---------------------------------
            //             | Fruehling(1) | Sommer(2) | Herbst(3) | Winter(4)
            // Standard(1) | 60           | 70        | 80        | 65
            // Komfort(2)  | 70           | 80        | 80        | 75
            // Suite(3)    | 100          | 110       | 120       | 105


            //-----Alle Zimmerpreise werden ausgegeben--------------------------------
            for (int i = 0; i < Zimmerpeise.GetLength(0); i++)
            {
                for (int j = 0; j < Zimmerpeise.GetLength(1); j++)
                {
                    Console.WriteLine($"{Zimmerpeise[i, j]}\t"); // \t = Tabulator
                }
            }
            //
            // Zimmerpreise.GetLength(0) -> nullte Dimension

            do
            {
                AusgabeWerteEnum<Saison>();
                saison = EingabeInteger("Saison?");
                zimmerkategorie = EingabeInteger("Zimmerkategorie? 1=Standard 2=Komfort 3=Suite");
                zimmerpreis = Zimmerpeise[zimmerkategorie - 1, saison - 1];
                anreisetag = EingabeInteger("Anreisetag?");
                aufenthaltsdauer = EingabeInteger("Aufenthaltsdauer?");
                rabattSonntag = AnreiseSonntag(aufenthaltsdauer, anreisetag);
                //if (aufenthaltsdauer == 5 && anreisetag == "Sonntag")
                //{
                //    rabattSonntag = 30.0;
                //};

                anzahlPersonen = EingabeInteger("Anzahl Personen?");
                int eingabeKundenkategorie = EingabeInteger("Kundenkategorie? ( 1=Stammkunde; 2=Firmenkunde; 3=Reisebüro )");
                kundenkategorie = (Kundenkategorie)eingabeKundenkategorie;

                switch (kundenkategorie)
                {
                    case Kundenkategorie.Stammkunde:
                        rabattKunde = 2; break;
                    case Kundenkategorie.Firmenkunde:
                        rabattKunde = 4; break;
                    case Kundenkategorie.Reisebüro:
                        rabattKunde = 6; break;
                    default:
                        rabattKunde = 0; break;
                }

                anzahlKinder = EingabeInteger("Anzahl Kinder? 0-2!");

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

                    preisKind = preisKind + aufenthaltsdauer * zimmerpreis * rabattKind / 100;
                    anzahlKinder = anzahlKinder - 1;
                }

                //Nettopreis berechnen
                if (rabattSonntag != 0)
                {
                    nettopreis = (zimmerpreis * anzahlPersonen * aufenthaltsdauer + preisKind) * (1 - (double)rabattKunde / 100) * (1 - rabattSonntag / 100);
                }
                else
                {
                    nettopreis = (zimmerpreis * anzahlPersonen * aufenthaltsdauer + preisKind) * (1 - (double)rabattKunde / 100);
                }

                if (aufenthaltsdauer == 1)
                {
                    nettopreis = nettopreis + aufschlag;
                }

                betragMwst = nettopreis * mwst / 100;

                gesamtpreis = Math.Round(nettopreis + betragMwst, 2);

                Console.WriteLine($"Der Gesamtpeis beträgt: {gesamtpreis} €");
                Console.WriteLine("Weitere Angebote berechen? (j/n)");
                antwort = Console.ReadLine();
            } while (antwort != "n");

            Console.WriteLine("Aufenthalsdauer?");
            string eingabeAufenthaltsdauer = Console.ReadLine();
            aufenthaltsdauer = Convert.ToInt32(eingabeAufenthaltsdauer);
        }
        public static int EingabeInteger(string aufforderung)
        {
            int antwort = 0;
            Console.WriteLine(aufforderung);
            string eingabeZahl = Console.ReadLine();
            antwort = Convert.ToInt32(eingabeZahl);

            return antwort;
        }
        public static double AnreiseSonntag(int aufenthaltsdauer, int anreisetag)
        {
            double rabatt = 0d;
            if (aufenthaltsdauer == 5 && anreisetag == 7)
            {
                rabatt = 30d;
            };
            return rabatt;
        }
        public static void AusgabeWerteEnum<T>() where T : Enum {

            var values = Enum.GetValues(typeof(T));
            int index = 0;
            foreach (T value in values)
            {
                Console.Write($"{index}={value} ");
                index++;
            };
        }
    }
}
