
public class Packet
{
    public int ToWhom { get; set; }
    public int FromWhom { get; set; }
    public int Id { get; set; }
    public byte[] Data { get; set; }

    public byte[] GetBytes()
    {
        return new byte[0];
    }
}