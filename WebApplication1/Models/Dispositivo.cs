namespace ESP32.Models
{
    public class Dispositivo : MQTT
    {
        public int id {  get; set; }
        public string nome { get; set; }
        public int latitude { get; set; }
        public int longitude { get; set; }
        /*
        public string clienteID { get; set; }
        public string servidor { get; set; }
        public int porta { get; set; }
        public string topico { get; set; }
        */

        public Dispositivo(string nome, int latitude, int longitude, string clienteID, string servidor, int porta, string topico) : base(clienteID,  servidor,  porta,  topico)
        {
            this.nome = nome;
            this.latitude = latitude;
            this.longitude = longitude;

        }
        
        public Dispositivo()
        {

        }

        public void insertDispositivo()
        {

        }

        public void alterDispositivo()
        {

        }

        public void selectDispositivoId(int id)
        {

        }

        public void selectDispositivoAll()
        {

        }
    }

}
