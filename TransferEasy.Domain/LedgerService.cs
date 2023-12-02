namespace TransferEasy.Domain
{
    public interface ILedgerService
    {
        int AddAccount(string username);
        double GetAccountBalance(string username);
        void Transfer(double amount, int fromAccountId, int toAccountId);
        void Deposit(double amount, int toAccountId);
        void Withdraw(double amount, int fromAccountId);
        void ProcessTransaction(Transaction transaction);
    }

    public class LedgerService : ILedgerService
    {
        private const int CashAccountId = 1;
        private const int RevenueAccountId = 2;
        private const int ExpensesAccountId = 3;

        private const double CardProcessingFeePercentage = 2.0;
        private const double WithdrawalFeePercentage = 3.0;

        private readonly Ledger ledger;

        public LedgerService() => ledger = new Ledger(1, "TransferEasyOperations", new List<Account>()
            {
                new AccountDebitNormal(CashAccountId, "Cash"),
                new AccountCreditNormal(RevenueAccountId, "Revenue from Fees"),
                new AccountDebitNormal(ExpensesAccountId, "Card Processing Expenses")
            });

        public int AddAccount(string username)
        {
            var newAccountId = ledger.Accounts.Count() + 1;

            var account = new AccountCreditNormal(newAccountId, username);

            var accounts = ledger.Accounts.Append(account);
            ledger.Accounts = accounts;

            return newAccountId;
        }

        public void Transfer(double amount, int fromAccountId, int toAccountId)
        {
            // Validate business
            var fromAccount = ledger.Accounts.First(a => a.Id == fromAccountId);
            var toAccount = ledger.Accounts.First(a => a.Id == toAccountId);
            if (fromAccount.GetBalance() < amount) throw new Exception("Not enough money");

            // Create transaction
            var originEntry = new Entry(amount, EntryDirection.Debit);
            var destinationEntry = new Entry(amount, EntryDirection.Credit);

            var entries = new List<TransactionEntry>()
            {
                new(fromAccountId, originEntry),
                new(toAccountId, destinationEntry)
            };

            // Add transaction
            var nextTransactionId = ledger.Transactions.Count - 1;
            var transaction = new Transaction(nextTransactionId, entries, "UserTransfer", DateTime.UtcNow);

            ledger.Transactions.Add(transaction);

            ProcessTransaction(transaction);
        }

        public void Deposit(double amount, int toAccountId)
        {
            // Validate business
            var toAccount = ledger.Accounts.First(a => a.Id == toAccountId);
            var cardProcessingFee = amount * CardProcessingFeePercentage / 100;

            // Create transaction
            var entries = new List<TransactionEntry>()
            {
                new(toAccountId, new (amount, EntryDirection.Credit)),
                new(CashAccountId, new(amount - cardProcessingFee, EntryDirection.Debit)),
                new(ExpensesAccountId, new(cardProcessingFee, EntryDirection.Debit))
            };

            // Add transaction
            var nextTransactionId = ledger.Transactions.Count - 1;
            var transaction = new Transaction(1, entries, "Deposit", DateTime.UtcNow);

            ledger.Transactions.Add(transaction);

            ProcessTransaction(transaction);
        }

        public void Withdraw(double amount, int fromAccountId)
        {
            // Validate business
            var fromAccount = ledger.Accounts.First(a => a.Id == fromAccountId);
            var withdrawalFee = amount * WithdrawalFeePercentage / 100;
            if (fromAccount.GetBalance() < amount + withdrawalFee) throw new Exception("Not enough money");

            // Create transaction
            var entries = new List<TransactionEntry>()
            {
                new(fromAccountId, new (amount, EntryDirection.Debit)),
                new(CashAccountId, new(amount - withdrawalFee, EntryDirection.Credit)),
                new(RevenueAccountId, new(withdrawalFee, EntryDirection.Credit))
            };

            // Add transaction
            var nextTransactionId = ledger.Transactions.Count - 1;
            var transaction = new Transaction(1, entries, "Withdrawal", DateTime.UtcNow);

            ledger.Transactions.Add(transaction);

            ProcessTransaction(transaction);
        }

        public void ProcessTransaction(Transaction transaction)
        {
            if (IsValid(transaction))
            {
                foreach (var entry in transaction.Entries)
                {
                    var account = ledger.Accounts.First(a => a.Id == entry.AccountId);
                    account.Entries.Add(entry.Entry);
                }
            }
        }

        private static bool IsValid(Transaction transaction)
        {
            var debitSum = transaction.Entries.Where(entry => entry.Entry.Direction == EntryDirection.Debit).Sum(e => e.Entry.Amount);
            var creditSum = transaction.Entries.Where(entry => entry.Entry.Direction == EntryDirection.Credit).Sum(e => e.Entry.Amount);

            return debitSum == creditSum;
        }

        public double GetAccountBalance(string username)
        {
            var account = ledger.Accounts.FirstOrDefault(acc => acc.Name.Equals(username));

            if (account == null) return 0;

            return account.GetBalance();
        }
    }
}
