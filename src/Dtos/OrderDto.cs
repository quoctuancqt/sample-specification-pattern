namespace src.Dtos
{
    using System.Collections.Generic;

    public class OrderDto
    {
        public string Name { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
