namespace MD5Hash.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net70)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Benchmarks
{
    [Params(10, 50, 100, 1_000, 10_000)]
    public int Size { get; set; }

    private readonly string data;

    public Benchmarks()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        data = new string(Enumerable.Repeat(chars, Size).Select(static s => s[Random.Shared.Next(s.Length)]).ToArray());
    }

    [Benchmark]
    public string Md5WithStackallockAndStatic()
    {
        var buffer = Encoding.UTF8.GetBytes(data);

        Span<byte> hash = stackalloc byte[MD5.HashSizeInBytes];
        MD5.HashData(buffer, hash);

        return Convert.ToHexString(hash);
    }

    [Benchmark]
    public string Md5KWithUsing()
    {
        using var hashAlgorithm = MD5.Create();
        var plainBytes = Encoding.UTF8.GetBytes(data);
        var encodedBytes = hashAlgorithm.ComputeHash(plainBytes);
        return ToHexString(encodedBytes);
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

