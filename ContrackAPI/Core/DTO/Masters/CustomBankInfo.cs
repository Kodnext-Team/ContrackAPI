namespace ContrackAPI
{
    public class CustomBankInfo
    {
        public string BankAccountUUID { get; set; } = Guid.NewGuid().ToString();
        public string Currency { get; set; } = "USD";
        public string AliasName { get; set; } = "";
        public List<KeyValuePair> BankAttributes { get; set; } = new List<KeyValuePair>();
        public void FillBankKeyValues()
        {
            if (BankAttributes.Count == 0)
            {
                BankAttributes.Add(new KeyValuePair() { KeyName = "Account Name" });
                BankAttributes.Add(new KeyValuePair() { KeyName = "Account Number" });
                BankAttributes.Add(new KeyValuePair() { KeyName = "Bank Name" });
                BankAttributes.Add(new KeyValuePair() { KeyName = "IBAN" });
                BankAttributes.Add(new KeyValuePair() { KeyName = "SWIFT/BIC" });
                BankAttributes.Add(new KeyValuePair() { KeyName = "Branch Code" });
                //BankAttributes.Add(new KeyValuePair() { KeyName = "Currency" });
            }
            var totalcount = BankAttributes.Count;
            for (int i = 0; i < 10 - totalcount; i++)
            {
                BankAttributes.Add(new KeyValuePair() { });
            }

        }
    }
}