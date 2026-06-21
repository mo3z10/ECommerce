using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace ECommerce.BIL.DTOS.OrderDtos
{
    public class CreateOrderItemDto
    { 
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int quaintity { get; set; }

    }
}