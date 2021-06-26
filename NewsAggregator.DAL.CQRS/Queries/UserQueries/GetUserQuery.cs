using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }

        public GetUserQuery(Guid? id, string email, string login)
        {
            Id = id;
            Email = email;
            Login = login;
        }
    }
}
