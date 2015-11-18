using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

// UdpPacket provides packetIO over UDP
public class UDPPacketIO : MonoBehaviour {
	private UdpClient Sender;
	private UdpClient Receiver;
	private bool socketsOpen;
	private string remoteHostName;
	private int remotePort;
	private int localPort;
	
	public void init(string hostIP, int remotePort, int localPort){
		RemoteHostName = hostIP;
		RemotePort = remotePort;
		LocalPort = localPort;
		socketsOpen = false;
	}

	~UDPPacketIO() { if (IsOpen()) Close(); }
	
	public bool Open() {
		try {
			Sender = new UdpClient();
			IPEndPoint listenerIp = new IPEndPoint(IPAddress.Any, localPort);
			Receiver = new UdpClient(listenerIp);
			socketsOpen = true;
			return true;
		} catch (Exception e) {
			Debug.LogWarning("cannot open udp client interface at port "+localPort);
			Debug.LogWarning(e);
		}
		return false;
	}

	public void Close() {
		if(Sender != null)
			Sender.Close();
		if (Receiver != null) {
			Receiver.Close();
		}
		Receiver = null;
		socketsOpen = false;
	}
	
	public void OnDisable() { Close(); }

	public bool IsOpen() { return socketsOpen; }

	public void SendPacket(byte[] packet, int length) {
		if (!IsOpen()) Open();
		if (!IsOpen()) return;
		Sender.Send(packet, length, remoteHostName, remotePort);
	}

	public int ReceivePacket(byte[] buffer) {
		if (!IsOpen()) Open();
		if (!IsOpen()) return 0;
		IPEndPoint iep = new IPEndPoint(IPAddress.Any, localPort);
		byte[] incoming = Receiver.Receive( ref iep );
		int count = Math.Min(buffer.Length, incoming.Length);
		System.Array.Copy(incoming, buffer, count);
		return count;
	}

	public string RemoteHostName {
		get { return remoteHostName; }
		set { remoteHostName = value; }
	}

	public int RemotePort {
		get { return remotePort; }
		set { remotePort = value; }
	}

	public int LocalPort {
		get { return localPort; }
		set { localPort = value; }
	}
}
