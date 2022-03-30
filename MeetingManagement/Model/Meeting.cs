using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManagement.Model
{
    public struct Meeting
    {
        /// <summary>
        /// Время начало события
        /// </summary>
        public DateTime startTime { get; set; }
        
        /// <summary>
        /// Примерное время окончания события
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        /// Время уведомления о событии
        /// </summary>
        public DateTime notificationTime { get; set; }

        /// <summary>
        /// Описание события
        /// </summary>
        public string Information { get; set; }
    }
}
