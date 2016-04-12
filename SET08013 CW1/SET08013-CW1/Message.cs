using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SET08013_CW1
{
    [DataContract]
    class Message
    {
        [DataMember]
        private string       _level;
        [DataMember]
        private string       _body;
        [DataMember]
        private List<string> _subjects;
        [DataMember]
        private List<string> _universities;

        public Message(String level, string body, List<string> subjects, List<string> universities)
        {
            _level = level;
            _body = body;
            _subjects = subjects;
            _universities = universities;
        }

        public String level
        {
            get { return _level; }
            set { _level = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public List<string> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }

        public List<string> Universities
        {
            get { return _universities; }
            set { _universities = value; }
        }
    }
}
