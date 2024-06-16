using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class PacketManager
{
    private Dictionary<int, Action<Packet>> packetHandlers;

    public PacketManager()
    {
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

    public void SendPacket(Socket socket, Packet packet)
    {
        socket.Send(packet.GetBytes());
    }

    public Packet ReceivePacket(Socket socket)
    {
        byte[] buffer = new byte[1024];
        int received = socket.Receive(buffer, SocketFlags.None);

        if (received == 0)
        {
            throw new SocketException((int)SocketError.ConnectionReset);
        }

        byte[] data = new byte[received];
        Array.Copy(buffer, data, received);

        Packet packet = new Packet();

        packet.Id = BitConverter.ToInt32(data, 0);
        
        packet.Data = new byte[received - 4];
        Array.Copy(data, 4, packet.Data, 0, received - 4);

        return packet;
    }
}
