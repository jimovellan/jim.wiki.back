using jim.wiki.core.Extensions;
using jim.wiki.core.Repository.Models.Search;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace jim.wiki.core.Repository.Extensions;

public static class IQueryableExtensions
{
    /// <summary>
    /// Ordena usando el nombre del campo y si es ascendente o descendente
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="field"></param>
    /// <param name="ascencing"></param>
    /// <returns></returns>
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string field, bool ascencing)
    {

        var entityType = typeof(T);

        var lambda = GenerateExpresionWithProperty(entityType, field);

        var property = entityType.GetProperties().FirstOrDefault(f => f.Name.Trim().ToLowerInvariant() == field.Trim().ToLowerInvariant());       
                
        var orderMethodName = ascencing  ?   "OrderBy" : "OrderByDescending";

        
        var typeQuery = typeof(System.Linq.Queryable);

        

        var method = typeQuery.GetMethods()
                         .Where(type => type.Name == orderMethodName
                                 && type.IsGenericMethod && type.IsPublic
                                 && type.GetParameters().Count() == 2
                                    ).FirstOrDefault();

       var genericMethod = method.MakeGenericMethod(entityType, property.PropertyType);


       return (IOrderedQueryable<T>)genericMethod.Invoke(null, new object[] { query, lambda });
    }

    /// <summary>
    /// Da otro indice de ordenación una vez ordenado anteriormente por otro campo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="field"></param>
    /// <param name="ascencing"></param>
    /// <returns></returns>
    public static IOrderedQueryable<T> ThenOrderBy<T>(this IOrderedQueryable<T> query, string field, bool ascencing = false) 
    {

        var type = typeof(T);

        var lambda = GenerateExpresionWithProperty(type, field);

        var property = type.GetProperties().FirstOrDefault(f => f.Name.Trim().ToLowerInvariant() == field.Trim().ToLowerInvariant());

        var orderMethodName = ascencing ? "ThenBy" : "ThenByDescending";


        var typeQuery = typeof(System.Linq.Queryable);
        

        var method = typeQuery.GetMethods()
                         .Where(type => type.Name == orderMethodName
                                 && type.IsGenericMethod && type.IsPublic
                                 && type.GetParameters().Count() == 2
                                    ).FirstOrDefault();

        var genericMethod = method.MakeGenericMethod(type, property.PropertyType);

        return (IOrderedQueryable<T>)genericMethod.Invoke(null, new object[] { query, lambda });
    }

    /// <summary>
    /// Genera una expresión del tipo x=>x.Property
    /// </summary>
    /// <param name="type"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    private static Expression  GenerateExpresionWithProperty(Type type, string field)
    {
        var property = type.GetProperties().FirstOrDefault(x=>x.Name.Trim().ToLowerInvariant() == field.Trim().ToLowerInvariant());

        var parameter = Expression.Parameter(type, "x");
        var prop = Expression.Property(parameter, property);
        return Expression.Lambda(prop, new ParameterExpression[] { parameter });
    }

    /// <summary>
    /// Crea el filtro Where en base a una lista de FieldSearch
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="fieldSearches"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, IEnumerable<FieldSearch> fieldSearches)
    {
        if (fieldSearches.NoContainElements()) return query;


        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

        Expression combinedExpression = null;
        
        foreach (var field in fieldSearches)
        {
            var property = typeof(T).GetProperties()
                                    .FirstOrDefault(f => f.Name.Trim().ToLowerInvariant() == field.Name.Trim().ToLowerInvariant());

            if(property == null) continue;

            var member = Expression.Property(parameter, property!);
            
            var expression = field.Operation switch
            {

                OperatorEnum.In => GenerateExpressionContainedInList(property.PropertyType,member,field.Values),
                OperatorEnum.Like => GenerateLikeExpression(member,property.PropertyType,field),
                _ => GenerateExpression(member,property.PropertyType, field)
                    
            };

            if(combinedExpression == null)
            {
                combinedExpression = expression!;
            }
            else
            {
                combinedExpression = field.LogicalOperation switch
                {
                    LogicalOperation.Or => Expression.Or(combinedExpression, expression!),
                    LogicalOperation.And => Expression.And(combinedExpression, expression!),
                    _ => throw new NotSupportedException("Logical operator not supported")
                };
            }
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

        return query.Where(lambda);
    }

    private static BinaryExpression GenerateExpression(MemberExpression member, Type propertyType, FieldSearch fieldSearch)
    {

        var parsedValue = ParseValue(fieldSearch?.Value?.ToString() ?? "", propertyType);
        var constant = Expression.Constant(parsedValue, propertyType);

        return fieldSearch.Operation switch
        {

            OperatorEnum.Equal => Expression.Equal(member, constant),
            OperatorEnum.NotEqual => Expression.NotEqual(member, constant),
            OperatorEnum.GreaterThan => Expression.GreaterThan(member, constant),
            OperatorEnum.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, constant),
            OperatorEnum.LessThan => Expression.LessThan(member, constant),
            OperatorEnum.LessThanOrEqual => Expression.LessThanOrEqual(member, constant),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Genera la expresion para un valor que este contenido en uno de los valroes
    /// </summary>
    /// <param name="PropertyType"></param>
    /// <param name="member"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    private static Expression GenerateExpressionContainedInList(Type PropertyType,MemberExpression member, IEnumerable<object?> values)
    {
        Expression Combinedexpression = null;
        foreach (var value in values)
        {
            var parsedValue = ParseValue(value?.ToString()??"", PropertyType);
            var constant = Expression.Constant(parsedValue);
            var expression = Expression.Equal(member, constant);

            if(Combinedexpression == null)
            {
                Combinedexpression= expression;
            }
            else
            {
                Combinedexpression =  Expression.OrElse(Combinedexpression, expression);
            }
        }

        return Combinedexpression;
    }

    /// <summary>
    /// Parseo de valores para uso en Constant para generar expressions
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static object ParseValue(string value, Type targetType)
    {
        object parsedValue = null;

        switch (Type.GetTypeCode(targetType))
        {
            case TypeCode.Boolean:
                if (bool.TryParse(value, out bool boolVal))
                {
                    parsedValue = boolVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to bool");
                }
                break;

            case TypeCode.Int32:
                if (int.TryParse(value, out int intVal))
                {
                    parsedValue = intVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to int");
                }
                break;

            case TypeCode.Int64:
                if (long.TryParse(value, out long longVal))
                {
                    parsedValue = longVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to long");
                }
                break;

            case TypeCode.Double:
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleVal))
                {
                    parsedValue = doubleVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to double");
                }
                break;

            case TypeCode.Decimal:
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalVal))
                {
                    parsedValue = decimalVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to decimal");
                }
                break;

            case TypeCode.DateTime:
                if (DateTime.TryParse(value, out DateTime dateTimeVal))
                {
                    parsedValue = dateTimeVal.ToUniversalTime();
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to DateTime");
                }
                break;

            case TypeCode.String:
                parsedValue = value;
                break;

            case TypeCode.Char:
                if (char.TryParse(value, out char charVal))
                {
                    parsedValue = charVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to char");
                }
                break;

            case TypeCode.Byte:
                if (byte.TryParse(value, out byte byteVal))
                {
                    parsedValue = byteVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to byte");
                }
                break;

            case TypeCode.SByte:
                if (sbyte.TryParse(value, out sbyte sbyteVal))
                {
                    parsedValue = sbyteVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to sbyte");
                }
                break;

            case TypeCode.UInt16:
                if (ushort.TryParse(value, out ushort ushortVal))
                {
                    parsedValue = ushortVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to ushort");
                }
                break;

            case TypeCode.UInt32:
                if (uint.TryParse(value, out uint uintVal))
                {
                    parsedValue = uintVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to uint");
                }
                break;

            case TypeCode.UInt64:
                if (ulong.TryParse(value, out ulong ulongVal))
                {
                    parsedValue = ulongVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to ulong");
                }
                break;

            case TypeCode.Single:
                if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float floatVal))
                {
                    parsedValue = floatVal;
                }
                else
                {
                    throw new Exception($"The value {value} cannot be cast to float");
                }
                break;

            default:
                if (targetType == typeof(Guid))
                {
                    if (Guid.TryParse(value, out Guid guidVal))
                    {
                        parsedValue = guidVal;
                    }
                    else
                    {
                        throw new Exception($"The value {value} cannot be cast to Guid");
                    }
                }
                else
                {
                    throw new Exception($"The type {targetType} is not supported");
                }
                break;
        }

        return parsedValue;
    }

    private static Expression GenerateLikeExpression(MemberExpression member, Type proppertyType, FieldSearch field)
    {
        if (proppertyType != typeof(String)) throw new ArgumentException($"the property {field.Name} cant be use Like expressions");

        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        var constant = Expression.Constant(field.Value?.ToString() ?? "", proppertyType);

        return Expression.Call(member, method, constant);

    }
}
