namespace src.Dtos
{
    using src.Domain;
    using System;

    public class HistoryDto
    {
        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public ActionFieldChangedEnum ActionType { get; set; }
        public string ActionTypeValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
