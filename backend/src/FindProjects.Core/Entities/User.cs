using Microsoft.AspNet.Identity.EntityFramework;

namespace FindProjects.Core.Entities;

public class User : IdentityUser
{
    public string ProfileDescription { get; set; }
}