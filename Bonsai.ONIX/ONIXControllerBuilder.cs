using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    // Used to hide hidden state (e.g. the oni.Context) into an expression tree that can be found by other nodes.
    class HiddenONIControllerExpression : Expression
    {
        readonly Expression proxy;

        public HiddenONIControllerExpression(Expression expression)
        {
            proxy = expression;
        }

        public ONIController Tag { get; set; }

        public override bool CanReduce => true;

        public override ExpressionType NodeType => ExpressionType.Extension;

        public override Type Type => proxy.Type;

        public override Expression Reduce()
        {
            // This is called only in ACTUAL compile time (just before run),
            // It defines the true transform of the node.
            return proxy;
        }
    }

    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    [DefaultProperty("Controller")]
    [Description("ONI-compliant hardware link.")]
    public class ONIXControllerBuilder : ZeroArgumentExpressionBuilder, IDisposable
    {
        bool disposed;

        [Description("The hardware link used by this node.")]
        [Editor("Bonsai.ONIX.Design.ONIControllerEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Externalizable(false)]
        public ONIController Controller { get; set; }

        public ONIXControllerBuilder()
        {
            Controller = new ONIController();
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            if (Controller.AcqContext == null) // If user has not explicitly connected already
                Controller.Refresh(); // This will throw if no connection can be made

            var sourceConstructor = Expression.Call(
                typeof(ONIXControllerBuilder),
                nameof(Generate),
                null,
                new Expression[] { Expression.Constant(Controller) });

            return new HiddenONIControllerExpression(sourceConstructor) { Tag = Controller };
        }

        static IObservable<oni.Frame> Generate(ONIController controller)
        {
            return Observable.Create<oni.Frame>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    try
                    {
                        controller.AcqContext.BlockReadSize = controller.BlockReadSize;
                        controller.AcqContext.BlockWriteSize = controller.WritePreAllocSize;
                        controller.AcqContext.Start();

                        while (!cancellationToken.IsCancellationRequested)
                        {
                            observer.OnNext(controller.AcqContext.ReadFrame());
                        }
                    }
                    finally
                    {
                        controller.AcqContext.Stop();
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            })
            .PublishReconnectable()
            .RefCount();
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ONIXControllerBuilder()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Controller.AcqContext.Dispose();
                    disposed = true;
                }
            }
        }

        void IDisposable.Dispose()
        {
            Close();
        }
    }
}
