namespace api.Utils
{
    public static class StaticCissaReferences
    {
        public static Guid PrimaryApplicationType { get; }
            = new("{7AFE2E28-EC80-4F1D-AB72-A302F57C38A9}");
        public static Guid SecondaryApplicationType { get; set; }
            = new("{48306CF9-3B20-427C-B3D5-2C6EC4B7FF09}");
        public static Guid MALE { get; } = new("{C3DCB977-2781-418A-BB96-12FE7F3F041B}");
        public static Guid FEMALE { get; } = new("{BC064CB6-0EF7-4535-9208-4288EA6EFD21}");
        public static Guid PASSPORT_DOCUMENT_TYPE { get; }
            = new("{5E2765E7-66F2-4F98-A6FF-CDB24F264BB5}");
        public static Guid PASSPORT_SERIES_ID { get; } = new("{775C31E7-F373-4532-848E-42FBED672F35}");
        public static Guid PASSPORT_SERIES_AN { get; } = new("{B3C7B962-4A81-44F6-93B3-B2D9277C6C91}");
        public static Guid PASSPORT_SERIES_AC { get; } = new("{9E831652-95F3-4B17-ACE1-48F657DD959C}");
        public static Guid PAYMENT_TYPE_UBK { get; } = new("{3193A90D-380B-4428-B7B1-04C548AA902E}");
    }
}
