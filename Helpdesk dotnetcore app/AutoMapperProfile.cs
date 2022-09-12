using AutoMapper;
using Helpdesk.Core.ViewModels.Dashboard;
using Helpdesk.Infrastructure.Data.Entities;

namespace Helpdesk_dotnetcore_app
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClientViewModel, Client>();
            CreateMap<Client, ClientViewModel>();

            CreateMap<CommunicationChannelViewModel, CommunicationChannel>();
            CreateMap<CommunicationChannel, CommunicationChannelViewModel>();

            CreateMap<ProgramViewModel, Helpdesk.Infrastructure.Data.Entities.Program>();
            CreateMap<Helpdesk.Infrastructure.Data.Entities.Program, ProgramViewModel>();

            CreateMap<RequestTypeViewModel, RequestType>();
            CreateMap<RequestType, RequestTypeViewModel>();

            CreateMap<RequestViewModel, Request>();
            CreateMap<Request, RequestViewModel>();

            CreateMap<ResponseViewModel, Response>();
            CreateMap<Response, ResponseViewModel>();

            CreateMap<RoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>();

            CreateMap<UserRoleViewModel, UserRole>();
            CreateMap<UserRole, UserRoleViewModel>();

            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();
        }
    }
}
