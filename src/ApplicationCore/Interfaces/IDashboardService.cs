using ApplicationCore.DTOs;
using ApplicationCore.DTOs.User;
using ApplicationCore.Wrappers;

namespace ApplicationCore.Interfaces
{
    public interface IDashboardService
    {
        Task<Response<object>> GetData();
        Task<Response<int>> Create(UserTDO request);
        Task<Response<int>> Update(int id, UserTDO request);
        Task<Response<String>> GetClientIpAddress();
        Task<Response<int>> GetLogsCreate(logsDTO request);
        Task<Response<int>> Createpersona(UserTDO userTDO);
        Task<Response<int>> Updatepersona(int id, UserTDO request);

        Task<Response<int>> Deletepersona(int id, UserTDO request);


    }

}
