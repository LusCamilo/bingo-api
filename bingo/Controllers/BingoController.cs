using bingo;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BingoController : ControllerBase
    {
        private const int MaxValue = 10;
        private const int MinValue = 1;
        private static int? _randomNumber = null;
        private static readonly Random s_randNum = new();
        private readonly ILogger<BingoController> _logger;

        public BingoController(ILogger<BingoController> logger)
        {
            _logger = logger;
        }

        private void SetPrevRandomNumber(int prevRandomNumber)
        {
            _randomNumber = prevRandomNumber;
        }

        private int? GetPrevRandomNumber()
        {
            return _randomNumber;
        }

        private static int GenerateRandomNumber()
        {
            return s_randNum.Next(MinValue, MaxValue);
        }

        private bool CheckBingo(int randomNumber)
        {
            int? prevRandomNumber = GetPrevRandomNumber();

            if (prevRandomNumber != null)
            {
                int numberResult = randomNumber - prevRandomNumber.Value;

                if (numberResult < 3)
                {
                    return false;
                }

                for (int i = 2; i <= Math.Sqrt(numberResult); i++)
                {
                    if (numberResult % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        [HttpGet(Name = "GetBingo")]
        public BingoResponse Get()
        {
            int randomNumber = GenerateRandomNumber();

            int? prevNumber = GetPrevRandomNumber();

            bool bingoResult = CheckBingo(randomNumber);

            SetPrevRandomNumber(randomNumber);

            return new BingoResponse
            {
                Number = randomNumber,
                Bingo = bingoResult,
                PrevNumber = prevNumber
            };
        }
    }
}
