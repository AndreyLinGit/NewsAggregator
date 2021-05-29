using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
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

        public async Task AddRoleToUser(string userName, RoleDto roleDto)
        {
            if (_unitOfWork.Role.GetById(roleDto.Id) != null)
            {
                var user = await _unitOfWork.User.FindBy(user => user.Email.Equals(userName)).FirstOrDefaultAsync();
                user.RoleId = roleDto.Id;
                await _unitOfWork.User.Add(user);
                await _unitOfWork.SaveChangeAsync();
            }
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
