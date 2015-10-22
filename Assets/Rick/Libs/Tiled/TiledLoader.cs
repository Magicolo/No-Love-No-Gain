using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Rick.TiledMapLoader{
	public abstract class TiledLoader {
	
		protected Dictionary<string, string> makePropertiesDictionary(XElement propertiesElement){
			Dictionary<string, string> properties = new Dictionary<string, string>();
			if(propertiesElement == null) return properties;
			
			foreach (var property in propertiesElement.Elements("property")) {
				string name = property.Attribute("name").Value;
				string value = property.Attribute("value").Value;
				properties.Add(name,value);
			}
			
			return properties;
		}

        protected Dictionary<string, string> makePropertiesDictionaryFromChild(XElement obj)
        {
            
            Dictionary<string, string> properties = new Dictionary<string, string>();
            if (!obj.Descendants("elements").Any())
            {
                return properties;
            }
            XElement propertiesElement = obj.Descendants("elements").First();
            foreach (var property in propertiesElement.Elements("property"))
            {
                string name = property.Attribute("name").Value;
                string value = property.Attribute("value").Value;
                properties.Add(name, value);
            }

            return properties;
        }

        protected ulong parseULong(string longStr){
			try{
				ulong id = UInt64.Parse(longStr);
				return id;
			}catch (OverflowException){
				UnityEngine.Debug.LogError(longStr + " overflow the memory :(");
			}
			return 0;
		}
		
		protected long parseLong(string longStr){
			try{
				long id = Int64.Parse(longStr);
				return id;
			}catch (OverflowException){
				UnityEngine.Debug.LogError(longStr + " overflow the memory :(");
			}
			return -1;
		}
		
		protected int parseInt(string intStr){
			try{
				int id = Int32.Parse(intStr);
				return id;
			}catch (OverflowException){
				UnityEngine.Debug.LogError(intStr + " overflow the memory :(");
			}
			return -1;
		}
		
		
		protected void outputError(string message){
			UnityEngine.Debug.LogError(message);
		
		}
	}
}