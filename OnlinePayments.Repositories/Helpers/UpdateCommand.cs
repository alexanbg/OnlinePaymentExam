using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Repositories.Helpers
{
    public class UpdateCommand
    {
        public static UpdateCommand Empty => new UpdateCommand();

        public Dictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();
        public UpdateCommand AddCondition(string field, object value)
        {
            Fields[field] = value;
            return this;
        }
    }
}
