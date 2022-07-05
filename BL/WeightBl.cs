using DL;
using DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WeightBl:IWeightBl
    {
        IWeightDl wdl;
        IUserBl ubl;
      //  private ICacheProvider _cacheProvider;
        public WeightBl(IWeightDl wdl,IUserBl ubl)
        {
            this.wdl = wdl;
            this.ubl = ubl;
        }
        public async Task<int> AddWeight(Weight w)
        {

            return await wdl.AddWeight(w);

        }
        public async Task<KeyValuePair<List<int>, double?>> GetWeeklyWinnerGroup()
        {
            Dictionary<int, double?> grps =
               new Dictionary<int, double?>(wdl.GetWeeklyWinnerGroup().Result);
            double? weight = grps.Values.Max();
            List<int> winnerGroups = new List<int>(grps
                .Where(g => g.Value == weight).Select(g => g.Key).ToList());
            return new KeyValuePair<List<int>, double?>(winnerGroups, weight);
        }
        public async Task<KeyValuePair<List<int>, double?>> GetWeeklyWinner()
        {
            Dictionary<int, double?> users =
               new Dictionary<int, double?>(wdl.GetWeeklyWinner().Result);
            double? weight = users.Values.Max();
            List<int> winners = new List<int>(users
                .Where(u => u.Value == weight).Select(u => u.Key).ToList());
            return new KeyValuePair<List<int>, double?>(winners, weight);

        }
        public async Task<KeyValuePair<List<int>, double?>> GetWeeklyGroupWinner(int id)
        {          

            List<UserWithKg> users = await ubl.GetUsersWithKg(id);
            double? maxWeight = -100;
            users.ForEach(u => { if (u.Kg > maxWeight) maxWeight = u.Kg; });

            
            List<int> winners = new List<int>(users
                .Where(user => user.Kg == maxWeight).Select(user => user.Id).ToList());
            return new KeyValuePair<List<int>, double?>(winners, maxWeight);
        }
        public async Task<List<Weight>> GetProgress(int userId)
        {
            return await wdl.GetProgress(userId, ubl.GetGroupByUserId(userId).Result.Id);
        }
    }
}
