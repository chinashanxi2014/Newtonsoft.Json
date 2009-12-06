﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Converters
{
  public class DataSetConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DataSet dataSet = (DataSet)value;

      DataTableConverter converter = new DataTableConverter();

      writer.WriteStartObject();

      foreach (DataTable table in dataSet.Tables)
      {
        writer.WritePropertyName(table.TableName);
        
        converter.WriteJson(writer, table, serializer);
      }

      writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
    {
      DataSet ds = new DataSet();

      DataTableConverter converter = new DataTableConverter();

      reader.Read();

      while (reader.TokenType == JsonToken.PropertyName)
      {
        DataTable dt = (DataTable)converter.ReadJson(reader, typeof (DataTable), serializer);
        ds.Tables.Add(dt);
      }

      reader.Read();

      return ds;
    }

    public override bool CanConvert(Type valueType)
    {
      return (valueType == typeof(DataSet));
    }
  }
}
#endif