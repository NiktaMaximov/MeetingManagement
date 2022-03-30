using MeetingManagement.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeetingManagement
{
    public class Managment
    {
        /// <summary>
        /// Список всех встреч
        /// </summary>
        public Dictionary<int, Meeting> listMeeting = new Dictionary<int, Meeting>();

        /// <summary>
        /// Список напоминалок
        /// </summary>
        Dictionary<int, Timer> listReminders = new Dictionary<int, Timer>();

        /// <summary>
        /// Добавление встречи
        /// </summary>
        public void AddMeeting(int ID, Meeting objectMeeting)
        {
            listMeeting[ID] = objectMeeting;
            Reminder(ID);
            Console.WriteLine("\nВстреча успешно добавлена\n");
        }

        /// <summary>
        /// Изменение встречи
        /// </summary>
        public void ChangesMeeting(int ID, Meeting objectMeeting)
        {
            if (listMeeting.ContainsKey(ID))
            {
                listMeeting[ID] = objectMeeting;
                Reminder(ID);
                Console.WriteLine("\nВстреча успешно изменена\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nОШИБКА: Похоже, что вы ошиблись при выборе встречи, такой встречи нет\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Удаление встречи
        /// </summary>
        public void DeleteMeeting(int ID)
        {
            if (listMeeting.ContainsKey(ID))
            {
                listMeeting.Remove(ID);
                Console.WriteLine("\nВстреча успешно удалена\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nОШИБКА: Похоже, что вы ошиблись при выборе встречи, такой встречи нет\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Экспорт встреч в текстовый файл
        /// </summary>
        /// <param name="dateTime"></param>
        public void ExportMeeting(DateTime dateTime)
        {
            var listMeetingExport = listMeeting.Values.Where(i => i.startTime.Date == dateTime).ToList();

            using (TextWriter tw = new StreamWriter("C:\\SavedList.txt"))
            {
                tw.WriteLine("Время начала  Информация о встрече  Время напоминания  Время окончания");
                foreach (var s in listMeetingExport)
                    tw.WriteLine($"{s.startTime} {s.Information} {s.notificationTime} {s.endTime}");
            }
            Console.WriteLine("\nДанные успешно экспортированы в файл C:\\SavedList.txt\n");
        }

        /// <summary>
        /// Напоминание о встрече
        /// </summary>
        public void Reminder(int ID)
        {           
            var time = listMeeting[ID].notificationTime;
            var nowTime = new DateTime(2022, 03, 28, 15,00,00);
            var timeDifference = ((int)(time - nowTime).TotalMilliseconds);

            Timer timer = new Timer(Message, null, timeDifference, 0);

            listReminders[ID] = timer;
        }

        public void Message(object o)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Скоро начнется встреча");
            Console.ResetColor();
        }
    }
}
