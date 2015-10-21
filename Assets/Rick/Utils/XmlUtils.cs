using UnityEngine;
using System.Collections;
using System.Xml;

public class XmlUtils
{
    public static XmlDocument load( string resourceFile )
    {
        TextAsset textAsset = (TextAsset)Resources.Load(resourceFile);
        if (textAsset == null)
        {
            throw new System.Exception("File '" + resourceFile + "' not found in resources noob.");
        }
        else
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textAsset.text);
            return xmldoc;
        }
        
    }
}
