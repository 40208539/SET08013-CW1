using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SET08013_CW1
{
    class Message
    {
        private Level        _level;
        private string       _body;
        private List<string> _subjects;
        private List<string> _universities;

        Message(Level level, string body, List<string> subjects, List<string> universities)
        {
            _level = level;
            _body = body;
            _subjects = subjects;
            _universities = universities;
        }

        void serialize()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(this);

            Console.WriteLine(json);
        }
    }
}
