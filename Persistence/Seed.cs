using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
  public class Seed
  {
    public static async Task SeedData(DataContext context,
    UserManager<AppUser> userManager)
    {
      if (!userManager.Users.Any())
      {
          var users = new List<AppUser>
          {
              new AppUser
              {
                  Id = "a",
                  DisplayName = "Kuba",
                  UserName = "kuba",
                  Email = "kuba@test.com",
                  Movie = "https://www.youtube.com/embed/MaUcTH2MCCw",
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "a",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610388689/sogtccaklenjjkkny8rx.jpg",
                      isMain = true
                    }
                  }
              },
              new AppUser
              {
                  Id = "b",
                  DisplayName = "Piotrek",
                  UserName = "piotr",
                  Email = "piotr@test.com",
                  Movie = "https://www.youtube.com/embed/hIUrfeoHLCw",
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "b",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610388720/sdkp41txqx4wettwtivr.jpg",
                      isMain = true
                    }
                  }
              },
              new AppUser
              {
                  Id = "c",
                  DisplayName = "Natalia",
                  UserName = "natalia",
                  Email = "natalia@test.com",
                  Movie = "https://www.youtube.com/embed/spqXq9Z6xio",
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "c",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610389032/zcwnjl1uyhn68lqcutsr.jpg",
                      isMain = true
                    }
                  }
              },
              new AppUser
              {
                  Id = "d",
                  DisplayName = "Jessica",
                  UserName = "jessica",
                  Email = "jessica@test.com",
                  Movie = " https://www.youtube.com/embed/hg4NEh2gjeQ",                 
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "d",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610388899/siq5d0lc6h4wxloe39yd.jpg",
                      isMain = true
                    }
                  }
              },
              new AppUser
              {
                  Id = "e",
                  DisplayName = "Camping Polaris",
                  UserName = "polaris",
                  Email = "polaris@test.com",
                  Movie = "https://www.youtube.com/embed/xl0kSHQMKGQ",                 
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "e",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610389072/pcpbhyloqf0l53ffxzuj.jpg",
                      isMain = true
                    }
                  }
              },
              new AppUser
              {
                  Id = "f",
                  DisplayName = "Chałupy 6 Easy Surf",
                  UserName = "easysurf",
                  Email = "easysurf@test.com",
                  Movie = "https://www.youtube.com/embed/xB0lb71GSFk",
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "f",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610389202/lwhrkg6oo9bemaz4d8nc.jpg",
                      isMain = true
                    }
                  }
              },
              new AppUser
              {
                  Id = "g",
                  DisplayName = "Kites Control",
                  UserName = "kites",
                  Email = "kites@test.com",
                  Movie = "https://www.youtube.com/embed/gL2dVi4J-Lk",
                  Photos = new List<Photo>
                  {
                    new Photo
                    {
                      Id = "g",
                      Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1610389149/ez3bhgnlommyfx3nyrjt.jpg",
                      isMain = true
                    }
                  }
              },
          };

          foreach (var user in users)
          {
              await userManager.CreateAsync(user, "Pa$$w0rd");
          }
      }

      if (!context.Activities.Any())
      {
        var activities = new List<Activity>
        {
            new Activity
            {
                Title = "Darmowa nauka windsurfingu dla dzieci",
                Date = new DateTime(2021, 7, 14, 12, 0, 0),
                Description = "Nauka pływania dla dzieci w wieku od 7 do 13 lat. Zapraszamy wszystkich na darmowy kurs z podstaw windsurfingu, każda zapisana osoba otrzyma godzinę szkolenia za darmo.",
                Category = "Szkolenie",
                City = "Camping Polaris",
                Venue = "Główny pomost",
                UserActivities = new List<UserActivity>
                {
                    new UserActivity
                    {
                        AppUserId = "e",
                        IsHost = true,
                        DateJoined = new DateTime(2021, 7, 14, 12, 0, 0)
                    }
                }
            },
            new Activity
            {
                Title = "Zawody surfingowe Surfing Challenge 2021",
                Date = new DateTime(2021, 7, 25, 10, 0, 0),
                Description = "Polskie Stowarzyszenie Surfingu zaprasza na XV edycję Mistrzostw Polski w Surfingu – Polish Surfing Challenge 2019. Zawody zostaną rozegrane na Półwyspie Helskim w gminie Władysławowo.  Zawodnicy i zawodniczki będą rywalizować w następujących dyscyplinach: Surfing OPEN, Surfing Juniors, SUP Surfing.",
                Category = "Zawody",
                City = "Władysławowo",
                Venue = "Zatoka",
                UserActivities = new List<UserActivity>
                {
                    new UserActivity
                    {
                        AppUserId = "f",
                        IsHost = true,
                        DateJoined = new DateTime(2021, 2, 1, 12, 0, 0)
                    },
                    new UserActivity
                    {
                        AppUserId = "a",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 5, 1, 12, 0, 0)
                    },
                    new UserActivity
                    {
                        AppUserId = "d",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 5, 10, 12, 0, 0)
                    },
                }
            },
            new Activity
            {
                Title = "Ford Kite Cup 2021",
                Date = new DateTime(2021, 8, 01, 10, 0, 0),
                Description = "Zapraszamy na XV sezon Ford Kite Cup organizowany przez Polski Związek Kiteboardingu. Zawody będą trwały dwa dni, zaplanowane jest około 20 wyścigów.",
                Category = "Zawody",
                City = "Chałupy",
                Venue = "Kemping Chałupy 3",
                UserActivities = new List<UserActivity>
                {
                    new UserActivity
                    {
                        AppUserId = "g",
                        IsHost = true,
                        DateJoined = new DateTime(2021, 1, 1, 10, 0, 0)
                    },
                    new UserActivity
                    {
                        AppUserId = "c",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 4, 1, 12, 0, 0)
                    },
                }
            },
            new Activity
            {
                Title = "Salt Wave Festival 2021",
                Date = new DateTime(2021, 8, 20, 18, 0, 0),
                Description = "Zapraszamy wszystkich wielbicieli nadmorskiego klimatu i dobrej muzyki na koncert kończący sezon nad morzem. Zagrają m. in: Pro8l3m, The Cinematic Orchestra, The Dumplings oraz Fisz Emdae Tworzywo.",
                Category = "Koncert",
                City = "Jastarnia",
                Venue = "Lotnisko Jastarnia",
                UserActivities = new List<UserActivity>
                {
                    new UserActivity
                    {
                        AppUserId = "d",
                        IsHost = true,
                        DateJoined = new DateTime(2020, 9, 1, 18, 0, 0)
                    },
                    new UserActivity
                    {
                        AppUserId = "a",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 4, 20, 15, 0, 0)
                    },
                    new UserActivity
                    {
                        AppUserId = "b",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 5, 10, 12, 0, 0)
                    },
                    new UserActivity
                    {
                        AppUserId = "c",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 5, 1, 14, 0, 0)
                    },
                }
            },
            new Activity
            {
                Title = "Poranne pływanie przy wejściu nr 7",
                Date = new DateTime(2021, 7, 12, 8, 0, 0),
                Description = "Cześć, 12 lipca prognozy zapowiadają super pogodę na surfing, wpadajcie popływać z nami!",
                Category = "Spotkanie",
                City = "Kuźnica",
                Venue = "Wejście nr 7",
                UserActivities = new List<UserActivity>
                {
                    new UserActivity
                    {
                        AppUserId = "c",
                        IsHost = true,
                        DateJoined = new DateTime(2021, 7, 10, 19, 0, 0),
                    },
                    new UserActivity
                    {
                        AppUserId = "d",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 7, 11, 11, 0, 0)
                    },
                }
            },
            new Activity
            {
                Title = "28 urodziny Kuby",
                Date = new DateTime(2021, 8, 15, 20, 0, 0),
                Description = "Cześć, 15tego sierpnia świętuje swoje 28 urodziny i z tej okazji zapraszam was na spotkanie na Molo Surf Bar.",
                Category = "Impreza",
                City = "Jastarnia",
                Venue = "Molo Surf Bar",
                UserActivities = new List<UserActivity>
                {
                    new UserActivity
                    {
                        AppUserId = "a",
                        IsHost = true,
                        DateJoined = new DateTime(2021, 8, 1, 16, 0, 0),
                    },

                    new UserActivity
                    {
                        AppUserId = "c",
                        IsHost = false,
                        DateJoined = new DateTime(2021, 8, 5, 16, 0, 0),
                    }
                }
            },
        };

        await context.Activities.AddRangeAsync(activities);
        await context.SaveChangesAsync();
      }
      if (!context.Spots.Any())
      {
        var spots = new List<Spot>
        {
          new Spot
          {
            Latitude = -22.969,
            Longitude = -43.187,
            Name = "Copacabana",
            Country = "Brazylia",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1609760892/Copacabana_emz5bs.jpg"
            
          },
          new Spot
          {
            Latitude = 39.601,
            Longitude = -9.070,
            Name = "Nazare",
            Country = "Portugalia",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610372233/nazare_uomqnp.jpg"
          },
          new Spot
          {
            Latitude = 54.695,
            Longitude = 18.679,
            Name = "Jastarnia",
            Country = "Polska",
            Bio = "Jedna z najbardziej znanych miejscowości na windsurfingowej i kitesurfingowej mapie Polski - duże imprezy sportowe, niepowtarzalny klimat oraz dobre warunki pogodowe w trakcie lata powodują, że jest to miejsce do którego chętnie kierują się osoby spragnione adrenaliny i rozrywki.",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610372232/jastarnia_cnzcag.jpg",
            Photos = new List<SpotPhoto>
            {
              new SpotPhoto
              {
                Id = "a",
                Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1611758015/noc_byn187.jpg"
              },

              new SpotPhoto
              {
                Id = "b",
                Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1611758016/windsurf_dislqg.jpg"
              },
              new SpotPhoto
              {
                Id = "c",
                Url = "https://res.cloudinary.com/da7q8ywkr/image/upload/v1611758018/widok_tq74qv.jpg"
              }
            }
          },
          new Spot
          {
            Latitude = 54.176,
            Longitude = 15.583,
            Name = "Kołobrzeg",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610372232/ko%C5%82obrzeg_hclhuy.jpg"
          },
          new Spot
          {
            Latitude = 54.735,
            Longitude = 18.580,
            Name = "Kuźnica",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610372232/kuznica_kjm3qe.jpg"
          },
          new Spot
          {
            Latitude = 54.791,
            Longitude = 18.403,
            Name = "Władysławowo",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610372233/w%C5%82%C4%85dek_sxoarj.jpg"
          },
          new Spot
          {
            Latitude = 54.760,
            Longitude = 17.556,
            Name = "Łeba",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610373465/%C5%82eba_tgyeov.jpg"
          },
          new Spot
          {
            Latitude = 54.631,
            Longitude = 18.497,
            Name = "Rewa",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610373465/rewa_gks4pl.jpg"
          },
          new Spot
          {
            Latitude = 54.718,
            Longitude = 18.409,
            Name = "Puck",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610373464/zatoka_pucka_x0t8f7.jpg"
          },
          new Spot
          {
            Latitude = 54.759,
            Longitude = 18.509,
            Name = "Chałupy",
            Country = "Polska",
            Bio = "Some fancy info",
            ImageUrl="https://res.cloudinary.com/da7q8ywkr/image/upload/v1610373720/cha%C5%82upy_qwr1jl.jpg"
          },

        };
        await context.Spots.AddRangeAsync(spots);
        await context.SaveChangesAsync();
      }
    }
  }
}