namespace src.Domain
{
    using src.Attributes;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderDetail: EntityBase
    {
        public OrderDetail() { }

        public OrderDetail(int orderId, string name)
        {
            OrderId = orderId;
            Name = name;
        }

        [Tracker(Actions = new ActionFieldChangedEnum[]
        {
            ActionFieldChangedEnum.Added,
            ActionFieldChangedEnum.Modified,
            ActionFieldChangedEnum.Deleted,
        })]
        public string Name { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
