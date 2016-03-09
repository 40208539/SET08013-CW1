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
        private Level        _level;
        [DataMember]
        private string       _body;
        [DataMember]
        private List<string> _subjects;
        [DataMember]
        private List<string> _universities;

        public Message(Level level, string body, List<string> subjects, List<string> universities)
        {
            _level = level;
            _body = body;
            _subjects = subjects;
            _universities = universities;
        }
    }
}
