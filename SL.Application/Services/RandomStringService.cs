using System.Text;

namespace SL.Application.Services;

public class RandomStringService
{
    private const int DefaultLength = 6;
    private const string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345689";

    public string GetRandomString()
    {
        var random = new Random();

        var sb = new StringBuilder();
        while (sb.Length != DefaultLength)
        {
            var rnd = random.Next(0, CharSet.Length);
            sb.Append(CharSet[rnd]);
        }

        return sb.ToString();
    }
}