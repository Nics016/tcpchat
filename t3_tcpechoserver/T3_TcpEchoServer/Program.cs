/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * User: Garraty
 * Date: 17.09.2015
 * Time: 20:46
 * 
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */
using System;							//For Console, Int32, ArgumentException, Enviroment
using System.Net; 				//For IPAddress
using System.Net.Sockets; //For TcpListener, TcpClient

namespace T3_TcpEchoServer
{
	class TcpEchoServer
	{
		private const int BUFSIZE = 32; //Size of receive buffer
		
		public static void Main(string[] args)
		{
			if (args.Length > 1) //Test for correct # of args
				throw new ArgumentException("Parameters: [<Port>]");
			
			int servPort = (args.Length == 1) ? Int32.Parse(args[0]) : 7;
			
			TcpListener listener = null;
			
			try
			{
				//Create TcpListener to accept client connections
				IPAddress newIP;
				//newIP = IPAddress.Any;
				newIP = IPAddress.Parse("127.0.0.1");
				Console.WriteLine("Starting server on IP - {0}:{1}", newIP.ToString(), servPort);
				listener = new TcpListener(newIP, servPort);
				listener.Start();
			}
			catch (SocketException se)
			{
				Console.WriteLine(se.ErrorCode + " : " + se.Message);
				Environment.Exit(se.ErrorCode);
			}
			
			byte[] rcvBuffer = new byte[BUFSIZE]; 	// Receive buffer
			int bytesRcvd;													// Received bytes count
			for (;;)
			{
				//Run forever, accepting and servicing connections (I bless you)
				TcpClient client = null;
				NetworkStream netStream = null;
				
				try
				{
					Console.WriteLine("Waiting for the client to connect...");
					client = listener.AcceptTcpClient(); //Get client connection
					netStream = client.GetStream();
					Console.Write("Handling client - ");
					
					//Receive until client closes connection, indicated by 0 return value
					int totalBytesEchoed = 0;
					while ((bytesRcvd = netStream.Read(rcvBuffer, 0, rcvBuffer.Length)) > 0)
					{
						netStream.Write(rcvBuffer, 0, bytesRcvd);
						totalBytesEchoed += bytesRcvd;
					}
					
					Console.WriteLine("echoed {0} bytes.", totalBytesEchoed);
					
					//Close the stream and socket. We are done with this Client!
					netStream.Close();
					client.Close();
				}
				
				catch (Exception e)
				{
					Console.WriteLine("Error: {1}", e.Message);
					netStream.Close();
				}
			}
		}
	}
}