/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * User: Garraty
 * Date: 15.09.2015
 * Time: 17:16
 * 
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */
using System;							//For string, Int32, Console, ArgumentException
using System.Text; 				//For Encoding
using System.IO;					//For IOException 
using System.Net.Sockets;	//For TcpClient, NewtworkStream, SocketException

namespace T2_TcpEchoClient
{
	class TcpEchoClient
	{
		public static void Main(string[] args)
		{
			if (args.Length < 2 || args.Length > 3) 
			{
				//test for correct # of args
				throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
			}
			
			string server = args[0]; // Server name of IP address
			
			//Convert input string to bytes
			byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);
			
			//Use port argument if supplied, otherwise default to 7
			int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;
			
			TcpClient client = null;
			NetworkStream netStream = null;
			
			try
			{
				//Create socket that is connected to server on specified port
				client = new TcpClient();
				client.Connect(server, servPort);
				
				Console.WriteLine("Connected to server... sending echo string");
				
				netStream = client.GetStream();
				
				//Send the echoed string to the server
				netStream.Write(byteBuffer, 0, byteBuffer.Length);
				
				Console.WriteLine("Sent {0} bytes to server...", byteBuffer.Length);
				
				int totalBytesRcvd = 0; //Total bytes recieved so far
				int bytesRcvd = 0; 			//Bytes received in last read
				
				//Receive the same string back from the server
				while (totalBytesRcvd < byteBuffer.Length)
				{
					if ((bytesRcvd = netStream.Read(byteBuffer, totalBytesRcvd,
					                                byteBuffer.Length - totalBytesRcvd)) == 0)
					{
						Console.WriteLine("Connection closed prematurely.");
						break;
					}
					totalBytesRcvd += bytesRcvd;
				}
				Console.WriteLine("Received {0} bytes from server: {1}", totalBytesRcvd, Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd));
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (netStream != null)
					netStream.Close();
				if (client != null)
					client.Close();
			}
			
			Console.Write("\n\nPress any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}