using System;
using System.Data.SQLite;

namespace SQLite_console
{
    class SQL
    {
        // -- Määritä DB sijainti -- //
        public string HaeOsoite()
        {
            string Osoite = "Data Source=C:\\sqlite\\test.db";
            Console.Write("Määritelläänkö uusi DB (1/0): ");
            int.TryParse(Console.ReadLine(), out int vastaus);

            if (vastaus == 1)
            {
                Console.Write("Nimeä uusi DB: ");
                string DB = Console.ReadLine();
                Osoite = $"Data Source=C:\\sqlite\\{DB}.db";
                return Osoite;
            }
            else {
                return Osoite;
            }
        }

        // -- Määritä SQL ja tee muutokset -- //
        public void SQLiteYhteys(string osoite)
        {
            SQLiteConnection yhteys = new(osoite);
            SQLiteCommand cmd = new(yhteys);

            try { 
                yhteys.Open();
                Console.WriteLine("Yhteys muodostettu @ "+osoite);
            }   catch(Exception ex) { Console.WriteLine("Virhe yhteydessä" + ex); }


            int vastaus = 1;
            // -- SQL SANDBOX -- //
            while(vastaus != 0) {
                Console.Write("\n1. Lue SQLite tietokanta\n2. Tuo uusi tieto\n8. Lajittele hinnan mukaan\n9. Alusta taulu\n0. Poistu\nAnna valintasi: ");

                int.TryParse(Console.ReadLine(), out vastaus);
                if(vastaus == 1)
                {
                    try { LueTiedot(); }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Tietokantaa ei ole luotu tai muu virhe...\n" + ex);
                    }
                    
                }
                if(vastaus == 2)
                {
                    LuoTieto();
                }
                else if (vastaus == 8)
                {
                    LajitteleHinta();
                }
                else if (vastaus == 9)
                {
                    AlustaJaLuoTaulu();
                }
            }
            void LueTiedot()
            {
                SQLiteCommand haeTieto = new("SELECT * FROM tuotteet", yhteys);
                SQLiteDataReader reader = haeTieto.ExecuteReader();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{reader.GetName(0), -3}{reader.GetName(1), -8}{reader.GetName(2), 12}{reader.GetName(3), 8}");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetInt32(0), -3}{reader.GetString(1), -8}{reader.GetString(2), 12}{reader.GetInt32(3), 8}");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            void LajitteleHinta()
            {
                SQLiteCommand lajitteleHinta = new("SELECT * FROM tuotteet ORDER BY Hinta ASC", yhteys); // Ei toimi'single quoteilla' eikä ilman..
                SQLiteDataReader reader = lajitteleHinta.ExecuteReader(); // Sama tässä, ei toimi ExecuteNonQuery eikä ExecuteReader
                Console.WriteLine($"\n{reader.GetName(0),-3}{reader.GetName(1),-8}{reader.GetName(2),12}{reader.GetName(3),8}");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetInt32(0),-3}{reader.GetString(1),-8}{reader.GetString(2),12}{reader.GetInt32(3),8}");
                }
            }
            void LuoTieto()
            {
                Console.Write("Anna merkki: ");
                string merkki = Console.ReadLine();
                Console.Write("Anna malli: ");
                string malli = Console.ReadLine();
                Console.Write("Hinta: ");
                int.TryParse(Console.ReadLine(), out int hinta);
                cmd.CommandText = $"INSERT INTO tuotteet(Merkki, Malli, Hinta) VALUES('{merkki}','{malli}',{hinta})";
                cmd.ExecuteNonQuery();
            }
            void AlustaJaLuoTaulu()
            {
                cmd.CommandText = "DROP TABLE IF EXISTS tuotteet";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE tuotteet(id INTEGER PRIMARY KEY, Merkki TEXT, Malli TEXT, Hinta INT)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO tuotteet(Merkki, Malli, Hinta) VALUES('Audi','A6',42642)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO tuotteet(Merkki, Malli, Hinta) VALUES('MB','SLK',57127)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO tuotteet(Merkki, Malli, Hinta) VALUES('Honda','Monkey',700)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO tuotteet(Merkki, Malli, Hinta) VALUES('Suzuki','PV',1000)";
                cmd.ExecuteNonQuery();

                Console.WriteLine("Taulukko: tuotteet ... alustettu!");
            }
        }
    }
}
