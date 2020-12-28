using System;

namespace Domain
{
  public class Spot
  {
    public Guid Id { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Name { get; set; }
  }
}