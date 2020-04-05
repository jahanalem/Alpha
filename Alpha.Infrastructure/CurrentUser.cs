namespace Alpha.Infrastructure
{
    public class CurrentUser
    {
        public string UserName { get; set; }
        public int? UserId { get; set; }

        public bool IsAuthenticated { get; set; }

        public string AuthenticationType { get; set; }

        //HttpContext.User.IsInRole("Users")
        public bool IsInRoleOfUsers { get; set; }

        //HttpContext.User.IsInRole("Admins")
        public bool IsInRoleOfAdmins { get; set; }

        public string UserIp { get; set; }
    }
}
