using System.Text.Json;
using System.Diagnostics;

Random random = new Random(40);

const int mapWidth = 100;
const int mapHeight = 100;

const int numberOfVehicles = 1000;

Debug.Assert(numberOfVehicles <= mapWidth * mapHeight, "More vehicles than fields — unique positions impossible.");

List<Vehicle> vehicles = [];
HashSet<Position> occupiedPositions = [];
for (int i = 0; i < numberOfVehicles; i++)
{
    Position position;
    do
    {
        position = new Position(random.Next(0, mapWidth), random.Next(0, mapHeight));
    } while (!occupiedPositions.Add(position));

    vehicles.Add(new Vehicle($"CAR-{i}", position)
    {
        Destination = new Position(random.Next(0, mapWidth), random.Next(0, mapHeight))
    });
}

int moved = 0;
int waiting = 0;
int arrived = 0;

List<TrafficLight> lights = [
    new TrafficLight(new Position(mapWidth / 2, mapHeight / 2)),
    new TrafficLight(new Position(10, 10)),
    new TrafficLight(new Position(80, 80)),
];

bool IsRedLight(Position p, List<TrafficLight> lights)
{
    return lights.Any(l => l.Color == TrafficLightColor.Red && l.Position == p);
}

Stopwatch sw = Stopwatch.StartNew();
// Tick
foreach (Vehicle vehicle in vehicles)
{

    Position oldPosition = vehicle.Position;
    vehicle.MoveTowardsDestination();

    if (vehicle.Position == oldPosition)
    {
        arrived++;
    }
    else if (occupiedPositions.Contains(vehicle.Position) || IsRedLight(vehicle.Position, lights))
    {
        vehicle.Position = oldPosition;
        waiting++;
    }
    else
    {
        occupiedPositions.Remove(oldPosition);
        occupiedPositions.Add(vehicle.Position);
        moved++;
    }
}

sw.Stop();
Console.WriteLine($"Tick took: {sw.Elapsed.TotalMilliseconds:F2} ms");

Debug.Assert(occupiedPositions.Count == vehicles.Count, "Collision! Two vehicles on the same field.");


Console.WriteLine($"Moved: {moved}, Waiting: {waiting}, Arrived: {arrived}");

bool saveJson = args.Contains("--save-json");
if (saveJson)
{
    string json = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText("city.json", json);
}


enum Direction
{
    North,
    East,
    South,
    West
}
record Position(int X, int Y);

class Vehicle
{
    public string Id
    {
        get;
    }
    public Position Position
    {
        get; set;
    }
    public Position? Destination
    {
        get; set;
    }

    public Vehicle(string id, Position position)
    {
        Id = id;
        Position = position;
    }

    public void Move(Direction direction)
    {
        Position movement = direction switch
        {
            Direction.North => new Position(0, -1),
            Direction.East => new Position(1, 0),
            Direction.South => new Position(0, 1),
            Direction.West => new Position(-1, 0),
            _ => new Position(0, 0)
        };

        Position = new Position(Position.X + movement.X, Position.Y + movement.Y);
    }

    public void MoveTowardsDestination()
    {
        Position? destination = Destination;

        if (destination is null)
            return;

        if (Position.X < destination.X)
            Move(Direction.East);
        else if (Position.X > destination.X)
            Move(Direction.West);
        else if (Position.Y < destination.Y)
            Move(Direction.South);
        else if (Position.Y > destination.Y)
            Move(Direction.North);
    }
}

enum TrafficLightColor
{
    Red, Yellow, Green
}

class TrafficLight
{
    public Position Position { get; }
    public TrafficLightColor Color { get; set; }

    public TrafficLight(Position position)
    {
        Position = position;
        Color = TrafficLightColor.Red;
    }

    public void ChangeColor()
    {
        Color = Color switch
        {
            TrafficLightColor.Red => TrafficLightColor.Green,
            TrafficLightColor.Yellow => TrafficLightColor.Red,
            TrafficLightColor.Green => TrafficLightColor.Yellow,
            _ => TrafficLightColor.Red
        };
    }
}