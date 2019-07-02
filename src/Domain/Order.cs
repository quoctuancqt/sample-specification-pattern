namespace src.Domain
{
    using src.Attributes;
    using System.Collections.Generic;

    public class Order: EntityBase
    {
        public Order() { }

        public Order(string name)
        {
            Name = name;
        }

        [Tracker(Actions = new ActionFieldChangedEnum[] 
        {
            ActionFieldChangedEnum.Added,
            ActionFieldChangedEnum.Modified,
            ActionFieldChangedEnum.Deleted,
        })]
        public string Name { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
