
public class Packet
{
    public int Id { get; set; }
    public byte[] Data { get; set; }

    public byte[] GetBytes()
    {
        return new byte[0];
    }
}