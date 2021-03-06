﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace HydroNumerics.Core.Time
{
  [DataContract]
  public class LinearConverter:TimeSeriesConverter
  {

    public LinearConverter()
    {
      TypeOfConverter = ConverterTypes.LinearConverter;
      Par1 = 0;
      Par2 = 1;

    }

    protected override void Initialize()
    {
      ConvertFunction = new Func<double, double>(x => Par1.Value + x * Par2.Value);
      ConvertBackFunction = new Func<double, double>(x => (x - Par1.Value) / Par2.Value);
    }

  }
}
