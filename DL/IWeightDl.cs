using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
    public interface IWeightDl
    {

        public  Task<int> AddWeight(Weight w);
        public  Task<Dictionary<int, double?>> GetWeeklyWinnerGroup();
        public  Task<Dictionary<int, double?>> GetWeeklyWinner();
        public  Task<Dictionary<int, double?>> GetWeeklyGroupWinner(int id);
        public Task<Dictionary<int, double?>> GetGroupWinner(int id);
        public  Task<List<Weight>> GetProgress(int userId, int groupId);
       
    }
}