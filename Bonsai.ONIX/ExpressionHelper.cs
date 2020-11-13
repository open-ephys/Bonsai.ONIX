using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bonsai.ONIX
{
    internal class ControllerGetter : ExpressionVisitor
    {
        public ONIController Controller { get; private set; }

        protected override Expression VisitExtension(Expression node)
        {
            var hidden_controller = node as HiddenONIControllerExpression;
            if (hidden_controller != null)
            {
                Controller = hidden_controller.Tag;
            }

            return base.VisitExtension(node);
        }
    }

    // TODO: This is finding controllers redundantly. Not sure why. But I'm just 
    // taking the hacky approach now by checking if Controllers already has 
    // a ref inside before adding.
    class ControllerFinder : ExpressionVisitor
    {
        public List<ONIController> Controllers { get; private set; }

        public ControllerFinder()
        {
            Controllers = new List<ONIController>();
        }

        protected override Expression VisitExtension(Expression node)
        {
            var hidden_controller = node as HiddenONIControllerExpression;
            if (hidden_controller != null)
            {
                if (!Controllers.Contains(hidden_controller.Tag))
                {
                    Controllers.Add(hidden_controller.Tag);
                    //Console.WriteLine("Found a Controller!");
                }
            }
            // We need to rely on reflection from here, because MultiCastBranchExpression is declared internal.
            if (node.GetType().FullName == "Bonsai.Expressions.MulticastBranchExpression")
            {
                // Get hidden source
                var source = (System.Linq.Expressions.MethodCallExpression)node.GetType().GetProperty("Source").GetValue(node);

                foreach (Expression a in source.Arguments)
                {
                    Visit(a);
                }

                Visit(node.Reduce()); // Internal parameter for the sake of completeness
                //Console.WriteLine("Found a MC!");
            }

            return base.VisitExtension(node);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            Visit(node.Body);
            Visit(node.Fault);
            Visit(node.Finally);
            return base.VisitTry(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            //Visit(node.Object);
            foreach (Expression a in node.Arguments)
            {
                Visit(a);
            }

            return base.VisitMethodCall(node);
        }
    }
}
