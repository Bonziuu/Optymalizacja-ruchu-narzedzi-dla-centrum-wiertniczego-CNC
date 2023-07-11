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
- czasy przemieszczeń poszczególnych wiertnic między punktami.
Aby uruchomić program należy ten plik wrzucić do odpowiedniego folderu, a następnie zamienić ścieżkę jego wczytania w funkcji Main.
