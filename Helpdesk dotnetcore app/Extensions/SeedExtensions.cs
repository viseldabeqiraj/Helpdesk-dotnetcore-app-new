using Helpdesk.Infrastructure.Data;
using Helpdesk.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helpdesk_dotnetcore_app.Extensions
{
    public static class SeedExtensions
    {
        public static void Seed(this ApplicationDbContext context)
        {
            if (context.RequestType.Any(c => c.Category == "Type1"))
                return;

            //Operator
            //Pergjegjes

            var operatorRoleId = -1;
            var pergjegjesRoleId = -1;

            if (!context.Role.Any(r => r.Title == "Operator"))
            {
                var operatortoAdd = new Role
                {
                    Title = "Operator"
                };
                var pergjegjes = new Role
                {
                    Title = "Pergjegjes"
                };
                context.Role.Add(operatortoAdd);
                context.Role.Add(pergjegjes);
                context.SaveChanges();
                operatorRoleId = operatortoAdd.Id;
                pergjegjesRoleId = pergjegjes.Id;
            }

            if (!context.User.Any(u => u.Username == "operator"))
            {
                context.User.Add(new User
                {
                    Username = "operator",
                    Password = "Test123$".GetMD5(),
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            RoleId = operatorRoleId
                        }
                    }
                });
                context.User.Add(new User
                {
                    Username = "pergjegjes",
                    Password = "Test123$".GetMD5(),
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            RoleId = pergjegjesRoleId
                        }
                    }
                });
            }

            context.RequestType.Add(new RequestType
            {
                Category = "Type1"
            });
            context.RequestType.Add(new RequestType
            {
                Category = "Type2"
            });
            context.RequestType.Add(new RequestType
            {
                Category = "Type3"
            });

            context.Program.Add(new Helpdesk.Infrastructure.Data.Entities.Program
            {
                Title = "Program1"
            });
            context.Program.Add(new Helpdesk.Infrastructure.Data.Entities.Program
            {
                Title = "Program2"
            });
            context.Program.Add(new Helpdesk.Infrastructure.Data.Entities.Program
            {
                Title = "Program3"
            });
            context.Program.Add(new Helpdesk.Infrastructure.Data.Entities.Program
            {
                Title = "Program4"
            });

            context.CommunicationChannel.Add(new CommunicationChannel
            {
                CommunicationChannelValue = "Channel1"
            });
            context.CommunicationChannel.Add(new CommunicationChannel
            {
                CommunicationChannelValue = "Channel2"
            });
            context.SaveChanges();
        }
        private static string GetMD5(this string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}
