using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET08013_CW1
{
    class Messages
    {
        enum Level {UG, PG};

        struct Message
        {
            string   body;
            string[] subjects;
            string[] universities;
            Level    level;
        }
    }
}
