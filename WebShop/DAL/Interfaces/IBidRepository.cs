using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBidRepository : IRepository<Bid> 
    {
        public IList<Bid> GetAllBidsForItem(int itemId);
        public Bid GetHighestBidForItem(int itemId);
    }

}
