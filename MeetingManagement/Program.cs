using MeetingManagement.Model;
using System;
using System.Linq;
using System.Threading;

namespace MeetingManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Events events = new Events();
            Managment managment = new Managment();

            events.AddMeeting += managment.AddMeeting;
            events.ChangesMeeting += managment.ChangesMeeting;
            events.DeleteMeeting += managment.DeleteMeeting;
            events.ExportMeeting += managment.ExportMeeting;

            int ID = default;
            int notificationTime = default;

            string information = default;

            DateTime startTime = default;
            DateTime endTime = default;

            int input = default;

            while (input != 5)
            {
                Console.WriteLine("\nДобро пожаловать в упраление встречами.\n" +
                    "Пожалуйста, выберите интересующий Вас пункт\n" +
                    "1 - Добавление новой встречи\n" +
                    "2 - Изменение существущей встречи\n" +
                    "3 - Удаление встречи\n" +
                    "4 - Экспорт данных\n" +
                    "5 - Выход из программы\n");

                input = int.TryParse(Console.ReadLine(), out var result) ? result : result;

                switch (input)
                {
                    case 1:
                        if (AddMeeting(ref ID, ref startTime, ref endTime, ref notificationTime, ref information))
                        {
                            events.OnAddMeeting(ID, new Meeting()
                            {
                                startTime = startTime,
                                endTime = endTime,
                                Information = information,
                                notificationTime = startTime.AddMinutes(-notificationTime)
                            });

                            break;
                        }
                        else
                            break;

                    case 2:
                        Console.WriteLine("Выберите, каую встречу Вы хотите изменить, указав её ID номер");
                        Console.WriteLine("ID | Время начала | Информация о встрече | Время напоминания | Время окончания");

                        foreach (var meeting in managment.listMeeting)
                            Console.WriteLine($"{meeting.Key} | {meeting.Value.startTime} | {meeting.Value.Information} | {meeting.Value.notificationTime} | {meeting.Value.endTime}");

                        var selectedKeyChange = int.TryParse(Console.ReadLine(), out var kc) ? kc : kc;

                        if (ChangeMeeting(ref startTime, ref endTime, ref notificationTime, ref information))
                        {
                            events.OnChangeMeeting(selectedKeyChange, new Meeting()
                            {
                                startTime = startTime,
                                endTime = endTime,
                                Information = information,
                                notificationTime = startTime.AddMinutes(-notificationTime)
                            });

                            break;
                        }
                        else
                            break;

                    case 3:
                        Console.WriteLine("Выберите, каую встречу Вы хотите удалить, указав её ID номер");
                        Console.WriteLine("ID | Время начала | Информация о встрече | Время напоминания | Время окончания");

                        foreach (var meeting in managment.listMeeting)
                            Console.WriteLine($"{meeting.Key} | {meeting.Value.startTime} | {meeting.Value.Information} | {meeting.Value.notificationTime} | {meeting.Value.endTime}");

                        var selectedKeyDelete = int.TryParse(Console.ReadLine(), out var kd) ? kd : kd;

                        events.OnDeleteMeeting(selectedKeyDelete);

                        break;

                    case 4:
                        Console.WriteLine("Выберите, за какую дату необходимо сохранить все встречи в текстовый файл");
                        Console.WriteLine("Время начала | Информация о встрече");

                        foreach (var meeting in managment.listMeeting)
                            Console.WriteLine($"{meeting.Value.startTime.ToShortDateString()} | {meeting.Value.Information}");

                        var selectedDay = DateTime.TryParse(Console.ReadLine(), out var sd) ? sd : sd;

                        events.OnExportMeeting(selectedDay);

                        break;

                    default:
                        Console.WriteLine("Ошибка, выбранного Вами пункта нет");
                        break;
                }
            }
        }

        public static bool AddMeeting(ref int ID, ref DateTime transmittedStartTime, ref DateTime transmittedEndTime, ref int transmittedNotificationTime, ref string transmittedInformation)
        {
            Console.WriteLine("Пожалуйста, укажите следующие данные");

            Console.WriteLine("Дату и время начала встречи:");
            var newStartTime = DateTime.TryParse(Console.ReadLine(), out var nsDate) ? nsDate : nsDate;

            if (transmittedStartTime != newStartTime)
                transmittedStartTime = newStartTime;
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка, встречу нельзя добавить так как на это время назначена другая встреча\n");
                Console.ResetColor();
                return false;
            }
                
            if (DateTime.Now > transmittedStartTime)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка, встречу нельзя добавить в прошедшом времени, попробуйте еще раз\n");
                Console.ResetColor();
                return false;
            }

            Console.WriteLine("Дату и время окончания встречи: (По умолчанию +1 час от начала встречи)");
            transmittedEndTime = DateTime.TryParse(Console.ReadLine(), out var eDate) ? eDate : transmittedStartTime.AddHours(1);

            if (transmittedStartTime > transmittedEndTime)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка, дата окончания не может быть раньше даты начала, попробуйте еще раз\n");
                Console.ResetColor();
                return false;
            }

            Console.WriteLine("Время в минутах, за которое необходимо уведомить о начале встречи");
            transmittedNotificationTime = int.TryParse(Console.ReadLine(), out var nDate) ? nDate : nDate;

            Console.WriteLine("Описание встречи");
            transmittedInformation = Console.ReadLine();

            ID++;

            return true;
        }

        public static bool ChangeMeeting(ref DateTime transmittedStartTime, ref DateTime transmittedEndTime, ref int transmittedNotificationTime, ref string transmittedInformation)
        {
            Console.WriteLine("Пожалуйста, укажите следующие данные");
            Console.WriteLine("Если какой-либо пункт будет пропущен, то значение этого пункта будет возращено к изначальному\n");

            Console.WriteLine("Дату и время начала встречи:");
            transmittedStartTime = DateTime.TryParse(Console.ReadLine(), out var nsDate) ? nsDate : transmittedStartTime;

            Console.WriteLine("Дату и время окончания встречи: (По умолчанию +1 час от начала встречи)");
            transmittedEndTime = DateTime.TryParse(Console.ReadLine(), out var eDate) ? eDate : transmittedStartTime.AddHours(1);

            Console.WriteLine("Время в минутах, за которое необходимо уведомить о начале встречи");
            transmittedNotificationTime = int.TryParse(Console.ReadLine(), out var nDate) ? nDate : transmittedNotificationTime;

            Console.WriteLine("Описание встречи");
            var newInformation = Console.ReadLine();
            transmittedInformation = newInformation == string.Empty ? transmittedInformation : newInformation;

            return true;
        }
    }
}
