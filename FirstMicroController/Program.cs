using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHI.OSHW.Hardware;

namespace FirstMicroController
{
    public class Program
    {
        private static GrillController grill;
        private static readonly OutputPort _onboardLed = new OutputPort((Cpu.Pin)FEZCerbuino.Pin.Digital.LED1, true);
        private static readonly double _voltage = 4.78;

        public static void Main()
        {
            var probes = new TempProbe[1];
            probes[0] = new TempProbe("Food 1", FEZCerbuino.Pin.AnalogIn.A0);

            grill = new GrillController(probes);
            var timer = new Timer(LedBlink, null, 0, 1000);
            while (true)
            {
                if (grill.DoWork())
                    NewTempAvailable();
            }
        }

        static void LedBlink(object o)
        {
            _onboardLed.Write(!_onboardLed.Read());
        }

        private static void NewTempAvailable()
        {
            UpdateDisplay();
        }

        private static void UpdateDisplay()
        {
            for (int i = 0; i < grill.Probes.Length; i++)
            {
                Debug.Print(grill.Probes[i].Name+": "+grill.Probes[i].TemperatureF.ToString("N0"));
            }
        }

        //private static int ReadTemp()
        //{
        //    var totalTemp = 0.0;
        //    var averageTemp = 0.0;
        //    for (int i = 0; i < 16; i++)
        //    {
        //        var adc = _temp1.ReadRaw();
        //        if (adc == 0 || adc >= 1023)
        //        {
        //            addAdcValue(0);
        //            return;
        //        }
        //    }
        //}

        //private static int count = 0;
        //private static double totals = 0.0;
        //private static double GetAverageTemperatureReading()
        //{
        //    var totalTemp = 0.0;
        //    var averageTemp = 0.0;

        //    for (int i = 0; i < 16; i++)
        //    {
                
        //        totalTemp += GetCurrentReading();
        //        Thread.Sleep(10);
        //    }
        //    averageTemp = totalTemp / 100;
        //    var resistance = System.Math.Log((1 / ((1024 / (double)averageTemp) - 1)) * (double)100000);
        //    var celcius = (1 / ((2.3067434E-4) + (2.3696596E-4) * resistance + (1.2636414E-7) * resistance * resistance * resistance)) - 273.25;
        //    var fahrenheit = ((int)((celcius * 9.0) / 5.0 + 32.0));

        //    Debug.Print((count+1)+" Current:"+GetCurrentReading()+" Avg:"+averageTemp+" Resistance:"+resistance+" Celcius:"+celcius+" Fahrenheit:"+fahrenheit);
        //    count++;
        //    totals += averageTemp;
        //    if (count==10)
        //    {
        //        Debug.Print("10 Avg:"+(totals/10));
        //        count = 0;
        //        totals = 0.0;
        //    }
        //    return fahrenheit;
        //}
    }
}
