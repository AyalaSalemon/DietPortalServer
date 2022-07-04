using DietPortal.Enums;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class GroupDl:IGroupDl
    {
        ProjectDBContext dbContext;
        public GroupDl(ProjectDBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Group> GetGroup(int id)
        {
            return await dbContext.Groups.SingleOrDefaultAsync(g => g.Id == id);
        }
        

        public async Task<List<Group>> GetAllGroups()//&&&
        {
            return await dbContext.Groups.Where(g=>g.Status!=2).ToListAsync();
        }
        public async Task<Group> GetGroupByUserId(int userId)//!
        {
            UserInGroup u=await dbContext.UserInGroups.Where(g => g.UserId == userId)
                .Include(uig => uig.Group).SingleOrDefaultAsync(uig => uig.Group.Status == 0);/////change to 0!!
            if (u != null)
            {
                return u.Group;
            }
            else
                return new Group();
            
            //return dbContext.UserInGroups.Where(g => g.UserId == userId)
            //   .Join(dbContext.Groups, ug => ug.GroupId, g => g.Id, 
            //   (ug,g)=>new { id = g.Id, status = g.Status })
            //   .SingleOrDefaultAsync(g=>g.status==1).Result.id;
           
        }
        public async Task<int> AddGroup(Group g)
        {
            await dbContext.Groups.AddAsync(g);
            
            await dbContext.SaveChangesAsync();
            return await dbContext.Groups.Where(w => (w.GroupName == g.GroupName && w.Status == g.Status)).Select(g1=>g1.Id).FirstOrDefaultAsync();

        }
       
       
       
        public async Task<Group> UpdateGroup(Group g)
        {
            var groupToUpdate = await dbContext.Groups.FindAsync(g.Id);
            if (groupToUpdate == null)
                return null;
            dbContext.Entry(groupToUpdate).CurrentValues.SetValues(g);
            await dbContext.SaveChangesAsync();

            return g;
        }
       
        public async Task DeleteGroup(Group g)
        {
            dbContext.Groups.Remove(g);
            await dbContext.SaveChangesAsync();

        }

        public async Task<Group> UpdateGroupStatusAndDate(int groupId, DateTime startDate)
        {
            Group groupToUpdate = await dbContext.Groups.FindAsync(groupId);
            if (groupToUpdate == null)
                return null;
            groupToUpdate.StartDate = startDate;
            groupToUpdate.Status = (int)StatusGroupEnum.Active;
            dbContext.Entry(groupToUpdate).CurrentValues.SetValues(groupToUpdate);
            await dbContext.SaveChangesAsync();

            return groupToUpdate;
        }
    }
}
