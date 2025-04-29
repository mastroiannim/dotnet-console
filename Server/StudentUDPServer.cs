using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class StudentUDPServer
{
    public static void Main()
    {
        // Ottieni il nome host corrente
        string hostName = Dns.GetHostName();
        Console.WriteLine($"Host Name: {hostName}");

        UdpClient udpc = new UdpClient(7878); 
        Console.WriteLine("Server Started, servicing on port no. 7878"); 
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 7878); 
        while (true){ 
            
          // Store received data from client 
            byte[] receivedData = udpc.Receive(ref ep); 
            
            string studentName = Encoding.ASCII.GetString(receivedData); 
            string msg;
            if (string.IsNullOrWhiteSpace(studentName)){
                msg = "No such Student available for conversation"; 
                Console.WriteLine(msg);
                break;
            }
            msg = "Hello " + studentName + ", welcome to the server!";
            byte[] sdata = Encoding.ASCII.GetBytes(msg); 
            udpc.Send(sdata, sdata.Length, ep); 
            Console.WriteLine($"Received from {ep.Address}:{ep.Port} - {studentName}");
            Console.WriteLine($"Sent to {ep.Address}:{ep.Port} - {msg}");

            System.Threading.Thread.Sleep(100);
        } 
        // Chiude il socket
        udpc.Close();
    }
}