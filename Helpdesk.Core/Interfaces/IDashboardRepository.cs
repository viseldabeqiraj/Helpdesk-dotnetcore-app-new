using Helpdesk.Core.Dtos.Dashboard;
using Helpdesk.Core.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<bool> DeleteConfirmedAsync(int id);
        Task<RequestViewModel> GetRequestAsync(int? id);
        Task<List<RequestViewModel>> GetRequestsAsync(GetRequestsDto getRequestsDto);
        Task<CreateViewModel> GetCreateViewModelAsync();
        Task<bool> CreateRequestAsync(ClientRequestViewModel clientRequestViewModel);
        Task<bool> ResponseAsync(RequestResponseViewModel requestResponseViewModel);
        Task<int?> GetClientIdByNIDAsync(string nid);
        Task<bool> EditRequestAsync(ClientRequestViewModel clientRequestViewModel);
        Task<RequestResponseViewModel> GetResponseAsync(int? id);
    }
}
