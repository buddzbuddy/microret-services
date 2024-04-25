namespace api.Models.BL
{
    public class WorkPeriodInfoDTO
    {
        public string? State { get; set; }
        public string? PIN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Issuer { get; set; }
        public Item[]? WorkPeriods { get; set; }
        public class Item
        {
            public string? PIN_LSS { get; set; }
            public string? Payer { get; set; }
            public string? INN { get; set;}
            public long? NumSF { get; set;}
            /// <summary>
            /// Use FORMAT
            /// </summary>
            public string? DateBegin { get;set; }
            public string? DateEnd { get; set; }
            public double? Sum { get; set; }
        }
    }
}
