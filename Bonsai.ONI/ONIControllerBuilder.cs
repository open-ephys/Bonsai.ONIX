using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    // Used to hide hidden state (e.g. the oni.Context) into an expression tree that can be found by other nodes.
    class HiddenStateExpression : Expression
    {
        readonly Expression proxy;

        public HiddenStateExpression(Expression expression)
        {
            proxy = expression;
        }

        public ONIController Tag { get; set; }

        public override bool CanReduce => true;

        public override ExpressionType NodeType => ExpressionType.Extension;

        public override Type Type => proxy.Type;

        public override Expression Reduce()
        {
            // this is called only in ACTUAL compile time (just before run),
            // It defines the true transform of the node.
            return proxy;
        }
    }

    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    [DefaultProperty("Controller")]
    public class ONIControllerBuilder : ZeroArgumentExpressionBuilder
    {
        [Description("The hardware controller associated with this node.")]
        [Editor("Bonsai.ONI.Design.ONIControllerEditor, Bonsai.ONI.Design", typeof(UITypeEditor))]
        [Externalizable(false)]
        public ONIController Controller { get; set; }

        public ONIControllerBuilder()
        {
            Controller = new ONIController();
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            if (Controller.AcqContext == null) // If user has not explcitly connected already
                Controller.Refresh(); // This will throw if no connection can be made

            var sourceConstructor = Expression.Call(
                typeof(ONIControllerBuilder),
                nameof(Generate),
                null,
                new Expression[] { Expression.Constant(Controller) });

            return new HiddenStateExpression(sourceConstructor) { Tag = Controller };
        }

        static IObservable<oni.Frame> Generate(ONIController controller)
        {
            return Observable.Create<oni.Frame>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    try
                    {
                        //controller.AcqContext.Reset(); // Riffa requires reset before start()
                        controller.AcqContext.SetBlockReadSize(controller.BlockReadSize);
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

        //static IObservable<oni.Frame> Generate(ONIController controller)
        //{
        //    controller.AcqContext.Reset();
        //    controller.AcqContext.SetBlockReadSize(controller.BlockReadSize);
        //    controller.AcqContext.Start();

        //    return Observable
        //        .Generate<ONIController, oni.Frame>(
        //        controller,
        //        c => true, // This should react to the stop button somehow
        //        c => c,
        //        c => controller.AcqContext.ReadFrame())
        //        .Finally(() => 
        //            controller.AcqContext.Stop()
        //        );
        //}

        //static IObservable<oni.Frame> Generate(ONIController controller)
        //{
        //    controller.AcqContext.Reset();
        //    controller.AcqContext.SetBlockReadSize(controller.BlockReadSize);
        //    controller.AcqContext.Start();

        //    return Observable.Create<oni.Frame>(o =>
        //    {
        //        o.OnNext(controller.AcqContext.ReadFrame());
        //        //o.OnCompleted(controller.AcqContext.Stop());
        //        return Disposable.Empty;
        //    });
        //}
    }
}
