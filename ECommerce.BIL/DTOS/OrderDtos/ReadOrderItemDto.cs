namespace ECommerce.BIL.DTOS.OrderDtos
{
    public class ReadOrderItemDto
    {
        public int ItemId { get; set; }
        public int ItemName { get; set; }
        public double ItemUnitPrice { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemTotalPrice { get; set; }


    }
}