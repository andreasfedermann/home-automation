using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_worker.Sensors
{
    /// <summary>
    /// This interface describes functions available
    /// for any kind of sensor
    /// </summary>
    public interface ISensorOperation<T>
    {
        T GetMeasuredValues();
    }
}
