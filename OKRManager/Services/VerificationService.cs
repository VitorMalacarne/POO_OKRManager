


namespace OkrManager.Services
{
    public class VerificationService
    {
        public string VerifyIsNotNull(string entry)
        {
            while(string.IsNullOrEmpty(entry))
            {
                Console.WriteLine("O campo não pode ser nulo!");
                Console.WriteLine("Inisira um dado não nulo:");
                entry = Console.ReadLine();
            }
            return entry;
        }
        public int VerifyIsNumber(string entry)
        {
            int number;
            while(!int.TryParse(entry, out number))
            {
                Console.WriteLine("Por favor, digite apenas números:");
                entry = Console.ReadLine();
            }
            return number;
        }

        public decimal VerifyIsDecimal(string entry)
        {
            decimal number;
            while (!decimal.TryParse(entry, out number))
            {
                Console.WriteLine("Por favor, digite apenas números decimais:");
                entry = Console.ReadLine();
            }
            return number;
        }


    }
}