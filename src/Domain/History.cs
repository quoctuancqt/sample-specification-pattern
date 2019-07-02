namespace src.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class History
    {
        public History()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public History(FieldChanged fieldChanged, int createdBy)
        {
            FieldName = fieldChanged.FieldName;
            OldValue = fieldChanged.OldValue;
            NewValue = fieldChanged.NewValue;
            ActionType = fieldChanged.ActionType;
            EntityType = fieldChanged.EntityType;
            CreatedDate = DateTime.UtcNow;
            CreatedBy = createdBy;
            EntityName = fieldChanged.EntityType.Name;
        }

        public int Id { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public ActionFieldChangedEnum ActionType { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
        public string EntityName { get; set; }
        public string _EntityType
        {
            get { return EntityType.AssemblyQualifiedName; }
            set { EntityType = Type.GetType(value); }
        }

        [NotMapped]
        public Type EntityType { get; private set; }
    }
}
