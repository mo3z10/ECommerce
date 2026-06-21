using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.GenericRepo;

namespace ECommerce.DAL.Reposatories.OrdersRepo
{
    public interface IOrderRepo :IGenericRepo<Order>
    {
    }
}
