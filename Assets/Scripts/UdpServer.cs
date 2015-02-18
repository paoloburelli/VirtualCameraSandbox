using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpServer
{
	private static int port;

	public UdpServer (int port)
	{
		UdpServer.port = port;
		//UdpServer.sm = sm;
		Thread newThrd = new Thread (new ThreadStart (UdpServer.Run));
		newThrd.Start ();
	}
	
	public static void Run ()
	{
		int recv;
		byte[] data;
		IPEndPoint ipep = new IPEndPoint (IPAddress.Any, port);
		
		Socket newsock = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		
		newsock.Bind (ipep);
		
		IPEndPoint sender = new IPEndPoint (IPAddress.Any, 0);
		EndPoint Remote = (EndPoint)(sender);
		
		//sm.DebugText = "Waiting for commands on port: " + port;
		
		while (true) {
			Thread.Sleep (10);
			data = new byte[65535];
			recv = newsock.ReceiveFrom (data, ref Remote);
			//sm.DebugText = "Command received: " + Encoding.ASCII.GetString (data, 0, recv);
			double now = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
			//string returnValue = sm.NotifyRemoteCommand (Encoding.ASCII.GetString (data, 0, recv));
			//data = Encoding.ASCII.GetBytes (returnValue);
			//newsock.SendTo (data, returnValue.Length, SocketFlags.None, Remote);
			//sm.DebugText = "Command executed in " + (System.DateTime.Now.TimeOfDay.TotalMilliseconds - now).ToString ("0.00") + "ms (" + returnValue + ")";
		}
	}
}

