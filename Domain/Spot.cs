using System;
namespace Domain
{
  public class Spot
  {
    public Guid Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string Name { get; set; }
  }
}