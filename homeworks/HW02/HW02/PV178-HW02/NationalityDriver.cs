namespace HW02
{
    internal class NationalityDriver
    {
        public int driverID;
        public int totalResults = 0;
        public List<string> relevantResults = new List<string>();

        public NationalityDriver(int ID)
        {
            driverID = ID;
        }
    }
}
