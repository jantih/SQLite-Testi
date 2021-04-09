namespace SQLite_console
{
    class Program
    {
        static void Main()
        {
            SQL SQL_Osoite = new();
            string haettuOsoite = HaeOsoite(SQL_Osoite);
            YhdistäSQL(SQL_Osoite, haettuOsoite);
            SQL_Osoite.Keke = "Kakka";
            System.Console.WriteLine(SQL_Osoite.Keke);
        }
        static string HaeOsoite(SQL SQL_Osoite)
        {
            string osoite = SQL_Osoite.HaeOsoite();
            return osoite;
        }
        static void YhdistäSQL(SQL Yhteys, string Osoite)
        {
            Yhteys.SQLiteYhteys(Osoite);
        }
    }
}