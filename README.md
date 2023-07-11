# Optymalizacja ruchu narzędzi dla centrum wiertniczego CNC

Jest to program stworzony na potrzeby pracy magisterskiej na Politechnice Wrocławskiej o nazwie "Optymalizacja ruchu narzędzi w centrum wiertniczym CNC”.
Sama praca zajmuje się zagadnieniami dotyczącymi sposobu działania maszyn CNC, ogólnej definicji
optymalizacji, oraz algorytmami optymalizacyjnymi służącymi do poprawiania jakości
uzyskiwanych rozwiązań dla problemu harmonogramowania zadań w maszynach CNC. W
pracy opisane jest rozwiązanie, które zostało zaimplementowane w celu ustalenia takiego
harmonogramu wykonywania operacji dla centrum wiertniczego CNC dla kliku agregatów wiertniczych pracujących jednocześnie. Przeprowadzone zostały badania na temat
zachowań działania stworzonego algorytmu Branch and Search (B&S), za pomocą którego
ustalane zostały odpowiednie permutacje operacji poszczególnych agregatów dla różnych
wprowadzanych danych wejściowych, takich jak liczba odwiertów, liczba wiertnic, czasy
przemieszczeń narzędzi wiertniczych z punktu do punktu oraz czasy operacji wiercenia.
Dodatkowo zawarty jest opis prototypowej funkcji wyznaczającej kolizje dla niezależnych agregatów oraz wyznaczania nowych permutacji w taki sposób, aby je zniwelować i
uzyskać jak najmniejsze wartości czasów całkowitego wykonania się wszystkich operacji
wiercących.

Program wczytuje dane z pliku "plik.txt" w którym zawiera informacje takie jak:
- liczba wszystkich operacji wiertniczych,
- liczba agregatów wiercących jednocześnie,
- liczba operacji dla poszczególnych agregatów,
- czasy wykonania czynności wiercenia,
- czasy przemieszczeń poszczególnych wiertnic między punktami,
- numery kolejności wierceń.

Aby uruchomić program należy ten plik wrzucić do odpowiedniego folderu, a następnie zamienić ścieżkę jego wczytania w funkcji Main.

Po uruchomieniu program wyświetla wyniki:
- "najlepsze Cmax" - ilość czasu wykonania najdłuższej operacji wiercenia i jednocześnie najkrótszy moment zakończenia całego procesu wiercenia.
- "najlepsze S dla pary konfliktów X" - czas rozpoczęcia operacji konfliktowej X zawartej w liście L,
- "najlepsze C dla pary konfliktów X" - czas zakończenia operacji konfliktowej X zawartej w liście L,
- "czas wykonania BnB" - czas wykonania algorytmu optymalizującego,
- "alfa końcowa" - jest to cały harmonogram wierceń ustalony przez algorytm, który składa się z liczby operacji przemieszczenia z punktu do punktu oraz numeru kolejności wiercenia i są one przemienne, aż do momentu koncowego, w którym przedstawiony jest tylko numer operacji oznaczający powrót agregatu do punktu początkowego. "0" jest separatorem pomiędzy poszczególnymi agregatami.

## Funkcje programu:

Algorytm Branch & Search jest stosowany W celu znalezienia najlepszego harmonogramu wyznaczającego bezkonfliktowy harmonogram wykonywania czynności przez kilka wiertnic. Obiektem podziału jest dowolna para (a, b) operacji dla której wykryto konflikt w
harmonogramie wyznaczonym dla pewnej permutacji α. Wówczas, przestrzeń rozwiązań
dzieli się na dwa podzbiory: (i), w którym operacja a wykonywana jest przed b oraz (i), w
którym operacja b wykonywana jest przed b, przez dodanie odpowiednia pary (a, b) albo
(b, a). Następnie dla każdego podziału wyznaczana jest najlepsze permutacja α dla której
spełniony jest odpowiedni warunek. W przypadku występowania innych konfliktów proces
jest kontynuowany. Jeżeli nie ma już konfliktów to otrzymujemy bezkonfliktową kolejność
spełniającą ograniczenia dodawane przy każdym podziale. Ograniczenia pamiętane są w λ.

Wyznaczenie optymalnego harmonogramu (kolejności α, dla której Cmax jest najmniejszy) dla zadanego ciągu ograniczeń λ jest problemem NP-trudnym z tego powodu do jego
wyznaczenia opracowano prostą metaheurystykę OptymalizacjaPi. Co więcej dla pewnych zbiorów może on nie istnieć (ograniczenia są sprzeczne) wówczas cała podprzestreń
określona przez λ jest pomijana.
Funkcja OptymalizacjaPi jest to rodzaj algorytmu przeszukiwania lokalnego, który iteracyjnie zamienia miejscami pary operacji w permutacji π, aby minimalizować koszt harmonogramu. Choć ta
funkcja może poprawić permutację π i zmniejszyć koszt harmonogramu, nie daje ona
gwarancji, że osiągnięty wynik będzie optymalny.

Funkcja CreateAlfa jest jedną z ważniejszych funkcji w programie. Jest ona odpowiedzialna za wygenerowanie permutacji dla wszystkich wiertnic. Permutacja ta składa się
z numeru operacji przemieszczeń oraz z kolejności wykonywania operacji wiercących
dla wszystkich wiertnic. Jest ona zbudowana na podstawie permutacji π, która określa
kolejność wykonywania operacji dla poszczególnych wiertnic. Zwracana permutacja α jest
później wykorzystywana do generowania harmonogramu wiercenia otworów i optymalizacji
permutacji π w dalszych etapach programu

Funkcja NumerOp odgrywa znaczną rolę w przetwarzaniu danych wejściowych dla
problemu. Jej zadaniem jest obliczenie wartości indeksu O, czyli numer operacji przemieszczenia dla zadanych parametrów k, a, b, gdzie k oznacza numer wiertnicy, a numer
otworu początkowego, b numer otworu końcowego w operacji przemieszczenia wiertnicy.
NumerOp jest podfunkcją dla funkcji CreateAlfa, dla której pomaga wyznaczyć parametr c.
Zwracany parametr O jest używany w dalszych obliczeniach i jest ważny dla optymalizacji
procesu.

Funkcja Harmonogram służy do generowania harmonogramu na podstawie wczytanych danych i listy czynności kolizyjnych.
Harmonogram jest tworzony na podstawie grafu, gdzie wierzchołki reprezentują operacje
(wiercenia i przemieszczenia), a krawędzie reprezentują zależności między nimi. Węzły
obciążone są wagą równą czasowi trwania operacji, natomiast łuki wagą 0. Następnie
wyznaczana jest najdłuższa ścieżka w grafie, a długości najdłuższych dróg są przypisywane do C (S jest obliczana na podstawie C ), które przechowują informacje dotyczące
kolejno czasów rozpoczęcia i zakończenia operacji w harmonogramie. Harmonogram jest
ostatecznie zwracany jako wynik działania funkcji.

Funkcja NumerW, zajmuje się
znajdowaniem numeru wiertnicy, do której należy dana operacja w permutacji π. Funkcja
ta przechodzi przez permutację, sprawdzając każdy element i porównując go z szukanym
elementem. Jeśli operacja zostanie znaleziona, zwracany jest numer odpowiadającej wiertnicy. W przypadku nieodnalezienia operacji zwracana jest wartość -1 jako sygnał błędu lub braku pasującej operacji.

Funkcja Konflikty służy do wykrywania konfliktów pomiędzy operacjami na podstawie
danych zawartych w listach L, S i C. Konflikt występuje, gdy okres wykonywania jednej
operacji nakłada się na okres wykonywania innej operacji. Lista L reprezentuje operacje, które mogą być konfliktowe, natomiast listy S i C przechowują czasy rozpoczęcia i
zakończenia operacji.
