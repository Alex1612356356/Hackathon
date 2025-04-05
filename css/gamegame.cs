using System;
using System.Collections.Generic;

namespace CityTrafficSimulation
{
    public class Car
    {
        public string Name { get; set; }
        public string CurrentLocation { get; set; }
        public string Destination { get; set; }
        public bool HasReachedDestination => CurrentLocation == Destination;

        public Car(string name, string currentLocation, string destination)
        {
            Name = name;
            CurrentLocation = currentLocation;
            Destination = destination;
        }

        // Move the car toward its destination based on available roads
        public void Move(List<Region> cityRegions)
        {
            if (!HasReachedDestination)
            {
                foreach (var region in cityRegions)
                {
                    if (region.Name == CurrentLocation)
                    {
                        var nextRegion = region.GetNextRegion(Destination);
                        if (nextRegion != null)
                        {
                            CurrentLocation = nextRegion.Name;
                            Console.WriteLine($"{Name} is moving to {CurrentLocation}.");
                        }
                    }
                }
            }
        }
    }

    public class Region
    {
        public string Name { get; set; }
        public List<Connection> Connections { get; set; }

        public Region(string name)
        {
            Name = name;
            Connections = new List<Connection>();
        }

        // Get the next region to move to based on destination
        public Region GetNextRegion(string destination)
        {
            foreach (var connection in Connections)
            {
                if (connection.ToRegion.Name == destination)
                    return connection.ToRegion;
            }
            return null;
        }

        // Add a connection between regions
        public void AddConnection(Region toRegion, string connectionType)
        {
            Connections.Add(new Connection(this, toRegion, connectionType));
        }
    }

    public class Connection
    {
        public Region FromRegion { get; set; }
        public Region ToRegion { get; set; }
        public string ConnectionType { get; set; } // Road, Bridge, Tunnel

        public Connection(Region fromRegion, Region toRegion, string connectionType)
        {
            FromRegion = fromRegion;
            ToRegion = toRegion;
            ConnectionType = connectionType;
        }

        public override string ToString()
        {
            return $"{FromRegion.Name} -> {ToRegion.Name} ({ConnectionType})";
        }
    }

    public class City
    {
        public List<Region> Regions { get; set; }
        public List<Car> Cars { get; set; }

        public City()
        {
            Regions = new List<Region>();
            Cars = new List<Car>();
        }

        // Add a region to the city
        public void AddRegion(Region region)
        {
            Regions.Add(region);
        }

        // Add a car to the city
        public void AddCar(Car car)
        {
            Cars.Add(car);
        }

        // Build a new road, bridge, or tunnel between two regions
        public void BuildConnection(Region from, Region to, string connectionType)
        {
            from.AddConnection(to, connectionType);
            to.AddConnection(from, connectionType); // Bidirectional connection
            Console.WriteLine($"{connectionType} built between {from.Name} and {to.Name}");
        }

        // Simulate the city traffic
        public void SimulateTraffic()
        {
            Console.WriteLine("Simulating city traffic...");
            foreach (var car in Cars)
            {
                car.Move(Regions);
                if (car.HasReachedDestination)
                    Console.WriteLine($"{car.Name} has reached its destination: {car.Destination}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a city
            City baiaMare = new City();

            // Create regions (parts of the city)
            Region north = new Region("North");
            Region south = new Region("South");
            Region east = new Region("East");
            Region west = new Region("West");

            // Add regions to the city
            baiaMare.AddRegion(north);
            baiaMare.AddRegion(south);
            baiaMare.AddRegion(east);
            baiaMare.AddRegion(west);

            // Create cars
            baiaMare.AddCar(new Car("Car 1", "North", "South"));
            baiaMare.AddCar(new Car("Car 2", "East", "West"));

            // Build roads, bridges, and tunnels
            baiaMare.BuildConnection(north, south, "Road");
            baiaMare.BuildConnection(east, west, "Bridge");

            // Simulate traffic and the movement of cars
            baiaMare.SimulateTraffic();

            Console.ReadKey();
        }
    }
}
