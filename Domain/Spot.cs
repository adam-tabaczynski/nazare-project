using System;
using System.Collections.Generic;

namespace Domain
{
  public class Spot
  {
    public Guid Id { get; set; }
    public string Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Bio { get; set; }
    public virtual ICollection<SpotPhoto> Photos { get; set; }
  }
}