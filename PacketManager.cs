using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class PacketManager
{
    public static PacketManager Instance { get; set; }
    private Dictionary<int, Action<Packet>> packetHandlers;

    public PacketManager()
    {
        Instance = this;
        packetHandlers = new Dictionary<int, Action<Packet>>();
    }

    public void RegisterPacketHandler(int packetId, Action<Packet> handler)
    {
        if (!packetHandlers.ContainsKey(packetId))
        {
            packetHandlers.Add(packetId, handler);
        }
        else
        {
            throw new Exception("Packet handler for this ID already exists.");
        }
    }

    public void UnregisterPacketHandler(int packetId)
    {
        if (packetHandlers.ContainsKey(packetId))
        {
            packetHandlers.Remove(packetId);
        }
        else
        {
            throw new Exception("No packet handler found for this ID.");
        }
    }

    public void HandleIncomingPacket(Packet packet)
    {
        if (packetHandlers.ContainsKey(packet.Id))
        {
            packetHandlers[packet.Id].Invoke(packet);
        }
        else
        {
            throw new Exception("No packet handler found for this ID.");
        }
    }

    public void SendPacket(UdpClient socket, Packet packet)
    {
        socket.Send(packet.GetBytes());
    }

    public Packet ReceivePacket(UdpClient socket)
    {
        byte[] buffer = new byte[1024];
        int received = socket.Client.Receive(buffer, SocketFlags.None);

        if (received == 0)
        {
            throw new SocketException((int)SocketError.ConnectionReset);
        }

        byte[] data = new byte[received];
        Array.Copy(buffer, data, received);

        Packet packet = new Packet();

        packet.Id       = BitConverter.ToInt32(data, 0);
        packet.ToWhom   = BitConverter.ToInt32(data, 4);
        packet.FromWhom = BitConverter.ToInt32(data, 8);
        
        packet.Data = new byte[received - 12];
        Array.Copy(data, 12, packet.Data, 0, received - 12);

        return packet;
    }
}
