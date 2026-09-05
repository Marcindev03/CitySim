# CitySim (PL)

Mała symulacja ruchu pojazdów na siatce. Powstała jako 30-dniowy challenge nauki C#, po 5 minut dziennie.

## Uruchomienie

```sh
dotnet run
```

Zapis stanu miasta do JSON:

```sh
dotnet run -- --save-json
```

## Wymagania

- .NET 10 SDK

## Co robi

- Tworzy 1000 pojazdów na unikalnych losowych polach (ziarnowany `Random`, powtarzalne wyniki).
- Wykonuje jeden tick symulacji: każdy pojazd rusza się o jedno pole w stronę celu.
- Wykrywa kolizje przez `HashSet<Position>` — dwa pojazdy nigdy nie stoją na jednym polu.
- 3 sygnalizacje świetlne blokują wjazd na czerwonym.
- Wypisuje statystyki (`Moved`, `Waiting`, `Arrived`) i czas ticka przez `Stopwatch`.
- Dwa asserty (`Debug.Assert`) pilnują niezmienników symulacji.

## Ograniczenia

1. Ruch tylko w osiach X oraz Y, jedno pole na tick, brak skosów.
2. Kolejność w `foreach` decyduje, kto pierwszy zajmie sporne pole.
3. Start wymaga wolnych pól (`numberOfVehicles <= mapWidth * mapHeight`).
