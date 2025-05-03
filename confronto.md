# Confronto TCP vs UDP in C#

## 1. Server Setup
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
TcpListener listener = new TcpListener(IPAddress.Any, 12345);
listener.Start();
``` | ```csharp
UdpClient udpServer = new UdpClient(12345);
``` | TCP richiede handshake, UDP no |

---

## 2. Client Connection
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
TcpClient client = new TcpClient("127.0.0.1", 12345);
``` | ```csharp
UdpClient udpClient = new UdpClient();
IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Loopback, 12345);
``` | UDP non stabilisce connessioni |

---

## 3. Ricezione Dati
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
NetworkStream stream = client.GetStream();
byte[] buffer = new byte[1024];
int bytesRead = stream.Read(buffer, 0, buffer.Length);
``` | ```csharp
UdpReceiveResult result = await udpClient.ReceiveAsync();
byte[] data = result.Buffer;
IPEndPoint sender = result.RemoteEndPoint;
``` | TCP: Stream continuo<br>UDP: Riceve pacchetti interi |

---

## 4. Invio Dati
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
string message = "Hello";
byte[] data = Encoding.ASCII.GetBytes(message);
stream.Write(data, 0, data.Length);
``` | ```csharp
string message = "Hello";
byte[] data = Encoding.ASCII.GetBytes(message);
await udpClient.SendAsync(data, data.Length, serverEndpoint);
``` | TCP: Garantisce ordine<br>UDP: Possibile perdita/riordino |

---

## 5. Gestione Client Multipli
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
while(true) {
  TcpClient client = listener.AcceptTcpClient();
  _ = HandleClientAsync(client);
}
``` | ```csharp
while(true) {
  var result = await udpServer.ReceiveAsync();
  _ = HandlePacketAsync(result);
}
``` | TCP: Thread per client<br>UDP: Singolo thread per tutti |

---

## 6. Chiusura Connessione
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
stream.Close();
client.Close();
``` | ```csharp
udpClient.Close();
``` | UDP non ha connessioni da chiudere |

---

## 7. Gestione Errori
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
try {
  // Operazioni di rete
} catch(SocketException ex) {
  // Gestione errori
}
``` | ```csharp
try {
  // Operazioni di rete
} catch(SocketException ex) {
  // Gestione errori
}
``` | Entrambi usano SocketException |

---

## 8. Utilizzo Stringhe
| **TCP** | **UDP** | **Note** |
|---------|---------|----------|
| ```csharp
using StreamReader reader = new StreamReader(stream);
string message = await reader.ReadLineAsync();
``` | ```csharp
var result = await udpClient.ReceiveAsync();
string message = Encoding.ASCII.GetString(result.Buffer);
``` | TCP: Stream-based<br>UDP: Conversione manuale |

---

## Tabella Comparativa
| Caratteristica        | TCP                          | UDP                          |
|-----------------------|------------------------------|------------------------------|
| **Tipo**              | Connection-oriented          | Connectionless               |
| **Affidabilità**      | Garantita                    | Best-effort                  |
| **Ordine pacchetti**  | Garantito                    | Non garantito                |
| **Throughput**        | Moderato (overhead)          | Alto (minimo overhead)       |
| **Latenza**           | Più alta (handshake)         | Più bassa                    |
| **Uso tipico**        | File transfer, Web, Email    | Streaming, Giochi, VoIP      |
| **Limiti dimensione** | Illimitato (stream)          | ~65KB per pacchetto          |