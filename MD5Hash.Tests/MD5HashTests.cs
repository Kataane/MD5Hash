namespace MD5Hash.Tests;

public class Md5HashTests
{
    [Fact]
    public void MD5HashWithUsing_String_Hash()
    {
        var email = "strange@email.com";

        var buffer = Encoding.UTF8.GetBytes(email);

        Span<byte> hash = stackalloc byte[MD5.HashSizeInBytes];
        MD5.HashData(buffer, hash);

        var actual = Convert.ToHexString(hash);

        Assert.Equal("152F2DD5F1F056FC761717EA5965828C", actual);
    }

    [Fact]
    public void MD5StackallockWithStatic_String_Hash()
    {
        var email = "strange@email.com";

        using var hashAlgorithm = MD5.Create();
        var plainBytes = Encoding.UTF8.GetBytes(email);
        var encodedBytes = hashAlgorithm.ComputeHash(plainBytes);

        var actual = ToHexString(encodedBytes);

        Assert.Equal("152F2DD5F1F056FC761717EA5965828C", actual);
    }

    public static string ToHexString(IEnumerable<byte> values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        var result = new StringBuilder();
        foreach (var value in values)
            result.Append(ToHexString(value));

        return result.ToString();
    }

    public static string ToHexString(byte value)
    {
        return value.ToString("X2", CultureInfo.InvariantCulture);
    }
}