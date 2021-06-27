using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> GetUserRole(Guid userId)
        {
            var role = (await _unitOfWork.User.FindBy(user => user.Id.Equals(userId), user => user.Role).FirstOrDefaultAsync()).Role;
            return _mapper.Map<RoleDto>(role);
        }

        public async Task AddRoleToUser(Guid id) // ADD MIGRATION!
        {
            var user = await _unitOfWork.User.GetById(id);
            if (user != null)
            {
                user.RoleId = (await _unitOfWork.Role.FindBy(role => role.Name.Equals("User")).FirstOrDefaultAsync()).Id;
                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveChangeAsync();
            }
        }

    }
}
