namespace TTT.Server.Data
{
    public class User
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public ushort Score { get; set; }

        public bool IsOnline { get; set; }
    }
}
