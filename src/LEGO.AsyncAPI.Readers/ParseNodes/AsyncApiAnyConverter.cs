using System;
using System.Globalization;
using System.Linq;
using System.Text;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal static class AsyncApiAnyConverter
    {
        public static IAsyncApiAny GetSpecificAsyncApiAny(IAsyncApiAny asyncApiAny, AsyncApiSchema schema = null)
        {
            if (asyncApiAny is AsyncApiArray asyncApiArray)
            {
                var newArray = new AsyncApiArray();
                foreach (var element in asyncApiArray)
                {
                    newArray.Add(GetSpecificAsyncApiAny(element, schema?.Items));
                }

                return newArray;
            }

            if (asyncApiAny is AsyncApiObject asyncApiObject)
            {
                var newObject = new AsyncApiObject();

                foreach (var key in asyncApiObject.Keys.ToList())
                {
                    if (schema?.Properties != null && schema.Properties.TryGetValue(key, out var property))
                    {
                        newObject[key] = GetSpecificAsyncApiAny(asyncApiObject[key], property);
                    }
                    else
                    {
                        newObject[key] = GetSpecificAsyncApiAny(asyncApiObject[key], schema?.AdditionalProperties);
                    }
                }

                return newObject;
            }

            if (!(asyncApiAny is AsyncApiString))
            {
                return asyncApiAny;
            }

            var value = ((AsyncApiString)asyncApiAny).Value;
            var type = schema?.Type;
            var format = schema?.Format;

            if (((AsyncApiString)asyncApiAny).IsExplicit())
            {
                if (schema == null)
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                    {
                        return new AsyncApiDateTime(dateTimeValue);
                    }
                }
                else if (type == "string")
                {
                    if (format == "byte")
                    {
                        try
                        {
                            return new AsyncApiByte(Convert.FromBase64String(value));
                        }
                        catch (FormatException)
                        { }
                    }

                    if (format == "binary")
                    {
                        try
                        {
                            return new AsyncApiBinary(Encoding.UTF8.GetBytes(value));
                        }
                        catch (EncoderFallbackException)
                        { }
                    }

                    if (format == "date")
                    {
                        if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                        {
                            return new AsyncApiDate(dateValue.Date);
                        }
                    }

                    if (format == "date-time")
                    {
                        if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                        {
                            return new AsyncApiDateTime(dateTimeValue);
                        }
                    }
                }

                return asyncApiAny;
            }

            if (value == null || value == "null")
            {
                return new AsyncApiNull();
            }

            if (schema?.Type == null)
            {
                if (value == "true")
                {
                    return new AsyncApiBoolean(true);
                }

                if (value == "false")
                {
                    return new AsyncApiBoolean(false);
                }

                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    return new AsyncApiInteger(intValue);
                }

                if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                {
                    return new AsyncApiLong(longValue);
                }

                if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    return new AsyncApiDouble(doubleValue);
                }

                if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                {
                    return new AsyncApiDateTime(dateTimeValue);
                }
            }
            else
            {
                if (type == "integer" && format == "int32")
                {
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                    {
                        return new AsyncApiInteger(intValue);
                    }
                }

                if (type == "integer" && format == "int64")
                {
                    if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                    {
                        return new AsyncApiLong(longValue);
                    }
                }

                if (type == "integer")
                {
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                    {
                        return new AsyncApiInteger(intValue);
                    }
                }

                if (type == "number" && format == "float")
                {
                    if (float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var floatValue))
                    {
                        return new AsyncApiFloat(floatValue);
                    }
                }

                if (type == "number" && format == "double")
                {
                    if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                    {
                        return new AsyncApiDouble(doubleValue);
                    }
                }

                if (type == "number")
                {
                    if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                    {
                        return new AsyncApiDouble(doubleValue);
                    }
                }

                if (type == "string" && format == "byte")
                {
                    try
                    {
                        return new AsyncApiByte(Convert.FromBase64String(value));
                    }
                    catch (FormatException)
                    { }
                }

                // binary
                if (type == "string" && format == "binary")
                {
                    try
                    {
                        return new AsyncApiBinary(Encoding.UTF8.GetBytes(value));
                    }
                    catch (EncoderFallbackException)
                    { }
                }

                if (type == "string" && format == "date")
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                    {
                        return new AsyncApiDate(dateValue.Date);
                    }
                }

                if (type == "string" && format == "date-time")
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                    {
                        return new AsyncApiDateTime(dateTimeValue);
                    }
                }

                if (type == "string")
                {
                    return asyncApiAny;
                }

                if (type == "boolean")
                {
                    if (bool.TryParse(value, out var booleanValue))
                    {
                        return new AsyncApiBoolean(booleanValue);
                    }
                }
            }
            return asyncApiAny;
        }
    }
}
