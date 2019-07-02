namespace src.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class EntityBase : IEntityChanged
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public IEnumerable<FieldChanged> FieldsChanged { get; set; }
    }
}
