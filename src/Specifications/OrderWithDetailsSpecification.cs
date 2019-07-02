namespace src.Specifications
{
    using System;
    using src.Domain;

    public class OrderWithDetailsSpecification : BaseSpecification<Order>
    {
        public OrderWithDetailsSpecification(string name) : base(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
        {
            AddInclude(x => x.OrderDetails);
        }
    }
}
