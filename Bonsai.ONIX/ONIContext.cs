using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [DefaultProperty("ContextConfiguration")]
    [Description("ONI-compliant acquisition context.")]
    public sealed class ONIContext : Source<ONIContextTask>, INamedElement
    {
        [Description("The ONI context configuration used by this node.")]
        [Editor("Bonsai.ONIX.Design.ONIContextConfigurationEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Externalizable(false)]
        public ONIContextConfiguration ContextConfiguration { get; set; } = new ONIContextConfiguration();

        public string Name => "ONI Context " + ContextConfiguration.Slot.ToString();

        public ONIContext()
        {
            ContextConfiguration.Slot.Driver = ONIContextTask.DefaultDriver;
            ContextConfiguration.Slot.Index = ONIContextTask.DefaultIndex;
        }

        public override IObservable<ONIContextTask> Generate()
        {
            return Observable.Using(
               () =>
               {
                   var c = ONIContextManager.ReserveContext(ContextConfiguration.Slot, true);
                   c.Context.BlockReadSize = ContextConfiguration.ReadSize;
                   c.Context.BlockWriteSize = ContextConfiguration.WriteSize;
                   c.Context.Start();
                   return c;
               },
               c =>
               {
                   return Observable
                       .Return(c.Context)
                       .Concat(Observable.Never(c.Context));
               });
        }
    }
}
