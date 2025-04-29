using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class StudentUDPClient 
{ 
    public static void Main() 
    { 
        // Get the host name
        string hostName = Dns.GetHostName(); 
        Console.WriteLine($"Host Name: {hostName}");
        UdpClient udpc = new UdpClient(hostName, 7878); 
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 7878); 
        while (true) 
        { 
            Console.Write("Enter Your Name: "); 
            string studentName = Console.ReadLine() ?? ""; 
            
          // Check weather student entered name to start conversation 
              if (string.IsNullOrWhiteSpace(studentName)){ 
              Console.WriteLine("You did not enter your name. Closing..."); 
              //send empty string to server
              byte[] closeMSG = Encoding.ASCII.GetBytes("");
              udpc.Send(closeMSG, closeMSG.Length);
              // close the socket
              udpc.Close();
              break; 
            } 
              // Data to send 
            byte[] msg = Encoding.ASCII.GetBytes(studentName); 
            udpc.Send(msg, msg.Length); 
            
          // received Data 
            byte[] rdata = udpc.Receive(ref ep); 
            string job = Encoding.ASCII.GetString(rdata); 
            Console.WriteLine(job); 
        } 
    } 
} 