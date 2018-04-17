using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Force.Meta.Validation
{
    public class EsExpressionVisitor: ExpressionVisitor
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public override string ToString() => _stringBuilder.ToString();
        
        protected override Expression VisitBinary(BinaryExpression node)
        {            
            _stringBuilder.Append("(");
 
            Visit(node.Left);
             
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    _stringBuilder.Append(" + ");
                    break;
 
                case ExpressionType.Divide:
                    _stringBuilder.Append(" / ");
                    break;
                    
                case ExpressionType.Subtract:
                    _stringBuilder.Append(" - ");
                    break;
                
                case ExpressionType.Multiply:
                    _stringBuilder.Append(" * ");
                    break;
                    
                case ExpressionType.GreaterThan:
                    _stringBuilder.Append(" > ");
                    break;    
                
                case ExpressionType.GreaterThanOrEqual:
                    _stringBuilder.Append(" >= ");
                    break;    
            
                case ExpressionType.LessThan:
                    _stringBuilder.Append(" < ");
                    break;    
                
                case ExpressionType.LessThanOrEqual:
                    _stringBuilder.Append(" <= ");
                    break;    
                    
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _stringBuilder.Append(" && ");
                    break;       
                    
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _stringBuilder.Append(" || ");
                    break;                     
            }
 
            Visit(node.Right);
            _stringBuilder.Append(")");
 
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _stringBuilder.Append(node.Value);
            return node;
        }

        public override Expression Visit(Expression node)
        {            
            return base.Visit(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _stringBuilder.Append(node.Name);
            return node;
        }
            
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Regex) && node.Method.Name == "Match")
            {
                var value = ((node.Object as MemberExpression)?.Expression as ConstantExpression)?.Value;
                if (value == null)
                {
                    throw new NotSupportedException();
                }

                var regex = value
                    .GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public).First()
                    .GetValue(value);
                 
                _stringBuilder.Append($"!!/{regex}/.exec(x)");
                
                return node;
            }
            
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!(node.Expression is ConstantExpression cnst)) return base.VisitMember(node);
            if (!(node.Member is FieldInfo field)) return base.VisitMember(node);
           
            var str = field.GetValue(cnst.Value);
            _stringBuilder.Append(str);
            return node;
        }
    }
}