using AutoMapper;
using AutoMapper.QueryableExtensions;
using Helpdesk.Core.Dtos.Dashboard;
using Helpdesk.Core.Interfaces;
using Helpdesk.Core.ViewModels.Dashboard;
using Helpdesk.Infrastructure.Data;
using Helpdesk.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpdesk.Infrastructure.Services.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DashboardRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<bool> DeleteConfirmedAsync(int id)
        {
            var request = await _dbContext
                .Request
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null || request?.Current_Status == "Përfunduar")
                return false;

            request.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<RequestViewModel> GetRequestAsync(int? id)
        {
            var request = await _dbContext
                .Request
                .Where(r => r.Id == id)
                .Include(h => h.Client)
                .Include(h => h.CommunicationChannel)
                .Include(h => h.Program)
                .Include(h => h.RequestType)
                .Include(h => h.Client)
                .ProjectTo<RequestViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return request;
        }

        public async Task<List<RequestViewModel>> GetRequestsAsync(GetRequestsDto getRequestsDto)
        {
            var query = _dbContext.Request
                .Where(r => r.IsDeleted != true && r.Current_Status == getRequestsDto.Status)
                .Include(h => h.Client)
                .Include(h => h.CommunicationChannel)
                .Include(h => h.Program)
                .Include(h => h.RequestType)
                .Include(h => h.Client)
                .AsQueryable();

            var role = await _dbContext.User
                .Where(u => u.Id == getRequestsDto.UserId)
                .Select(u => u.RoleId).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(getRequestsDto.Status))
            {
                if (role == 1)
                {
                    query = _dbContext.Request
                        .Where(r => r.OperatorId == getRequestsDto.UserId);
                }
                else if (role == 2)
                {
                    query = _dbContext.Request.Where
                        (r => r.ResponsibleId == getRequestsDto.UserId);
                }
            }
            else
            {
                if (role == 1)
                {
                    query = _dbContext.Request.Where
                        (r => r.Current_Status == getRequestsDto.Status && r.OperatorId == getRequestsDto.UserId);
                }
                else if (role == 2)
                {
                    query = _dbContext.Request.Where
                        (r => r.Current_Status == getRequestsDto.Status && r.ResponsibleId == getRequestsDto.UserId);
                }
            }

            var list = await query
                .ProjectTo<RequestViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return list;
        }

        public async Task<CreateViewModel> GetCreateViewModelAsync()
        {
            var responsible = await _dbContext
                .User
                .Where(u => u.RoleId == 2)
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var commChannels = await _dbContext
                .CommunicationChannel
                .ProjectTo<CommunicationChannelViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var programs = await _dbContext
                .Program
                .ProjectTo<ProgramViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var requestTypes = await _dbContext
                .RequestType
                .ProjectTo<RequestTypeViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new CreateViewModel
            {
                Responsible = responsible,
                CommunicationChannels = commChannels,
                Programs = programs,
                RequestTypes = requestTypes
            };
        }

        public async Task<bool> CreateRequestAsync(ClientRequestViewModel clientRequestViewModel)
        {
            var hasClient = await _dbContext.Client
                .AnyAsync(s => s.NID == clientRequestViewModel.NID);

            var req = new Request();
            var client = new Client();

            if (hasClient)
            {
                var clientId = await _dbContext.Client
                    .Where(s => s.NID == clientRequestViewModel.NID)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();
                req.ClientId = clientId;
            }
            else
            {
                client.NID = clientRequestViewModel.NID;
                client.FirstName = clientRequestViewModel.FirstName;
                client.Surname = clientRequestViewModel.Surname;
                client.Email = clientRequestViewModel.Email;
                client.Telephone_Nr = clientRequestViewModel.Telephone_Nr;
                _dbContext.Client.Add(client);
                await _dbContext.SaveChangesAsync();
                req.ClientId = client.Id;
            }

            req.RequestTypeId = clientRequestViewModel.IDHD_Request_Type;
            req.ProgramId = clientRequestViewModel.IDHD_Program;
            req.Title = clientRequestViewModel.Title;
            req.Descriptions = clientRequestViewModel.Descriptions;
            req.Reception_Date = clientRequestViewModel.Reception_Date;
            req.Document_Name = clientRequestViewModel.Document_Name;
            if (clientRequestViewModel.Bytes != null)
            {
                req.Document_Content = clientRequestViewModel.Bytes;
            }
            req.Current_Status = clientRequestViewModel.Current_Status;
            req.Registration_Date = DateTime.Now;
            req.CommunicationChannelId = clientRequestViewModel.IDHD_CommunicationChannel;
            req.ResponsibleId = clientRequestViewModel.IDHD_Responsible;
            req.OperatorId = clientRequestViewModel.UserId;

            _dbContext.Request.Add(req);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ResponseAsync(RequestResponseViewModel requestResponseViewModel)
        {
            var check = await _dbContext.Response
                .Where(r => r.RequestId == requestResponseViewModel.RequestId)
                .FirstOrDefaultAsync();

            if (check == null)
            {
                var Request_Response = new Response();
                Request_Response.Response_Content = requestResponseViewModel.Response_Content;
                Request_Response.Response_Date = DateTime.Now;
                Request_Response.RequestId = requestResponseViewModel.RequestId;
                // db.HD_Request.Where(s => s.IDHD_Request == hD_response.IDHD_Request).Select(s => s.IDHD_Request).FirstOrDefault();
                Request_Response.UserId = requestResponseViewModel.UserId;
                var id = await _dbContext.Request
                    .Where(r => r.Id == requestResponseViewModel.RequestId)
                    .FirstOrDefaultAsync();
                id.Current_Status = "Përfunduar";
                id.Completion_Date = DateTime.Now;
                _dbContext.Response.Add(Request_Response);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return true;
            }
        }

        public async Task<int?> GetClientIdByNIDAsync(string nid)
        {
            var clientId = await _dbContext.Client
                .Where(s => s.NID == nid)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            return clientId;
        }

        public async Task<bool> EditRequestAsync(ClientRequestViewModel hD_Request)
        {
            var client = await _dbContext.Client
                .Where(c => c.NID == hD_Request.NID)
                .FirstOrDefaultAsync();

            client.NID = hD_Request.NID;
            client.FirstName = hD_Request.FirstName;
            client.Surname = hD_Request.Surname;
            client.Email = hD_Request.Email;
            client.Telephone_Nr = hD_Request.Telephone_Nr;

            var request = await _dbContext
                .Request
                .Where(r => r.Id == hD_Request.IDHD_Request)
                .FirstOrDefaultAsync();

            request.RequestTypeId = hD_Request.IDHD_Request_Type;
            request.ProgramId = hD_Request.IDHD_Program;
            request.Title = hD_Request.Title;
            request.Descriptions = hD_Request.Descriptions;
            request.Reception_Date = hD_Request.Reception_Date;
            request.CommunicationChannelId = hD_Request.IDHD_CommunicationChannel;
            request.Document_Name = hD_Request.Document_Name;
            if (hD_Request.Bytes != null)
            {
                request.Document_Content = hD_Request.Bytes;
            }
            request.Current_Status = hD_Request.Current_Status;
            request.ResponsibleId = hD_Request.IDHD_Responsible;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<RequestResponseViewModel> GetResponseAsync(int? id)
        {
            var hD_Request = await _dbContext.Request.Include(r => r.Client).FirstOrDefaultAsync(r => r.Id == id);

            if (hD_Request == null)
                return null;

            RequestResponseViewModel hd_Client_Request = new RequestResponseViewModel();
            hd_Client_Request.NID = hD_Request.Client.NID;
            hd_Client_Request.FirstName = hD_Request.Client.FirstName;
            hd_Client_Request.Surname = hD_Request.Client.Surname;
            hd_Client_Request.Email = hD_Request.Client.Email;
            hd_Client_Request.Telephone_Nr = hD_Request.Client.Telephone_Nr;

            var Category = new { category = "" };
            var cat = (from r in _dbContext.Request
                       join t in _dbContext.RequestType
                       on r.RequestTypeId equals t.Id
                       where r.Id == id
                       select new
                       {
                           Category = t.Category
                       }).FirstOrDefault();
            hd_Client_Request.Category = cat.Category;

            var Program = new { program = "" };
            var prog = (from r in _dbContext.Request
                        join p in _dbContext.Program
                        on r.ProgramId equals p.Id
                        where r.Id == id
                        select new
                        {
                            Program = p.Title
                        }).FirstOrDefault();
            hd_Client_Request.Program = prog.Program;

            var Chanel = new { chanel = "" };
            var ch = (from r in _dbContext.Request
                      join c in _dbContext.CommunicationChannel
                      on r.CommunicationChannelId equals c.Id
                      where r.Id == id
                      select new
                      {
                          Chanel = c.CommunicationChannelValue
                      }).FirstOrDefault();
            hd_Client_Request.Communication_Channel = ch.Chanel;

            var Responsible = new { responsible = "" };
            var res = (from r in _dbContext.Request
                       join u in _dbContext.User
                       on r.ResponsibleId equals u.Id
                       where r.Id == id
                       select new
                       {
                           Responsible = u.FirstName
                       }).FirstOrDefault();
            hd_Client_Request.Responsible = res?.Responsible;

            hd_Client_Request.Title = hD_Request.Title;
            hd_Client_Request.Descriptions = hD_Request.Descriptions;
            hd_Client_Request.Reception_Date = hD_Request.Reception_Date;
            hd_Client_Request.Document_Name = hD_Request.Document_Name;
            hd_Client_Request.Current_Status = hD_Request.Current_Status;
            hd_Client_Request.Registration_Date = hD_Request.Registration_Date;
            hd_Client_Request.IDHD_CommunicationChannel = hD_Request.CommunicationChannelId;

            hd_Client_Request.IDHD_Request = _dbContext.Request.Where
                (r => r.Id == id).Select
                (r => r.Id).FirstOrDefault();
            hd_Client_Request.Response_Content = _dbContext.Response.Where
                (r => r.RequestId == id).Select
                (r => r.Response_Content).FirstOrDefault();
            // hd_Client_Request.Completion_Date=
            //hd_Client_Request.Response_Date = db.HD_Response.Where(r => r.IDHD_Request == id).Select(r => r.Response_Date).FirstOrDefault();
            var check = _dbContext.Response.Where
                (r => r.RequestId == id).ToList();

            if (check.Count > 0)
            {
                hd_Client_Request.Response_Date = _dbContext.Response.Where
                    (r => r.RequestId == id).Select
                    (r => r.Response_Date).FirstOrDefault();
                hd_Client_Request.Completion_Date = hd_Client_Request.Response_Date;
            }
            else
                hd_Client_Request.Response_Date = DateTime.Now;

            return hd_Client_Request;
        }
    }
}
