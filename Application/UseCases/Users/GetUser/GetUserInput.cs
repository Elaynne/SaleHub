using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.GetUser
{
    public class GetUserInput : IRequest<User>
    {
        public Guid Id { get; set; }
    }
}
