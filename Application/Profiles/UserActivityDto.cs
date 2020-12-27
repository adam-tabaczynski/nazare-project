using System;

namespace Application.Profiles
{
    // This is just for the profile tab of activities - I do not need all the other informations like
    // comments, who is attending, etc.
    public class UserActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
    }
}