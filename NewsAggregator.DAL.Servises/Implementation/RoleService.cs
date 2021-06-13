using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDto> GetUserRole(string userName)
        {
            var user = await _unitOfWork.User.FindBy(user => user.Email.Equals(userName), user => user.Role).FirstOrDefaultAsync();
            var role = (await _unitOfWork.User.FindBy(user => user.Email.Equals(userName), user => user.Role).FirstOrDefaultAsync()).Role;
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<UserDto> AddRoleToUser(Guid id) // ADD MIGRATION!
        {
            var user = await _unitOfWork.User.GetById(id);
            if (user != null)
            {
                user.RoleId = _unitOfWork.Role.FindBy(role => role.Name.Equals("User")).FirstOrDefault().Id;
                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveChangeAsync();
            }
            
            return  new UserDto
            {
                Email = user.Email,
                HashPass = user.HashPass,
                Id = user.Id,
                Login = user.Login,
                RoleId = user.Id
            };
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            var roles = await _unitOfWork.Role.Get().ToListAsync();
            var rolesDto = new List<RoleDto>();
            foreach (var role in roles)
            {
                rolesDto.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }

            return rolesDto;
        }

    }
}
