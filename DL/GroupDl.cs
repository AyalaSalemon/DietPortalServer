﻿using Entities;
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
            return await dbContext.Groups.Where(g=>g.Status!=3).ToListAsync();
        }
        public async Task<int> GetGroupId(int userId)//!
        {
            UserInGroup u=await dbContext.UserInGroups.Where(g => g.UserId == userId)
                .Include(uig => uig.Group).SingleOrDefaultAsync(uig => uig.Group.Status == 1);
            if (u!=null) { 
                return u.Group.Id;
            }
            else
                return 0;
            
            //return dbContext.UserInGroups.Where(g => g.UserId == userId)
            //   .Join(dbContext.Groups, ug => ug.GroupId, g => g.Id, 
            //   (ug,g)=>new { id = g.Id, status = g.Status })
            //   .SingleOrDefaultAsync(g=>g.status==1).Result.id;
           
        }
        public async Task<int> AddGroup(Group g)
        {
            await dbContext.Groups.AddAsync(g);
            
            await dbContext.SaveChangesAsync();
            return await dbContext.Groups.Where(w => (w.GroupName == g.GroupName && w.Status == g.Status)).Select(g1=>g1.Id).SingleOrDefaultAsync();

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
    }
}