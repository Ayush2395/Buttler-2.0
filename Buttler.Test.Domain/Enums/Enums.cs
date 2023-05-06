using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Domain.Enums
{
    public class Enums
    {
        public enum Orderstatus
        {
            pending, processing, waiter, served
        }
        public enum UserRole
        {
            staff, admin
        }
    }
}
