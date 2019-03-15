using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app
{
    public interface IControllerSettings
    {
        DB_Config DbConfig { get; set; }
    }

    public class ControllerSettings : IControllerSettings
    {
        public DB_Config DbConfig { get; set; }
    }
}
