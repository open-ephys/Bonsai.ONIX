using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    [Description("Sends data to the twelve 16-bit analog outputs on the Open Ephys FMC Host. Used in concert with FMCAnalogInputDevice.")]
    public class FMCAnalogOutputDevice : ONIFrameWriterDeviceBuilder<Mat>
    {
        const int Rows = 12;
        const int Cols = 1;
        readonly Size MAT_SIZE = new Size(Cols, Rows);

        // Control registers (see oedevices.h)
        public enum Register
        {
            NULLPARM = 0,
            CHDIR = 1,
            CH00INRANGE = 2,
            CH01INRANGE = 3,
            CH02INRANGE = 4,
            CH03INRANGE = 5,
            CH04INRANGE = 6,
            CH05INRANGE = 7,
            CH06INRANGE = 8,
            CH07INRANGE = 9,
            CH08INRANGE = 10,
            CH09INRANGE = 11,
            CH10INRANGE = 12,
            CH11INRANGE = 13,
        }

        uint io_reg;

        public enum InputOutput
        {
            Input = 0,
            Output = 1
        }

        void SetIO(int channel, InputOutput? io)
        {
            if (Controller != null && Controller.SelectedController != null)
            {
                io_reg = (io_reg & ~((uint)1 << channel)) | ((uint)(io) << channel);
                Controller.SelectedController.WriteRegister(DeviceIndex.SelectedIndex,
                                         (uint)Register.CHDIR,
                                         io_reg);
            }
        }

        InputOutput? GetIO(int channel)
        {
            uint? reg = (uint?)Controller?.SelectedController?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CHDIR);
            if (reg == null)
            {
                return null;
            } else
            {
                io_reg = (uint)reg;
                return (InputOutput)((io_reg >> channel) &(uint)1);
            }
        }


        public FMCAnalogOutputDevice() : base(oni.Device.DeviceID.FMCANALOG1R3)
        {
            io_reg = 0;
        }

        public override IObservable<Mat> Process(IObservable<Mat> source)
        {
            return source.Do(x => {

                // Check dims
                if (x.Size != MAT_SIZE) 
                {
                    throw new IndexOutOfRangeException("Source must be a 12x1 matrix");
                }

                if (x.Depth != Depth.U16)
                {
                    throw new InvalidOperationException("Source elements must be unsigned 16 bit integers");
                }

                Controller.SelectedController.AcqContext.Write((uint)DeviceIndex.SelectedIndex, x.Data, 2 * Rows);
            });
        }

        [Category("Acquisition")]
        [Description("The direction of channel 0.")]
        public InputOutput? Direction00
        {
            get
            {
                return GetIO(0);
            }
            set
            {
                SetIO(0, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 1.")]
        public InputOutput? Direction01
        {
            get
            {
                return GetIO(1);
            }
            set
            {
                SetIO(1, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 2.")]
        public InputOutput? Direction02
        {
            get
            {
                return GetIO(2);
            }
            set
            {
                SetIO(2, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 1.")]
        public InputOutput? Direction03
        {
            get
            {
                return GetIO(3);
            }
            set
            {
                SetIO(3, value);
            }
        }


        [Category("Acquisition")]
        [Description("The direction of channel 1.")]
        public InputOutput? Direction04
        {
            get
            {
                return GetIO(4);
            }
            set
            {
                SetIO(4, value);
            }
        }


        [Category("Acquisition")]
        [Description("The direction of channel 5.")]
        public InputOutput? Direction05
        {
            get
            {
                return GetIO(5);
            }
            set
            {
                SetIO(5, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 6.")]
        public InputOutput? Direction06
        {
            get
            {
                return GetIO(6);
            }
            set
            {
                SetIO(6, value);
            }
        }
        [Category("Acquisition")]
        [Description("The direction of channel 7.")]
        public InputOutput? Direction07
        {
            get
            {
                return GetIO(7);
            }
            set
            {
                SetIO(7, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 8.")]
        public InputOutput? Direction08
        {
            get
            {
                return GetIO(8);
            }
            set
            {
                SetIO(8, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 9.")]
        public InputOutput? Direction09
        {
            get
            {
                return GetIO(9);
            }
            set
            {
                SetIO(9, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 10.")]
        public InputOutput? Direction10
        {
            get
            {
                return GetIO(10);
            }
            set
            {
                SetIO(10, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 11.")]
        public InputOutput? Direction11
        {
            get
            {
                return GetIO(11);
            }
            set
            {
                SetIO(11, value);
            }
        }
    }
}
