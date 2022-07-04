using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IGroupBl
    {
        public Task<Group> GetGroup(int id);
        public Task<Group> GetGroupByUserId(int id);
        public Task<List<Group>> GetAllGroups();
        public  Task<int> AddGroup(Group g);
        public  Task<Group> UpdateGroup(Group g);

    }
}