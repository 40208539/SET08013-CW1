using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET08013_CW1
{
    class Message
    {
        private enum Level {UG, PG};

        private Level    _level;
        private string   _body;
        private string[] _subjects;
        private string[] _universities;
    }
}
