/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * User: Garraty
 * Date: 15.09.2015
 * Time: 13:22
 * 
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */
using System;
using System.Net;					//For Dns, IPHostEntry, IPAddress
using System.Net.Sockets; //For	SocketException

namespace T1_IPAddress
{
	class IPAddressExample
	{
		static void PrintHostInfo(string host)
		{
			try 
			{
				IPHostEntry hostInfo;
				
				//Attempt to resolve DNS for given host or address
				hostInfo = Dns.GetHostEntry(host);
				
				//Display the primary host name
				Console.WriteLine("\tCanonical Name:	" + hostInfo.HostName);
				
				//Display list of IP addresses for this host
				Console.WriteLine("\tIP Addresses:	");
				foreach(IPAddress ipAddr in hostInfo.AddressList)
				{
					Console.Write(ipAddr.ToString() + " ");
				}
				Console.WriteLine();
				
				//Display list of alias names for this host
				Console.Write("\tAliases:	");
				foreach(string alias in hostInfo.Aliases)
				{
					Console.Write(alias + " ");
				}
				Console.WriteLine("\n");
			}
			catch (Exception)
			{
				Console.WriteLine("\tUnable to resolve host: " + host + "\n");
			}
		}
		
		public static void Main(string[] args)
		{
			//Get and print local host info
			try
			{
				Console.WriteLine("Local Host:");
				string localHostName = Dns.GetHostName();
				Console.WriteLine("\tHost Name:	" + localHostName);
				
				PrintHostInfo(localHostName);
			}
			catch (Exception)
			{
				Console.WriteLine("Unable to resolve local host\n");
			}
			
			//Get and print info for hosts given on command line
			foreach (string arg in args)
			{
				Console.WriteLine(arg + ":");
				PrintHostInfo(arg);
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}