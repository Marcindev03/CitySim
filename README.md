# CitySim

A small grid-based vehicle traffic simulation. Built as a 30-day C# learning challenge, 5 minutes a day.

## Run

```sh
dotnet run
```

Export the city state to JSON:

```sh
dotnet run -- --save-json
```

## Requirements

- .NET 10 SDK

## What it does

- Spawns 1000 vehicles on unique random grid positions (seeded `Random`, reproducible runs).
- Runs a single simulation tick: every vehicle moves one cell toward its destination.
- Detects collisions with a `HashSet<Position>` — two vehicles never share a cell.
- 3 traffic lights block movement on red.
- Prints stats (`Moved`, `Waiting`, `Arrived`) and tick time via `Stopwatch`.
- Two `Debug.Assert` guards protect the simulation invariants.

## Limitations

1. Movement only along the X/Y axes, one cell per tick, no diagonals.
2. `foreach` order decides who claims a contested cell first.
3. Spawn requires free cells (`numberOfVehicles <= mapWidth * mapHeight`).
