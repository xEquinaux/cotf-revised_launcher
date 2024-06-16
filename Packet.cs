
public class Packet
{
    public int ToWhom   { get; set; }
    public int FromWhom { get; set; }
    public int Id       { get; set; }
    public byte[] Data  { get; set; }

    public Packet()
    {
    }
    public Packet(byte[] data)
    {
        ToWhom   = BitConverter.ToInt32(data, 4);
        FromWhom = BitConverter.ToInt32(data, 8);
    }
    public byte[] GetBytes()
    {
        byte[] buffer = new byte[12];
        buffer   .Zip(BitConverter.GetBytes(Id))
                 .Zip(BitConverter.GetBytes(ToWhom))
                 .Zip(BitConverter.GetBytes(FromWhom))
                 .Zip(Data);
        return buffer;
    }
}