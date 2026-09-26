using System.Runtime;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public class BankAccount
    {
        private int accountNumber;
        private string owner = string.Empty;
        private decimal balance;
        private static int counter = 0;
        public BankAccount(string ownernew)
        {
            Console.WriteLine($" кол-во экземпляров {BankAccount.counter}");
            owner = ownernew;
            counter += 1;
            accountNumber = GenerateAccountNumber();
            balance = 0;

        }
        public void Deposit(int amount)
        {
            if (isValidAmount(amount) == true)
            {
                balance += amount;
                Console.WriteLine($"Баланс успешно пополнен текущая сумма {balance}");
            }
           else
            {
                Console.WriteLine("данную сумму нельзя пополнить на баланс");
            }
        }
        public void Withdraw(int amount)
        {
           if (isValidAmount(amount) == true)
            {
                
                if ( balance < amount)
                {
                    Console.WriteLine("Недостаточно средств для снятия");
                }
                else
                {
                    balance -= amount;
                    Console.WriteLine($"сумма {amount} была успешно снята остаток на балансе {balance}");
                } 
            }
           else
            {
                Console.WriteLine("данную сумму нельзя пополнить на баланс");
            }
        }
        public void GetBalance()
        {
            Console.WriteLine(balance);
        }
        private bool isValidAmount(int amount)
        {
            if ( amount > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetTotalCounts()
        {
          
                return BankAccount.counter;
        }
        public static int GenerateAccountNumber()
        {
            Random generate = new Random();
            var generatenumber = generate.Next(1,100);
            return generatenumber;
            
            
        }
        public void GetAccountInfo()
        {
            Console.WriteLine($"{owner} , {accountNumber}");
        }
    }
    private static void Main()
    {
        BankAccount bankacc = new BankAccount("Олег") ;
        BankAccount bankacc1 = new BankAccount("Олег") ;
        bankacc.Deposit(100);
        bankacc.Withdraw(50);
        bankacc.GetBalance();
        bankacc.GetAccountInfo();
        BankAccount.GetTotalCounts();
       

    }
}