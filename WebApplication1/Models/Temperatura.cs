namespace ESP32.Models
{
    public class Temperatura
    {
        int id_temperatura {  get; set; }
        int id_dispositivo { get; set; }
        float temperatura { get; set; }
        DateTime data {  get; set; }
    }
}
