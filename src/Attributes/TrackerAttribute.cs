namespace src.Attributes
{
    using src.Domain;
    using System;
    using System.Collections.Generic;

    [AttributeUsage(AttributeTargets.Property)]
    public class TrackerAttribute : Attribute
    {
        public TrackerAttribute()
        {
            Actions = new List<ActionFieldChangedEnum>
            {
                ActionFieldChangedEnum.Added,
                ActionFieldChangedEnum.Modified,
                ActionFieldChangedEnum.Deleted
            }.ToArray();
        }

        public ActionFieldChangedEnum[] Actions { get; set; }

    }
}
