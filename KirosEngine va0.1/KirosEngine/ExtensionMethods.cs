using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace KirosEngine
{
    public static class ExtensionMethods
    {
        public static string TryGetElementValue(this XElement parentEl, string elementName, XNamespace nameSpace = null, string defaultValue = null)
        {
            XElement foundEl;
            if(nameSpace != null)
            {
                foundEl = parentEl.Element(nameSpace + elementName);
            }
            else
            {
                foundEl = parentEl.Element(elementName);
            }

            if (foundEl != null)
            {
                return foundEl.Value;
            }

            return defaultValue;
        }
    }
}
