using ApplicationCore.DTOs;
using ApplicationCore.DTOs.User;
using ApplicationCore.Wrappers;
using MediatR;

namespace ApplicationCore.Commads
{
    public class CreateUserCommand : UserTDO, IRequest<Response<int>>
    {
    }
}
