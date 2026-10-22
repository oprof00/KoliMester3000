using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace KoliMester3000.components
{
    public class Room
    {
        public string roomNameC { get; set; }
        public int roomSpaceC { get; set; }
        public bool roomFullC { get; set; }
        public bool cleanNeddedC { get; set; }
        public List<string> residents { get; set; } = new List<string>();
    }

    internal class roomManagger
    {
        private static readonly string directoryPath = @"C:\kolimester";
        private static readonly string filePath = Path.Combine(directoryPath, "rooms.json");

        private static List<Room> loadRooms()
        {
            if (!File.Exists(filePath))
            {
                return new List<Room>();
            }

            try
            {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Room>();
                }
                return JsonSerializer.Deserialize<List<Room>>(json) ?? new List<Room>();
            }
            catch
            {
                return new List<Room>();
            }
        }

        private static void saveRooms(List<Room> rooms)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(rooms, options);
            File.WriteAllText(filePath, json);
        }

        public static void createRoom(string roomName, int space)
        {
            Console.WriteLine("letrehozas");
            Console.WriteLine($"nev: {roomName}");
            Console.WriteLine($"hely: {space}");

            var newRoom = new Room
            {
                roomNameC = roomName,
                roomSpaceC = space,
                roomFullC = false,
                cleanNeddedC = false,
                residents = new List<string>()
            };

            List<Room> roomsList = loadRooms();
            roomsList.Add(newRoom);
            saveRooms(roomsList);

            Console.WriteLine("Szoba sikeresen hozzáadva és mentve!\n");
        }

        public static void showRooms()
        {
            Console.WriteLine("- MENTETT SZOBÁK LISTÁJA -");

            List<Room> roomsList = loadRooms();

            if (roomsList.Count == 0)
            {
                Console.WriteLine("Még nincsenek mentett szobák.\n");
                return;
            }

            int index = 1;
            foreach (var room in roomsList)
            {
                string lakok;
                if (room.residents != null && room.residents.Count > 0)
                {
                    lakok = string.Join(", ", room.residents);
                }
                else
                {
                    lakok = "Nincs lakó";
                }

                int lakokSzama = 0;
                if (room.residents != null)
                {
                    lakokSzama = room.residents.Count;
                }

                string teleVanSzoveg;
                if (room.roomFullC)
                {
                    teleVanSzoveg = "Igen";
                }
                else
                {
                    teleVanSzoveg = "Nem";
                }

                string takaritasSzoveg;
                if (room.cleanNeddedC)
                {
                    takaritasSzoveg = "Igen";
                }
                else
                {
                    takaritasSzoveg = "Nem";
                }

                Console.WriteLine($"{index}. Szoba neve: {room.roomNameC}");
                Console.WriteLine($"Férőhely: {room.roomSpaceC} (Jelenlegi lakók száma: {lakokSzama})");
                Console.WriteLine($"Lakók: {lakok}");
                Console.WriteLine($"Tele van?: {teleVanSzoveg}");
                Console.WriteLine($"Takarítás szükséges?: {takaritasSzoveg}");
                Console.WriteLine(new string('-', 30));

                index++;
            }
            Console.WriteLine();
        }

        public static void addResidentToRoom(string targetRoomName, string studentName)
        {
            List<Room> roomsList = loadRooms();

            Room foundRoom = null;
            foreach (var r in roomsList)
            {
                if (r.roomNameC.Equals(targetRoomName, StringComparison.OrdinalIgnoreCase))
                {
                    foundRoom = r;
                    break;
                }
            }

            if (foundRoom == null)
            {
                Console.WriteLine($"Nincs '{targetRoomName}' nevű szoba a rendszerben.\n");
                return;
            }

            if (foundRoom.residents == null)
            {
                foundRoom.residents = new List<string>();
            }

            if (foundRoom.residents.Count >= foundRoom.roomSpaceC)
            {
                Console.WriteLine($"A(z) {foundRoom.roomNameC} szoba már megtelt! Nem adható hozzá új lakó.\n");
                return;
            }

            foundRoom.residents.Add(studentName);
            if (foundRoom.residents.Count >= foundRoom.roomSpaceC)
            {
                foundRoom.roomFullC = true;
            }

            saveRooms(roomsList);
            Console.WriteLine($"'{studentName}' sikeresen hozzáadva a(z) {foundRoom.roomNameC} szobához!\n");
        }

        
        public static dynamic deleteRoomMembers(string targetRoomName, string deleteResident)
        {
            List<Room> roomsList = loadRooms();

            Room foundRoom = null;

            foreach (var room in roomsList)
            {
                if (room.roomNameC.Equals(
                    targetRoomName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    foundRoom = room;
                    break;
                }
            }

            if (foundRoom == null)
            {
                return "no-room";
            }
            
            if (!foundRoom.residents.Contains(deleteResident))
            {
                Console.WriteLine("Nem sikerült törölni");
                return "no-resident";
            }

            int removedCount = foundRoom.residents.RemoveAll(
                member => member == deleteResident
            );

            if (removedCount > 0)
            {
                Console.WriteLine($"Sikeres törlés: {deleteResident}");
                saveRooms(roomsList);
                return null;
            }

            Console.WriteLine("Nem sikerült törölni");
            return "no-resident";
        }

    }
}