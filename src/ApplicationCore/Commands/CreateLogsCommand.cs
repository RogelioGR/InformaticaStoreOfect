using ApplicationCore.DTOs;
using ApplicationCore.Wrappers;
using MediatR;

namespace ApplicationCore.Commands
{
     public class CreateLogsCommand : logsDTO, IRequest<Response<int>>
     {

     }
}
