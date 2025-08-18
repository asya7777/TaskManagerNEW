using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Handlers.Users
{
    public class GetAllUsersHandler
    {
        IUserRepository _userRepo;
        public GetAllUsersHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async System.Threading.Tasks.Task<IEnumerable<object>> HandleAsync()//<List<User>> diyemiyoruz çünkü tüm user attr. return lemiyor
        {
            var users = await _userRepo.GetAllUsersAsync();
            return users.Select(u => new {u.usrId, u.firstName, u.lastName });
        }
    }
}
