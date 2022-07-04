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
   public class GroupBl:IGroupBl
    {

        IGroupDl gdl;
        IUserInGroupBl uigbl;
        public GroupBl(IGroupDl gdl,IUserInGroupBl uigbl)
        {
            this.gdl = gdl;
            this.uigbl = uigbl;
        }

        public async Task<Group> GetGroup(int id)
        {
            return await gdl.GetGroup(id);
        }
        public async Task<Group> GetGroupByUserId(int id)
        {
            return await gdl.GetGroupByUserId(id);
        }
        public async Task<List<Group>> GetAllGroups()
        {
            return await gdl.GetAllGroups();
        }
        public async Task<int> AddGroup(Group g)
        {

            return await gdl.AddGroup(g);

        }
       
        public async Task<Group> UpdateGroup(Group g)
        {
            return await gdl.UpdateGroup(g);
        }
        public async Task<Group> ChangeGroupStatus(Group g)
        {
            Group g1=await gdl.UpdateGroup(g);
            if (g.Status == 1)
            {    
                uigbl.GetAllUsers(g.Id).Result.ForEach(u => uigbl.GetAllUserGroups(u.Id).Result.ForEach(uig=>uigbl.DeleteUserInGroup(uig)));
            }
            return g1;
        }

    }
}
