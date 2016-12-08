/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * User: Garraty
 * Date: 28.09.2015
 * Time: 22:09
 * 
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */
using System;							// Console, Int32, ArgumentException, Enviroment
using System.Net;					// IPEndPoint
using System.Net.Sockets; // UdpClient, SocketException

namespace T5_UdpEchoServer
{
	class UdpEchoServer
	{
		public static void Main(string[] args)
		{
			if (args.Length > 1)
			{
				throw new ArgumentException("Parameters: <Port");
			}
			
			int servPort = (args.Length == 1) ? Int32.Parse(args[0]) : 7;
			
			UdpClient client = null;
			
			try
			{
				//Create an instance of UdpClient on the port to listen on
				client = new UdpClient(servPort);
			}
			catch (SocketException se)
			{
				Console.WriteLine(se.ErrorCode + ": " + se.Message);
				Environment.Exit(se.ErrorCode);
			}
			
			// Create an IPEndPoint instance that will be passed as a reference
			// to the Recieve() call and be populated with the remote client info
			IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
			
			// Run forever
			for (;;)
			{
				try
				{
					// Receive a byte array with echo datagram packet contents
					byte[] byteBuffer = client.Receive(ref remoteIPEndPoint);
					Console.Write("Handling client at " + remoteIPEndPoint + " - ");
					
					// Send an echo packet back to the client
					client.Send(byteBuffer, byteBuffer.Length, remoteIPEndPoint);
					Console.WriteLine("echoed {0} bytes.", byteBuffer.Length);
				}
				
				catch (SocketException se)
				{
					Console.WriteLine(se.ErrorCode + ": " + se.Message);
				}
			}
			
		}
	}
}