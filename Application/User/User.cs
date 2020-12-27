namespace Application.User
{
  // This is basically a return DTO after login.
  public class User
  {
      public string DisplayName { get; set; }
      public string Token { get; set; }
      public string Username { get; set; }
      public string Image { get; set; }
  }
}