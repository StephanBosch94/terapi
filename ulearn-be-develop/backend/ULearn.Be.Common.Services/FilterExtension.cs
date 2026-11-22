using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Common.Models;

namespace ULearn.Be.Common.Services;

/// <summary>
///     General sort/filter extension
/// </summary>
public static class FilterExtension
{
    public static PagedVM<TSource> ToPagedVM<TSource, TFilter>(
        this IQueryable<TSource> source,
        FilterView<TFilter> filter,
        bool withoutCount = false
    ) where TSource : TFilter where TFilter : new()
    {
        var data = source
            .Sort(filter)
            .Filter(filter)
            .SkipTake(filter)
            .ToList();

        var count = withoutCount
            ? data.Count
            : source.Filter(filter).Count();

        return new PagedVM<TSource>(count, data);
    }

    public static PagedVM<T> ToPagedVM<T>(this ICollection<T> data, int? count = null)
    {
        return new PagedVM<T>(count ?? data.Count, data);
    }

    public static async Task<PagedVM<TSource>> ToPagedVMAsync<TSource, TFilter>(
        this IQueryable<TSource> source,
        FilterView<TFilter> filter,
        bool withoutCount = false
    ) where TFilter : new()
    {
        var data = await source
            .Sort(filter)
            .Filter(filter)
            .SkipTake(filter)
            .ToListAsync();

        var count = withoutCount
            ? data.Count
            : await source.Filter(filter).CountAsync();

        return new PagedVM<TSource>(count, data);
    }

    public static IQueryable<TSource> SkipTake<TSource, TFilter>(
        this IQueryable<TSource> source,
        FilterView<TFilter> filter
    )
    {
        if (filter.Skip.HasValue)
        {
            source = source.Skip(filter.Skip.Value);
        }

        if (filter.Take.HasValue)
        {
            source = source.Take(filter.Take.Value);
        }

        return source;
    }

    private static IQueryable<TSource> OrderByExpression<TSource>(
        this IQueryable<TSource> source,
        string propName,
        bool? reverse,
        bool first = true
    )
    {
        if (!HasProperty(typeof(TSource), propName))
        {
            return source;
        }

        string orderByName;
        if (first)
        {
            orderByName = reverse == true
                ? nameof(Queryable.OrderByDescending)
                : nameof(Queryable.OrderBy);
        }
        else
        {
            orderByName = reverse == true
                ? nameof(Queryable.ThenByDescending)
                : nameof(Queryable.ThenBy);
        }

        var (orderByExp, typeArgs) = GetRootDeclaringType(typeof(TSource), propName);
        var resultExp = Expression.Call(
            typeof(Queryable),
            orderByName,
            typeArgs,
            source.Expression,
            Expression.Quote(orderByExp)
        );

        return source.Provider.CreateQuery<TSource>(resultExp);
    }

    private static (LambdaExpression lambda, Type[] typeArgs) GetRootDeclaringType(Type root, string propertyPath)
    {
        var child = GetProperty(root, propertyPath);

        var parameter = Expression.Parameter(root, "p");
        var path = Expression.MakeMemberAccess(parameter, child);

        return (
            lambda: Expression.Lambda(path, parameter),
            typeArgs: new[] { root, child.PropertyType }
        );
    }

    private static IEnumerable<PropertyInfo> GetProperties(Type root, string propertyPath)
    {
        return root
            .GetProperties()
            .Where(o => string.Equals(o.Name, propertyPath, StringComparison.OrdinalIgnoreCase));
    }

    private static PropertyInfo GetProperty(Type root, string propertyName)
    {
        return GetProperties(root, propertyName)
            .FirstOrDefault();
    }

    private static bool HasProperty(Type root, string propertyName)
    {
        return GetProperty(root, propertyName) != null;
    }

    public static IQueryable<TSource> Sort<TSource, TFilter>(
        this IQueryable<TSource> source,
        FilterView<TFilter> filter
    ) where TFilter : new()
    {
        if (OrderingMethodFinder.OrderMethodExists(source.Expression))
        {
            return source;
        }

        var predicate = filter.Sort?.Predicate;
        var filterType = typeof(TFilter);

        if (typeof(ISortableEntity).IsAssignableFrom(filterType))
        {
            // this is a workaround, because you cannot have static methods
            var x = filter.Filter ?? new TFilter();
            predicate = ((ISortableEntity)x).GetSortProperty(predicate);
        }

        var sortColumns = new Dictionary<int, PropertyInfo>();
        foreach (var propertyInfo in filterType.GetProperties())
        {
            if (Attribute.IsDefined(propertyInfo, typeof(DefaultSortColumnAttribute)))
            {
                var attr = (DefaultSortColumnAttribute[])propertyInfo.GetCustomAttributes(
                    typeof(DefaultSortColumnAttribute),
                    false
                );
                var orderNumber = attr[0].OrderNumber;
                sortColumns[orderNumber] = propertyInfo;
            }

            /* Filter could have defined List of values for filtering also as for sorting */
            /* in that case use SourcePropertyName instead of Property / List name as predicate */
            if (predicate != null
                && string.Equals(propertyInfo.Name, predicate, StringComparison.OrdinalIgnoreCase)
                && Attribute.IsDefined(propertyInfo, typeof(SourcePropertyNameAttribute)))
            {
                var attr = (SourcePropertyNameAttribute[])propertyInfo.GetCustomAttributes(
                    typeof(SourcePropertyNameAttribute),
                    false
                );
                predicate = attr[0].Name;
            }
        }

        var firstOrderBy = true;
        if (IsOrderByPredicateValid<TSource>(predicate))
        {
            source = source.OrderByExpression(predicate, filter.Sort?.Reverse ?? false);
            firstOrderBy = false;
        }

        if (sortColumns.Count <= 0)
        {
            return source;
        }

        var keys = sortColumns.Keys.OrderBy(x => x).ToList();
        for (var i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            var propertyInfo = sortColumns[key];
            var attr = (DefaultSortColumnAttribute[])propertyInfo.GetCustomAttributes(
                typeof(DefaultSortColumnAttribute),
                false
            );
            firstOrderBy &= i == 0;
            source = source.OrderByExpression(propertyInfo.Name, attr[0].Reversed, firstOrderBy);
        }

        return source;
    }

    private static bool IsOrderByPredicateValid<TSource>(string predicate)
    {
        return !string.IsNullOrWhiteSpace(predicate) && HasProperty(typeof(TSource), predicate);
    }

    public static IQueryable<TSource> Filter<TSource, TFilter>(
        this IQueryable<TSource> source,
        FilterView<TFilter> filter
    )
    {
        if (filter.Filter == null)
        {
            return source;
        }

        var equalTypes = GetEqualTypes();
        var stringTypes = GetContainsTypes();

        foreach (var x in filter.Filter.GetType().GetProperties())
        {
            if (Attribute.IsDefined(x, typeof(IgnoreFromAutomaticFilterAttribute)))
            {
                continue;
            }

            if (Attribute.IsDefined(x, typeof(FilterAsStringContainsAttribute)))
            {
                source = NullableStringContains(x, source, filter.Filter, x.PropertyType);
            }
            else if (Attribute.IsDefined(x, typeof(FilterAsStringStartsWithAttribute)))
            {
                source = NullableStringStartsWith(x, source, filter.Filter, x.PropertyType);
            }
            else if (Attribute.IsDefined(x, typeof(FilterAsStringContainsIgnoreCaseAttribute)))
            {
                source = StringContainsIgnoreCase(x, source, filter.Filter);
            }
            else if (Attribute.IsDefined(x, typeof(ComparisonAttribute)))
            {
                source = CustomComparison(x, source, filter.Filter);
            }
            else if (Attribute.IsDefined(x, typeof(OrFilterByAttribute)))
            {
                source = StringContainsWithOr(x, source, filter.Filter);
            }
            else if (equalTypes.Contains(x.PropertyType) || Attribute.IsDefined(x, typeof(KeywordAttribute)))
            {
                source = NullableEqual(x, source, filter.Filter);
            }
            else if (stringTypes.Contains(x.PropertyType))
            {
                source = StringContains(x, source, filter.Filter);
            }
            else if (x.PropertyType.GetInterface(nameof(ICollection)) != null) //IEnumerable by bylo pro []
            {
                source = ListContains(x, source, filter.Filter);
            }
        }
        
        return source;
    }

    public static IQueryable<TSource> Distinct<TSource>(
        this IQueryable<TSource> source,
        bool distinct
    )
    {
        return distinct ? source.Distinct() : source;
    }

    private static IQueryable<TSource> CustomComparison<TSource, TFilter>(
        PropertyInfo propertyInfo,
        IQueryable<TSource> source,
        TFilter filter
    )
    {
        if (Attribute.IsDefined(propertyInfo, typeof(GreaterOrEqualAttribute)))
        {
            return GenericFilterOperation<TSource, TFilter, GreaterOrEqualAttribute>(
                propertyInfo,
                source,
                filter,
                Expression.GreaterThanOrEqual
            );
        }

        if (Attribute.IsDefined(propertyInfo, typeof(GreaterAttribute)))
        {
            return GenericFilterOperation<TSource, TFilter, GreaterAttribute>(
                propertyInfo,
                source,
                filter,
                Expression.GreaterThan
            );
        }

        if (Attribute.IsDefined(propertyInfo, typeof(LessOrEqualAttribute)))
        {
            return GenericFilterOperation<TSource, TFilter, LessOrEqualAttribute>(
                propertyInfo,
                source,
                filter,
                Expression.LessThanOrEqual
            );
        }

        if (Attribute.IsDefined(propertyInfo, typeof(LessAttribute)))
        {
            return GenericFilterOperation<TSource, TFilter, LessAttribute>(
                propertyInfo,
                source,
                filter,
                Expression.LessThan
            );
        }

        throw new ArgumentException("Invalid custom comparison");
    }


    private static IQueryable<TSource> ListContains<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter
    )
    {
        var value = x.GetValue(filter);
        if (value == null)
        {
            return source;
        }

        if (value is not ICollection col || col.Count == 0)
        {
            return source;
        }

        var underlyingCollectionType = value.GetType().GetGenericArguments()[0];

        var propertyName = GetCustomAttributeValue<SourcePropertyNameAttribute>(x, x.Name);
        var parameterExp = Expression.Parameter(typeof(TSource), propertyName);
        var method = col.GetType().GetMethod("Contains");
        Expression property = GetFieldOrProperty(parameterExp);

        if (IsNullableType(property.Type) && !IsNullableType(underlyingCollectionType))
        {
            property = Expression.Call(property, "GetValueOrDefault", Type.EmptyTypes);
        }

        var call = Expression.Call(
            Expression.Constant(col),
            method,
            property
        );
        var lambda = Expression.Lambda<Func<TSource, bool>>(call, parameterExp);

        return source.Where(lambda);
    }

    private static string GetCustomAttributeValue<T>(ICustomAttributeProvider x, string defaultValue)
        where T : Attribute, IAttributeWithName
    {
        var attr = ((T[])x.GetCustomAttributes(
            typeof(T),
            false
        )).Where(o => !string.IsNullOrWhiteSpace(o.Name));

        return attr.Any() ? attr.First().Name : defaultValue;
    }

    private static IQueryable<TSource> StringContains<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter
    )
    {
        var value = x.GetValue(filter);
        if (value == null)
        {
            return source;
        }

        var parameterExp = Expression.Parameter(typeof(TFilter), x.Name);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var propertyExp = Expression.Property(parameterExp, x.Name);
        var someValue = Expression.Constant(value, typeof(string));
        var containsMethodExp = Expression.Call(propertyExp, method, someValue);

        var lambda = Expression.Lambda<Func<TSource, bool>>(containsMethodExp, parameterExp);

        return source.Where(lambda);
    }

    private static IQueryable<TSource> StringContainsWithOr<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter
    )
    {
        var value = x.GetValue(filter);
        if (value == null)
        {
            return source;
        }

        var parameterExp = Expression.Parameter(typeof(TFilter), x.Name);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var propertyExp = Expression.Property(parameterExp, x.Name);
        var someValue = Expression.Constant(value, typeof(string));
        var containsMethodExp = Expression.Call(propertyExp, method, someValue);

        var name = GetCustomAttributeValue<OrFilterByAttribute>(x, x.Name);
        var propertyExp2 = Expression.Property(parameterExp, name);
        var containsMethodExp2 = Expression.Call(propertyExp2, method, someValue);

        var or = Expression.OrElse(containsMethodExp, containsMethodExp2);
        var lambda = Expression.Lambda<Func<TSource, bool>>(or, parameterExp);

        return source.Where(lambda);
    }

    private static IQueryable<TSource> StringContainsIgnoreCase<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter
    )
    {
        var value = x.GetValue(filter);
        if (value == null)
        {
            return source;
        }

        var parameterExp = Expression.Parameter(typeof(TFilter), x.Name);
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
        var propertyExp = Expression.Property(parameterExp, x.Name);
        var someValue = Expression.Constant(value, typeof(string));
        var comparison = Expression.Constant(StringComparison.OrdinalIgnoreCase, typeof(StringComparison));
        var containsMethodExp = Expression.Call(propertyExp, method, someValue, comparison);

        var lambda = Expression.Lambda<Func<TSource, bool>>(containsMethodExp, parameterExp);

        return source.Where(lambda);
    }

    private static MemberExpression GetFieldOrProperty(ParameterExpression parameter)
    {
        var fieldsWithMatchingName = parameter.Type.GetFields()
            .Where(q => parameter.Name == q.Name)
            .ToList();
        var propertiesWithMatchingName = parameter.Type.GetProperties()
            .Where(q => parameter.Name == q.Name)
            .ToList();
        var fieldInfoFromDeclaringType = fieldsWithMatchingName
            .FirstOrDefault(q => q.DeclaringType == parameter.Type);
        var propertyInfoFromDeclaringType = propertiesWithMatchingName
            .FirstOrDefault(q => q.DeclaringType == parameter.Type);

        if (propertyInfoFromDeclaringType != null)
        {
            return Expression.Property(parameter, propertyInfoFromDeclaringType);
        }

        if (propertiesWithMatchingName.Any())
        {
            var propertyInfo = propertiesWithMatchingName.First();
            return Expression.Property(parameter, propertyInfo);
        }

        if (fieldInfoFromDeclaringType != null)
        {
            return Expression.Field(parameter, fieldInfoFromDeclaringType);
        }


        var fieldMatchingName = fieldsWithMatchingName.First();
        return fieldsWithMatchingName.Any()
            ? Expression.Field(parameter, fieldMatchingName)
            : Expression.PropertyOrField(parameter, parameter.Name);
    }

    private static IQueryable<TSource> NullableEqual<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter
    )
    {
        var value = x.GetValue(filter);
        var compareNull = Attribute.IsDefined(x, typeof(EqualNullAttribute));
        if (value == null && !compareNull)
        {
            return source;
        }

        var parameter = Expression.Parameter(typeof(TSource), x.Name);
        var targetField = GetFieldOrProperty(parameter);
        var lambda = Expression.Lambda<Func<TSource, bool>>(
            Expression.Equal(
                targetField,
                Expression.Constant(value, targetField.Type)
            ),
            parameter
        );
        return source.Where(lambda);
    }

    private static IQueryable<TSource> GenericFilterOperation<TSource, TFilter, TCustomAttribute>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter,
        Func<Expression, Expression, Expression> expr
    ) where TCustomAttribute : Attribute, IAttributeWithName
    {
        var value = x.GetValue(filter);
        if (value == null)
        {
            return source;
        }

        var sourceField = (Expression)Expression.PropertyOrField(
            Expression.Constant(filter, typeof(TFilter)),
            x.Name
        );
        var propertyName = GetCustomAttributeValue<TCustomAttribute>(x, x.Name);
        var parameter = Expression.Parameter(typeof(TSource), propertyName);
        var targetField = GetFieldOrProperty(parameter);
        var sourcePropertyInfo = typeof(TSource)
            .GetProperties()
            .First(o => o.Name == propertyName);

        if (IsNullableType(x.PropertyType) && !IsNullableType(sourcePropertyInfo.PropertyType))
        {
            sourceField = Expression.Property(sourceField, "Value");
        }

        var lambda = Expression.Lambda<Func<TSource, bool>>(
            expr(targetField, sourceField),
            parameter
        );

        return source.Where(lambda);
    }

    private static bool IsNullableType(Type t)
    {
        return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    private static IQueryable<TSource> NullableStringContains<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter,
        Type xPropertyType
    )
    {
        return NullableStringMethodCall(x, source, filter, xPropertyType, "Contains");
    }

    private static IQueryable<TSource> NullableStringStartsWith<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter,
        Type xPropertyType
    )
    {
        return NullableStringMethodCall(x, source, filter, xPropertyType, "StartsWith");
    }

    private static IQueryable<TSource> NullableStringMethodCall<TSource, TFilter>(
        PropertyInfo x,
        IQueryable<TSource> source,
        TFilter filter,
        Type xPropertyType,
        string methodCall
    )
    {
        var value = x.GetValue(filter);
        if (value == null)
        {
            return source;
        }

        var parameterExp = Expression.Parameter(typeof(TFilter), x.Name);
        var method = typeof(string).GetMethod(methodCall, new[] { typeof(string) });
        var propertyExp = Expression.Property(parameterExp, x.Name);
        var someValue = Expression.Constant(value, xPropertyType);
        var containsMethodExp = Expression.Call(MakeString(propertyExp), method, MakeString(someValue));

        var lambda = Expression.Lambda<Func<TSource, bool>>(containsMethodExp, parameterExp);

        return source.Where(lambda);
    }

    private static Expression MakeString(Expression source)
    {
        return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
    }

    public static Type[] GetEqualTypes()
    {
        return new[]
        {
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(short),
            typeof(short?),
            typeof(double?),
            typeof(double),
            typeof(decimal),
            typeof(decimal?),
            typeof(bool),
            typeof(bool?),
            typeof(DateTime),
            typeof(DateTime?)
        };
    }

    public static Type[] GetContainsTypes()
    {
        return new[]
        {
            typeof(string)
        };
    }
}