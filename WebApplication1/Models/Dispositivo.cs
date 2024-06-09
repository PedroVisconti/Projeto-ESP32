namespace ESP32.Models
{
    public class Dispositivo
    {
        int id {  get; set; }
        string nome { get; set; }
        int latitude { get; set; }
        int longitude { get; set; }

        public Dispositivo(int id, string nome, int latitude, int longitude)
        {
            this.id = id;
            this.nome = nome;
            this.latitude = latitude;
            this.longitude = longitude;
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
