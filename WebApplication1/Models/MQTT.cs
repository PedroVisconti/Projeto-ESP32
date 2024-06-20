using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Threading;

namespace ESP32.Models
{
    public class MQTT
    {
        public string clienteID { get; set; } //Pedro
        public string servidor { get; set; } //test.mosquitto.org
        public int porta { get; set; } //1883
        public string topico { get; set; } // espdash/automacao/sensor
        IMqttClient mqttClient { get; set; }



        public MQTT(string clienteID, string servidor, int porta, string topico)
        {

            this.clienteID = clienteID;
            this.servidor = servidor;
            this.porta = porta;
            this.topico = topico;

        }

        public MQTT()
        {

        }
        async public void conectarMQTT()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();


            var options = new MqttClientOptionsBuilder()
                .WithClientId(this.clienteID)
                .WithTcpServer(this.servidor, this.porta)
                .Build();

            var connectResult = await mqttClient.ConnectAsync(options);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"Conexão realizada com sucesso para: Servidor - {this.servidor}, Porta - {this.porta}");

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine("Received application message.");
                    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                    return Task.CompletedTask;
                };

                inscreverTopico();
            }
            else
            {
                Console.WriteLine($"Erro na conexão para: Servidor - {this.servidor}, Porta - {this.porta}");
            }

        }

        public void inscreverTopico()
        {
            try
            {
                var subscribeResult = mqttClient.SubscribeAsync(this.topico);
                Console.WriteLine($"Inscrito no tópico com sucesso.{this.topico}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha ao inscrever no tópico. {ex.Message}");
            }

        }

        async public void EnviarMensagem()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(this.clienteID)
                .WithTcpServer(this.servidor, this.porta)
                .Build();

            await mqttClient.ConnectAsync(options);

            await Task.Delay(10000);

            string message = "Teste MQTT";

            var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(this.topico)
            .WithPayload(message)
            .Build();



            await mqttClient.PublishAsync(mqttMessage);

            await mqttClient.DisconnectAsync();
            Console.WriteLine("Enviou");
        }

    }
}
