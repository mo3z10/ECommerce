using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.Services.InventoryJob
{
    public interface IInventoryJob
    {
        Task CheckLowStock();
    }
}
