using System;

namespace KoliMester3000
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("KoliMester 3000");
                Console.WriteLine("1 - Szoba létrehozása");
                Console.WriteLine("2 - Szobák lekérése");
                Console.WriteLine("3 - Lakó hozzáadása szobához");
                Console.WriteLine("66 - Konzol kiürítése");
                Console.WriteLine("67 - Kilépés");
                Console.Write("Írj be egy számot a folytatáshoz: ");

                bool valid = int.TryParse(Console.ReadLine(), out int menuNumber);
                if (valid)
                {
                    switch (menuNumber)
                    {
                        case 1:
                            Console.Clear();
                            Console.Write("Szoba neve: ");
                            string roomName = Console.ReadLine();
                            Console.Write("Férőhely: ");
                            int space;
                            while (!int.TryParse(Console.ReadLine(), out space) || space <= 0)
                            {
                                Console.Write("Kérlek érvényes pozitív számot adj meg a férőhelyhez: ");
                            }

                            components.roomManagger.createRoom(roomName, space);

                            Console.WriteLine("Nyomj meg egy gombot a folytatáshoz...");
                            Console.ReadKey();
                            Console.Clear();
                            break;

                        case 2:
                            Console.Clear();
                            components.roomManagger.showRooms();
                            Console.WriteLine("Nyomj meg egy gombot a visszatéréshez a főmenübe...");
                            Console.ReadKey();
                            Console.Clear();
                            break;

                        case 3:
                            Console.Clear();
                            Console.Write("Melyik szobába szeretnél lakót tenni? (Szoba neve): ");
                            string targetRoom = Console.ReadLine();
                            Console.Write("Lakó neve: ");
                            string residentName = Console.ReadLine();

                            components.roomManagger.addResidentToRoom(targetRoom, residentName);

                            Console.WriteLine("Nyomj meg egy gombot a folytatáshoz...");
                            Console.ReadKey();
                            Console.Clear();
                            break;

                        case 66:
                            Console.Clear();
                            break;

                        case 67:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Nincs ilyen menüpont!\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Érvénytelen bemenet, számot adj meg!\n");
                }
            }
        }
    }
}