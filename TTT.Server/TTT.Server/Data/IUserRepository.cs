namespace TTT.Server.Data
{
    public interface IUserRepository : IRepository<User>
    {
        void SetOnline(string id);

        void SetOffline(string id);
    }
}
