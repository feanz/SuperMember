using SuperMember.Sample.Code.Services;

namespace SuperMember.Sample.Code.Interfaces
{
    public interface IUserServiceFactory
    {
        UserService CreateUserService();
    }
}