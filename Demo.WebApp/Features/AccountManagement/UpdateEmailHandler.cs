using System.Linq;
using Demo.WebApp.Domain;
using Demo.WebApp.Domain.Entities.Account;
using Force;
using Force.Cqrs;

namespace Demo.WebApp.Features.AccountManagement
{
    public class UpdateEmailHandler: IHandler<UpdateUserEmail>
    {
        public UpdateEmailHandler(IQueryable<User> users)
        {

        }

        public void Handle(UpdateUserEmail command)
        {
            var user = command.UserId.Entity;
            user.Email = command.Email;
        }
    }
}