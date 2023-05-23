using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.Security.ApplicationId.PolicyManagement.PolicyModel
{
  public abstract class PolicyElement : XmlSerializer, ICloneable
  {
    public object Clone() => this.CloneObject();

    protected abstract object CloneObject();

    public static int Compare<T>(T element1, T element2) where T : IComparable<T>
    {
      if ((object) element1 == (object) element2)
        return 0;
      if ((object) element1 != null && (object) element2 == null)
        return 1;
      return (object) element1 == null && (object) element2 != null ? -1 : element1.CompareTo(element2);
    }

    protected static string CollectionToString<T>(ICollection<T> collection) where T : PolicyElement
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (T obj in (IEnumerable<T>) collection)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(';');
        stringBuilder.Append(obj.ToString());
      }
      return stringBuilder.ToString();
    }
  }
}
