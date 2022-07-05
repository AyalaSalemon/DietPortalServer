using DL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserInGroupBl : IUserInGroupBl
    {

        IGroupDl gdl;
        IUserBl ubl;
        IUserInGroupDl uigdl;
        IMailBl mailBl;
        public UserInGroupBl(IUserInGroupDl uigdl, IGroupDl gdl, IUserBl ubl, IMailBl mail)
        {
            this.uigdl = uigdl;
            this.gdl = gdl;
            this.ubl = ubl;
            mailBl = mail;
        }
        public async Task<List<User>> GetAllUsers(int groupId)
        {
            return await uigdl.GetAllUsers(groupId);
        }
        public async Task<int> AddUserInGroup(UserInGroup userInGroup, string? password)
        {

            //var groupUsers = await uigdl.GetAllUsers(userInGroup.GroupId);
            //if (groupUsers.Count == 4)
            //{
            //    int daysToAdd = ((int)DayOfWeek.Sunday - (int)DateTime.Today.DayOfWeek + 7) % 7;
            //    DateTime startDate = DateTime.Today.AddDays(daysToAdd);
            //    groupUsers.ForEach(user =>
            //    {
            //        mailBl.SendEmail(user.Email, "new group!!", $"We are happy to tell you that your group is going to begin\n{startDate.ToString("dd/MM/yyyy")}");
            //    });

            //    await gdl.UpdateGroupStatusAndDate(userInGroup.GroupId, startDate);
            //}
            //var userTask = await uigdl.AddUserInGroup(userInGroup);
            //return userTask;
            Group group = await gdl.GetGroupByUserId(userInGroup.UserId);
         //   if (gdl.GetGroupByUserId(userInGroup.UserId).Id == 0)
           if(group.Id==0)
            {

                Group g = await gdl.GetGroup(userInGroup.GroupId);
                if (g.IsOpen == false)
                {
                    if (g.Password.Trim() == password)
                    {
                        return await uigdl.AddUserInGroup(userInGroup);
                    }
                    else
                        throw new Exception("You May Not Join This Group. It's Closed. You Shoud Try Another One.");
                }
                else
                {

                    User u = await ubl.GetUser(userInGroup.UserId);
                    if (g.GenderId != null)
                    {
                        if (g.GenderId != u.Gender)
                        {
                            throw new Exception("You May Not Join This Group. You Shoud Try Another One.");
                        }
                    }
                    if (g.MaxAge != null)
                    {
                        if (g.MaxAge < DateTime.Now.Year - u.DateOfBirth.Year)
                        {
                            throw new Exception("You May Not Join This Group. You Shoud Try Another One.");
                        }
                    }
                    if (g.MinAge != null)
                    {
                        if (g.MinAge > DateTime.Now.Year - u.DateOfBirth.Year)
                        {
                            throw new Exception("You May Not Join This Group. You Shoud Try Another One.");
                        }
                    }
                    var userTask = await uigdl.AddUserInGroup(userInGroup);
                    var groupUsers = await uigdl.GetAllUsers(userInGroup.GroupId);
                    if (groupUsers.Count == 4)
                    {
                        int daysToAdd = ((int)DayOfWeek.Sunday - (int)DateTime.Today.DayOfWeek + 7) % 7;
                        DateTime startDate = DateTime.Today.AddDays(daysToAdd);
                        groupUsers.ForEach(user =>
                       {
                           mailBl.SendEmail(user.Email, "new group!!", $"We are happy to tell you that your group is going to begin\n{String.Format("dd/MM/yyyy", startDate)}");
                       });

                        await gdl.UpdateGroupStatusAndDate(userInGroup.GroupId, startDate);
                    }
                    return userTask;
                }

            }
            else
                throw new Exception("You can't participant in 2 groups.");

        }

        public async Task<List<UserInGroup>> GetAllUserGroups(int userId)
        {
            return await uigdl.GetAllUserGroups(userId);
        }
        public async Task DeleteUserInGroup(UserInGroup userInGroup)
        {

            if ((int)(await gdl.GetGroup(userInGroup.GroupId)).Status == 1)
            {
                await uigdl.DeleteUserInGroup(userInGroup);
                if (uigdl.GetAllUsers(userInGroup.GroupId) == null)
                {
                    await gdl.DeleteGroup(await gdl.GetGroup(userInGroup.GroupId));
                }

            }



        }



    }
}
