using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
    public interface IGroupDl
    {
        public  Task<Group> GetGroup(int id);
        public  Task<List<Group>> GetAllGroups();
        public  Task<Group> GetGroupByUserId(int userId);
        public Task<int> AddGroup(Group g);
        public  Task<Group> UpdateGroup(Group g);
        public Task DeleteGroup(Group g);
        Task<Group> UpdateGroupStatusAndDate(int groupId, DateTime startDate);


    }
}