namespace src.Domain
{
    using System.Collections.Generic;

    public interface IEntityChanged
    {
        IEnumerable<FieldChanged> FieldsChanged { get; set; }
    }
}
