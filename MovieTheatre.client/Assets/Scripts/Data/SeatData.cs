namespace Data
{
    public class SeatData
    {
        public int SeatNumber { get; set; }//right two digits - number, others - raw; 1105 - 5th seat in 11th raw 
        public EventData EventData { get; set; }
        public bool Reserved { get; set; }
        public float Cost { get; set; }
        public string ClientName { get; set; }
    }
}