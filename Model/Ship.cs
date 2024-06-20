using NUnit.Framework;

public class Ship
{
    public int Mmsi { get; set; }
    public string Name { get; set; }
    public Position Position { get; set; }
    public bool IsHome { get; set; }
}