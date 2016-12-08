/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * User: Garraty
 * Date: 02.10.2015
 * Time: 9:41
 * 
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */
using System;												// String, Int32, Console, ArgumentException
using System.Text;									// Encoding
using System.IO;										// IOException
using System.Net.Sockets;						// Socket, SocketException
using System.Net;										// IPAddress, IPEndPoint

namespace T7_TcpEchoClientSocket
{
	class TcpEchoClientSocket
	{
		public static void Main(string[] args)
		{
			if (args.Length < 2 || args.Length > 3)
			{
				throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
			}
			
			string server = args[0];	// Server name or IP address
			
			// Convert string to bytes
			byte[] byteBuffer = Encoding.UTF8.GetBytes(args[1]);
			
			// Use port argument if supplied or default to 7
			int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;
			
			Socket sock = null;
			try
			{
				// Create a TCP socket instance
				sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
				                  ProtocolType.Tcp);
				
				// Creates server IPEndPoint instance. We assume Resolve returns
				// at least one address
				IPEndPoint serverEndPoint = new IPEndPoint(Dns.GetHostEntry(server).AddressList[0],
				                                           servPort);
				
				// Connect the socket to the server on specified port
				sock.Connect(serverEndPoint);
				Console.WriteLine("Connected to server... sending echo string");
				
				// Send the encoded string to server
				sock.Send(byteBuffer, 0, byteBuffer.Length, SocketFlags.None);
				
				Console.WriteLine("Sent {0} bytes to the server...", byteBuffer.Length);
				
				int totalBytesRcvd = 0;		// Total bytes rcvd so far
				int bytesRcvd = 0;				// Bytes rcvd in last read
				
				//receive the same string back from the server
				while (totalBytesRcvd < byteBuffer.Length)
				{
					bytesRcvd = sock.Receive(byteBuffer, totalBytesRcvd, byteBuffer.Length - totalBytesRcvd, SocketFlags.None);
					if (bytesRcvd == 0)
					{
						Console.WriteLine("Connection closed prematurely.");
						break;
					}
					totalBytesRcvd += bytesRcvd;
				}
				
				string sAnswer = Encoding.UTF8.GetString(byteBuffer);
				Console.WriteLine("Received {0} bytes from server: '{1}'", totalBytesRcvd, sAnswer);
				                  
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				sock.Close();
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}