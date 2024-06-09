using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace ESP32.Models
{
    public class MQTT
    {
        Dispositivo esp {  get; set; }
        string clienteID { get; set; } //Pedro
        string servidor {  get; set; } //test.mosquitto.org
        int porta { get; set; } //1883
        string topico { get; set; } // espdash/automacao/sensor
        IMqttClient mqttClient { get; set; }



        public MQTT(Dispositivo esp, string clienteID, string servidor, int porta, string topico)
        {
            this.esp = esp;
            this.clienteID = clienteID;
            this.servidor = servidor;
            this.porta = porta;
            this.topico = topico;

            conectarMQTT();

        }

        async public void conectarMQTT()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();


            var options = new MqttClientOptionsBuilder()
                .WithClientId(clienteID) 
                .WithTcpServer(servidor, porta)
                .WithCleanSession()
                .Build();

            var connectResult = await mqttClient.ConnectAsync(options);

            if(connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"Conexão realizada com sucesso para: Servidor - {servidor}, Porta - {porta}");
                inscreverTopico();

            }
            else
            {
                Console.WriteLine($"Erro na conexão para: Servidor - {servidor}, Porta - {porta}");
            }

        }

        async public void inscreverTopico()
        {
            try
            {
                var subscribeResult = await mqttClient.SubscribeAsync(topico);
                Console.WriteLine($"Inscrito no tópico com sucesso.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha ao inscrever no tópico. {ex.Message}");
            }
            

        }

    }
}
