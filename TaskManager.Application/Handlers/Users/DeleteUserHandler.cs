using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Handlers.Users
{
    public class DeleteUserHandler
    {
        private readonly IUserRepository _userRepo;

        public DeleteUserHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async System.Threading.Tasks.Task<bool> HandleAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            await _userRepo.DeleteAsync(user);
            return true;
        }

    }
}
