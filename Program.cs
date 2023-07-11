using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Problem
    {
        public int n; // liczba otworów
        public int w; // liczba wiertnic
        public int[] p = new int[1000]; // czasy wiercenia 
        public int[] lo = new int[1000]; // liczba operacji
        public int[] pi = new int[1000]; // permutacja
        public int[] alfa = new int[1000]; // kolejność wykonania wszystkich czynności na numerWiertnicych
        public int o; // liczba operacji
        public int[] pot = new int[1000]; // pierwsza operacja przemieszczenia wiertnicy
        public int[] npw = new int[1000]; // numer pierwszej operacji wiertnicy
        public int[] m = new int[1000]; // czasy przemieszczenia

        // wczytywanie danych z pliku
        public void Loadfromfile(string nazwapliku)
        {
            List<string> L = MIO.LoadToList(nazwapliku); // wczytanie całego pliku do listy L
            string[] sp = L[0].Split(new char[] { ' ' }); // rozdzielenie pierwszej linii z pliku po spacji
            n = int.Parse(sp[0]); // pierwszy rozdzielony element to liczba otworów
            w = int.Parse(sp[1]); // drugi to liczba wiertnic

            sp = L[1].Split(new char[] { ' ' }); // rozdzielenie drugiej linii w pliku
            for (int i = 0; i < w; i++) // pętla wczytywania liczby operacji lo dla każdej wiertnicy
            {
                lo[i + 1] = int.Parse(sp[i]);

            }

            sp = L[2].Split(new char[] { ' ' }); // rozdzielenie linii trzeciej - czasów wiercenia
            for (int i = 0; i < n; i++) // pętla wczytywania czasów wiercenia p dla każdego otworu
            {
                p[i + 1] = int.Parse(sp[i]);
            }


            L.RemoveRange(0, 3); // usunięcie pierwszych trzech linii listy L
            int wsk = n; // wskaźnik
                         // pętla wczytująca czasy wiercen dla każdej wiertnicy i zapisuje je w p
            for (int i = 1; i <= w; i++) // dla każdej wiertnicy
            {

                for (int k = 1; k <= lo[i] + 1; k++) // dla każdej operacji
                {
                    sp = L[k - 1].Split(new char[] { ' ' }); // rozdzielenie wartości czasów przemieszczenia między punktami
                    for (int r = 0; r < lo[i] + 1; r++) // dla każdej wartości czasu przemieszczenia
                    {
                        p[++wsk] = int.Parse(sp[r]); // przypisanie wartości
                    }
                }
                L.RemoveRange(0, lo[i] + 1);

            }

            // wczytanie kolejnosci wykonywania zadań pi dla każdej z wiertnic od 1 do o (liczby operacji)
            o = wsk;
            wsk = 0;
            for (int i = 1; i <= w; i++) // dla każdej wiertnicy
            {
                sp = L[i - 1].Split(new char[] { ' ' }); // rozdzielenie wartości kolejności wykonywania wierceń

                foreach (var s in sp)
                {
                    pi[++wsk] = int.Parse(s);

                }
                pi[++wsk] = 0; // znacznik końca permutacji
            }
            L.RemoveRange(0, 2);

            // wczytanie kolejnosci wykonywania zadań pi dla wszytkich wiertnic od 1 do o
            wsk = 0;
            for (int i = 1; i <= w; i++)
            {
                sp = L[i - 1].Split(new char[] { ' ' });

                foreach (var s in sp)
                {
                    alfa[++wsk] = int.Parse(s);
                }
                alfa[++wsk] = 0;
            }
            pot[1] = n + 1;
            for (int i = 2; i <= w; i++)
            {
                pot[i] = pot[i - 1] + (lo[i - 1] + 1) * (lo[i - 1] + 1);
            }
            npw[1] = 1;
            for (int i = 2; i <= w; i++)
            {
                npw[i] = npw[i - 1] + lo[i - 1];
            }
            // wczytanie czasów przemieszczenia m dla każdej operacji
            wsk = 0;
            for (int i = 1; i <= w; i++)
            {
                sp = L[i - 1].Split(new char[] { ' ' });

                foreach (var s in sp)
                {
                    m[++wsk] = int.Parse(s);
                }
                m[++wsk] = 0;
            }
        }
        // metoda generująca harmonogram na podstawie listy lambda 
        public (int, int[], int[]) Harmonogram(List<(int src, int dst)> lambda) // src - aktualny wierzcholek, dst - docelowy wierzcholek, lambda - lista czynnosci kolizyjnych
        {
            Graf g = new Graf(o);

            // przypisanie wag wierzchołkom grafu g
            for (int i = 1; i <= o; i++)
            {
                g.p[i] = p[i];

            }

            // tworzenie krawędzi w grafie na podstawie kolejnosci alfa
            int count = 0;
            for (int i = 1; 1 == 1; i++)
            {
                int a = alfa[i - 1];
                int b = alfa[i];
                if (a == 0)
                {
                    continue;
                }

                if (b == 0)
                {
                    count++;
                    if (count == w)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }

                g.addarc(a, b);
            }

            // dodawanie krawędzi na podstawie listy lambda
            foreach (var e in lambda)
            {
                g.addarc(e.src, e.dst);
            }

            // wywołanie metody obliczającej najdłuższą ścieżkę w grafie
            int max = 0;
            if (g.longestpath() == false)
            {
                max = 99999;
            }

            int[] C = new int[o + 2];
            int[] S = new int[o + 2];

            for(int i = 0; i <= o+1; i++)
            {
                C[i] = 0;
                S[i] = 0;
            }

            foreach (int i in alfa)
            {
                if (i != 0)
                {
                    max = Math.Max(max, g.l[i]);
                    C[i] = g.l[i];
                    S[i] = C[i] - p[i];
                }

            }
            return (max, S, C);
        }

        // metoda tworząca alfa na podstawie permutacji pi i liczby operacji dla każdej wiertnicy
        public void Alfa()
        {
            List<int> al = new List<int>();

            int k = 1;
            al.Add(0);
            for (int i = 1; i <= n + w; i++)
            {
                int a = pi[i - 1];
                int b = pi[i];

                int c = NumerO(k, a, b);
                if (b == 0)
                {
                    k++;
                }
                al.Add(c);
                al.Add(b);
            }
            alfa = al.ToArray();
        }

        // metoda dla operacji O zwracająca numer przemieszczenia dla danego czasu przemieszczenia
        public int NumerO(int k, int a, int b)
        {
            if (k > 1)
            {
                if (b != 0)
                {
                    b = b - npw[k] + 1;
                }
                if (a != 0)
                {
                    a = a - npw[k] + 1;
                }

            }
            int O = pot[k] + a * (lo[k] + 1) + b;


            return O;
        }
       
        public void OptymalizacjaPI(List<(int src, int dst)> lambda)
        {
            
            List<int> listaPI = pi.ToList(); // lista przechowująca permutację pi dla każdej wiertnicy
            Alfa(); // generowanie permutacji alfa
            int najlepszyKoszt;
            (najlepszyKoszt, _, _) = Harmonogram(lambda); // obliczanie najlepszego kosztu harmonogramu

            int bezZmian = 0; // Licznik iteracji bez zmiany
            int maxBezZmian = 10; // Maksymalna liczba iteracji bez zmiany

            while (true) // pętla działa tak długo dopóki można poprawic permutację
            {

                bool flaga = false;

                // przejście przez wszystkie możliwe pary elementów w liście pi
                for (int i = 1; i < listaPI.Count - 1; i++)
                {
                    for (int j = i + 1; j < listaPI.Count; j++)
                    {
                        // jeśli drugi element pary operacji jest równy 0 to pomiń parę
                        if (listaPI[j] == 0)
                        {
                            continue;
                        }

                        // sprawdzenie czy oba elementy pary operacji należą do tej samej wiertnicy
                        int numerWiertnicy_i = NumerW(listaPI[i]);
                        int numerWiertnicy_j = NumerW(listaPI[j]);
                        if (numerWiertnicy_i != numerWiertnicy_j)
                        {
                            continue;
                        }

                        // zamiana miejscami elementów pary operacji
                        int zmienna = listaPI[i];
                        listaPI[i] = listaPI[j];
                        listaPI[j] = zmienna;

                        pi = listaPI.ToArray(); // aktualizacja permutacji pi
                        Alfa(); // kolejne wygenerowanie permutacji alfa na podstawie nowego pi
                        int nowyKoszt;
                        (nowyKoszt, _, _) = Harmonogram(lambda);

                        // sprawdzenie czy nowy koszt jest lepszy
                        if (nowyKoszt < najlepszyKoszt)
                        {
                            // jeśli tak to nadpisz nowy koszt
                            najlepszyKoszt = nowyKoszt;
                            flaga = true;
                        }
                        else
                        {
                            // jeśli nie to następuje przywrócenie poprzedniej permutacji, jeśli nowa nie jest lepsza
                            zmienna = listaPI[i];
                            listaPI[i] = listaPI[j];
                            listaPI[j] = zmienna;
                        }
                    }
                }

                if (!flaga)
                {
                    bezZmian++; // Zwiększanie licznika bez zmiany

                    if (bezZmian >= maxBezZmian)
                    {
                        break; // Zakończenie pętli, jeśli przekroczono maksymalną liczbę iteracji bez zmiany
                    }
                }
            }
        }
        // znajdowanie do którego numeru wiertnicy należy dana operacja pi
        public int NumerW(int element)
        {
            int numerWiertnicy = 1;
            for (int i = 1; i <= o; i++) // od 1 do o czyli liczby wszystkich operacji
            {
                if (pi[i] == element) // jesli operacja to szukany element
                {
                    return numerWiertnicy; // to zwracanie numeru wiertnicy
                }
                if (pi[i] == 0) // jesli operacja to 0
                {
                    numerWiertnicy++; // to daj kolejną wiertnicę
                }
            }
            return -1; // w przypadku błędu dla braku pasującej operacji
        }
    }
    internal class Program
    {
        static List<(int src, int dst)> Konflikty (List<(int src, int dst)> L, int[] S, int[] C)
        {
            List<(int src, int dst)> R = new List<(int src, int dst)>();
            foreach (var p in L)
            {
                int nd1;
                int nd2;
                (nd1, nd2) = p;
                int s1 = S[nd1];
                int s2 = S[nd2];
                int c1 = C[nd1];
                int c2 = C[nd2];
                bool isconflict = false;
                if (s2 >= s1) if (s2 < c1) isconflict = true;
                if (s1 >= s2) if (s1 < c2) isconflict = true;

                if(isconflict == true)
                {
                    R.Add(p);
                }
            }
            return R;
        }

        static (int, int[], int[]) BnB(Problem P, List<(int src, int dst)> L)
        {
            List<(int src, int dst)> lambda = new List<(int src, int dst)>();
            int Cmax = int.MaxValue;
            int[] C = null;
            int[] S = null;
            Queue<List<(int src, int dst)>> Q = new Queue<List<(int src, int dst)>>();
            Q.Enqueue(lambda);

            int najlepszeCmax = int.MaxValue;
            int[] najlepszeS = new int[P.o + 2];
            int[] najlepszeC = new int[P.o + 2];
            (int, int) najlepszaParaKonfliktu = (-1, -1);

            Stopwatch zegar = new Stopwatch();
            zegar.Start();
            while (Q.Count > 0)
            {
                lambda = Q.Dequeue();
                List<(int src, int dst)> lambda1 = new List<(int src, int dst)>(lambda);
                List<(int src, int dst)> lambda2 = new List<(int src, int dst)>(lambda);

                P.Alfa();
                P.OptymalizacjaPI(lambda);
                (Cmax, S, C) = P.Harmonogram(lambda);
                List<(int src, int dst)> konflikty = Konflikty(L, S, C);
                if (konflikty.Count == 0)
                {
                    if (Cmax < najlepszeCmax)
                    {
                        najlepszeCmax = Cmax;
                        S.CopyTo(najlepszeS, 0);
                        C.CopyTo(najlepszeC, 0);
                    }

                    continue;
                }
                if (Cmax >= 99999) continue;
                
                int nd1;
                int nd2;
                (nd1, nd2) = konflikty[0];
                if (Cmax < najlepszeCmax)
                {
                    najlepszeCmax = Cmax;
                    najlepszaParaKonfliktu = (nd1, nd2);
                    S.CopyTo(najlepszeS, 0);
                    C.CopyTo(najlepszeC, 0);
                }
                lambda1.Add((nd1, nd2));
                lambda2.Add((nd2, nd1));

                Q.Enqueue(lambda1);
                Q.Enqueue(lambda2);
            }
            zegar.Stop();

            Console.WriteLine($"Najlepsze Cmax: {najlepszeCmax}");
            Console.WriteLine($"Najlepsze S dla pary konfliktów {najlepszaParaKonfliktu}: {najlepszeS[najlepszaParaKonfliktu.Item1]}, {najlepszeS[najlepszaParaKonfliktu.Item2]}");
            Console.WriteLine($"Najlepsze C dla pary konfliktów {najlepszaParaKonfliktu}: {najlepszeC[najlepszaParaKonfliktu.Item1]}, {najlepszeC[najlepszaParaKonfliktu.Item2]}");

            Console.WriteLine($"Czas wykonania BnB: {zegar.Elapsed}");

            Console.WriteLine("Alfa końcowa:");
            foreach (int a in P.alfa)
            {
                Console.Write(a + " ");
            }

            return (najlepszeCmax, najlepszeS, najlepszeC);
        }


        static void Main(string[] args)
        {
            Problem P = new Problem();
            P.Loadfromfile("D:\\Github\\plik.txt");
            List<(int src, int dst)> L = new List<(int src, int dst)>();
            L.Add((10, 5));
            L.Add((6, 1));
            L.Add((7, 1));
            L.Add((8, 1));

            var result = BnB(P, L);

            // Wyświetlanie alfa dla momentu, w którym Cmax jest najmniejsze
            int najlepszeCmax = result.Item1;
            int[] najlepszeS = result.Item2;
            int[] najlepszeC = result.Item3;

            Console.ReadKey();
        }

    }
    public class Graf
    {
        public int n; // liczba wierzchołkow
        public List<int>[] nast; // następniki
        public List<int>[] pop; // poprzedniki
        public int[] p = new int[1000]; // waga wierzcholka
        public int[] l = new int[1000]; // najdłuższe ścieżki w grafie

        // inicjalizacja grafu
        public Graf(int n)
        {
            this.n = n;
            nast = new List<int>[n + 1]; // lista następników dla danego wierzchołka
            pop = new List<int>[n + 1]; // lista poprzedników 
            for (int i = 0; i <= n; i++)
            {
                nast[i] = new List<int>();
                pop[i] = new List<int>();
            }

        }

        // dodawanie krewędzi do grafu
        public void addarc(int src, int dst)
        {
            nast[src].Add(dst);
            pop[dst].Add(src);

        }

        // metoda obliczania najdłuższej ścieżki w grafie
        public bool longestpath()
        {
            int[] lp = new int[1000];
            for (int i = 1; i <= n; i++)
            {
                lp[i] = pop[i].Count;
                l[i] = 0;
            }
            Queue<int> q = new Queue<int>();
            for (int i = 1; i <= n; i++)
            {
                if (lp[i] == 0)
                {
                    q.Enqueue(i);
                }
            }

            int count = 0;
            while (q.Count > 0)
            {
                count++;
                int nd = q.Dequeue();

                int s = 0;

                foreach (int pp in pop[nd])
                {
                    s = Math.Max(s, l[pp]);

                }

                l[nd] = s + p[nd];

                foreach (int nst in nast[nd])
                {
                    lp[nst]--;
                    if (lp[nst] == 0)
                    {
                        q.Enqueue(nst);
                    }
                }
            }

            return count == n;
        }
    }
}
