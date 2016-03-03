using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET08013_CW1
{
    class Message
    {
        enum Level {UG, PG};

        Level    level;
        string   body;
        string[] subjects;
        string[] universities;
    }
}
