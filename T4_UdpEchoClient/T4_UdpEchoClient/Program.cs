/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * User: Garraty
 * Date: 28.09.2015
 * Time: 21:16
 * 
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */
using System;							// For string, Int32, Console
using System.Text;				// For Encoding
using System.Net;					// For IPEndPoint
using System.Net.Sockets; // For UdpClient, SocketException

namespace T4_UdpEchoClient
{
	class UdpEchoClient
	{
		public static void Main(string[] args)
		{
			if ((args.Length < 2) || (args.Length > 3))
			{
				//Test for correct # of args
				throw new System.ArgumentException("Parameters: <Server> <Word> [<Port>]");
			}
			
			// Server name or IP address
			string server = args[0];
			
			// Use port argument if supplied, otherwise default to 7
			int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;
			
			// Convert input string to an array of bytes
			byte[] sendPacket = Encoding.UTF8.GetBytes(args[1]);
			
			//create a udp client instance
			UdpClient client = new UdpClient();
			
			//try to send packet
			try
			{
				// Send the echo string to the specified host and port
				client.Send(sendPacket, sendPacket.Length, server, servPort);
				
				Console.WriteLine("Sent {0} bytes to the server...", sendPacket.Length);
				
				// This IPEndPoint instance will be populated with the remote sender's
				// endpoint information after the Receive() call
				IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
				
				//Attempt echo reply receive
				byte[] rcvPacket = client.Receive(ref remoteIPEndPoint);
				
				Console.WriteLine("Received {0} bytes from {1}: {2}", 
				                  rcvPacket.Length, remoteIPEndPoint,
				                  Encoding.UTF8.GetString(rcvPacket, 0, rcvPacket.Length));
			}
			catch (SocketException se)
			{
				Console.WriteLine(se.ErrorCode + ": " + se.Message);
			}
			
			client.Close();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}