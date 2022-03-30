using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManagement.Model
{
    public class Events
    {
        public delegate void ContainerMeeting(int ID, Meeting objectMeeting);

        public delegate void ContinerDeletingMeeting(int ID);

        public delegate void MeetingExport(DateTime dateTime);

        public event ContainerMeeting AddMeeting;

        public void OnAddMeeting(int ID, Meeting objectMeeting)
        {
            AddMeeting(ID, objectMeeting);
        }

        public event ContainerMeeting ChangesMeeting;

        public void OnChangeMeeting(int ID, Meeting objectMeeting)
        {
            ChangesMeeting(ID, objectMeeting);
        }

        public event ContinerDeletingMeeting DeleteMeeting;

        public void OnDeleteMeeting(int ID)
        {
            DeleteMeeting(ID);
        }

        public event MeetingExport ExportMeeting;

        public void OnExportMeeting(DateTime dateTime)
        {
            ExportMeeting(dateTime);
        }
    }
}
