#if UNITY_EDITOR 
using System;
using System.Xml;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using Rick.TiledMapLoader;

namespace RickTools.MapLoader{
	
	public class SimpleAutoTileExporter {
	
		XmlDocument doc;
		XmlNode map;
		
		string sourceTileset;
		List<int> idsCenter;
		List<int> idsSide;
		List<int> idsCornerIn;
		List<int> idsCornerOut;
		List<List<int>> allLists;
		
		int maxVariation = 0;
		
		public SimpleAutoTileExporter(string sourceTileset, List<int> idsCenter, List<int> idsSide, List<int> idsCornerIn, List<int> idsCornerOut) {
			this.sourceTileset = sourceTileset;
			this.idsCenter = idsCenter;
			this.idsSide = idsSide;
			this.idsCornerIn = idsCornerIn;
			this.idsCornerOut = idsCornerOut;
			
			allLists = new List<List<int>>();
			allLists.Add(idsCenter);
			allLists.Add(idsSide);
			allLists.Add(idsCornerIn);
			allLists.Add(idsCornerOut);
			
			foreach (var idList in allLists) {
				maxVariation = Mathf.Max(maxVariation, idList.Count);	
			}
		}
		
		public XmlDocument generateDocument(){
			doc = new XmlDocument();
			
			createAndAppendMapNode();
			createAndAppendAttributes();
			addTileset(sourceTileset);
			
			createAndAppendRegionsLayer();
			
			for (int i = 0; i < maxVariation; i++) {
				createAndAppendInputRegion(i);
			}
			for (int i = 0; i < maxVariation; i++) {
				createAndAppendInputRemoverStuffRegion(i);
			}
			for (int i = 0; i < maxVariation; i++) {
				//createAndAppendNotInputRegion(i);
			}
			for (int i = 0; i < maxVariation; i++) {
				createAndAppendOutputRegion(i);
			}
			
			return doc;
		}
		
		
		

		void createAndAppendMapNode() {
			map = doc.CreateElement("map");
			doc.AppendChild(map);
			
			map.Attributes.Append(createAttribut(doc,"version","1.0"));
			map.Attributes.Append(createAttribut(doc,"orientation","orthogonal"));
			map.Attributes.Append(createAttribut(doc,"renderorder","right-up"));
			map.Attributes.Append(createAttribut(doc,"width","3"));
			map.Attributes.Append(createAttribut(doc,"height","43"));
			map.Attributes.Append(createAttribut(doc,"tilewidth","32"));
			map.Attributes.Append(createAttribut(doc,"tileheight","32"));
			map.Attributes.Append(createAttribut(doc,"nextobjectid","1"));
		}	

		void createAndAppendAttributes() {
			XmlNode properties = doc.CreateElement("properties");
			map.AppendChild(properties);
			
			XmlNode automappingRadius = doc.CreateElement("property");
			properties.AppendChild(automappingRadius);
			automappingRadius.Attributes.Append(createAttribut(doc,"name","AutomappingRadius"));
			automappingRadius.Attributes.Append(createAttribut(doc,"value","3"));
			
			XmlNode NoOverlappingRules = doc.CreateElement("property");
			properties.AppendChild(NoOverlappingRules);
			NoOverlappingRules.Attributes.Append(createAttribut(doc,"name","NoOverlappingRules"));
			NoOverlappingRules.Attributes.Append(createAttribut(doc,"value","False"));
			
		}	

		void addTileset(string sourceTileSet) {
			XmlNode tileset = doc.CreateElement("tileset");
			map.AppendChild(tileset);
			tileset.Attributes.Append(createAttribut(doc,"firstgid","1"));
			tileset.Attributes.Append(createAttribut(doc,"source",sourceTileSet));
		}

		void createAndAppendInputRemoverStuffRegion(int i) {
			createAndAppendInputRemoverStuffRegion(idsCornerIn, i);
			createAndAppendInputRemoverStuffRegion(idsCornerOut, i);
			createAndAppendInputRemoverStuffRegion(idsSide, i);
			
		}
		
		void createAndAppendInputRemoverStuffRegion(List<int> list, int variation) {
			currentDataNode = createAndAppendLayerReturnDataNode("input_Tiles", "3", "3", "0");
			
			
			appendCurrentDataNodeLine(format(list,variation,0), 0, format(list,variation,90));
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(list,variation,180), 0, format(list,variation,270));
			
			fixCurrentDataEndLine();
		}
		
		XmlNode currentDataNode;
		void createAndAppendRegionsLayer() {
			currentDataNode = createAndAppendLayerReturnDataNode("regions", "3", "43", "0");
			
			appendCurrentDataNodeLine(1, 0, 1);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(1, 0, 1);
			appendEmptyLine();
			
			//CornerOUT
			appendCurrentDataNodeLine(0, 1, 0);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(0, 1, 1);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 1, 0);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(1, 1, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(1, 1, 0);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(0, 1, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 1, 1);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(0, 1, 0);
			appendEmptyLine();
			
			//CornerIn
			appendCurrentDataNodeLine(0, 1, 0);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(0, 1, 1);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 1, 0);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(1, 1, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(1, 1, 0);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(0, 1, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 1, 1);
			appendCurrentDataNodeLine(1, 1, 1);
			appendCurrentDataNodeLine(0, 1, 0);
			appendEmptyLine();
			
			//Sides
			
			appendCurrentDataNodeLine(1, 0, 1);
			appendCurrentDataNodeLine(1, 0, 1);
			appendCurrentDataNodeLine(1, 0, 1);
			appendEmptyLine();
			appendCurrentDataNodeLine(1, 1, 1);
			appendEmptyLine();
			appendCurrentDataNodeLine(1, 1, 1);
			
			
			fixCurrentDataEndLine();
		}

		void createAndAppendOutputRegion(int i) {
			currentDataNode = createAndAppendLayerReturnDataNode("output_Tiles", "3", "43", "0");
			
			appendCurrentDataNodeLine(format(idsCenter,0,0), 0, format(idsCenter,0,0));
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsCenter,0,0), 0, format(idsCenter,0,0));
			appendCurrentDataNodeLine(0, 0, 0);
			
			//CornerOUT
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerOut,i), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerOut,i,90), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerOut,i,180), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerOut,i,270), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			
			
			//CornerIn
		
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerIn,i), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerIn,i,90), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerIn,i,180), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerIn,i,270), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			
			//SIDE
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsSide,i,0), 0, format(idsSide,i,180));
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsSide,i,90), 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsSide,i,270), 0);
		
			fixCurrentDataEndLine();
		}	

		void createAndAppendInputRegion(int index) {
			currentDataNode = createAndAppendLayerReturnDataNode("input_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			ulong center = format(idsCenter,index);
			
			//CornerOUT
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, center, center);
			appendCurrentDataNodeLine(0, center, center);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(center, center, 0);
			appendCurrentDataNodeLine(center, center, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(center, center, 0);
			appendCurrentDataNodeLine(center, center, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, center, center);
			appendCurrentDataNodeLine(0, center, center);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			
			//CornerIn
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,0), 0);
			appendCurrentDataNodeLine(format(idsCornerOut,index,0), center, center);
			appendCurrentDataNodeLine(0, center, center);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,90), 0);
			appendCurrentDataNodeLine(center, center, format(idsCornerOut,index, 90));
			appendCurrentDataNodeLine(center, center, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(center, center, 0);
			appendCurrentDataNodeLine(center, center, format(idsCornerOut,index, 180));
			appendCurrentDataNodeLine(0, format(idsCornerOut,index, 180), 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, center, center);
			appendCurrentDataNodeLine(format(idsCornerOut,index, 270), center, center);
			appendCurrentDataNodeLine(0, format(idsCornerOut,index, 270), 0);
			appendEmptyLine();
			
			
			appendCurrentDataNodeLine(0, 0, center);
			appendCurrentDataNodeLine(center, 0, center);
			appendCurrentDataNodeLine(center, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(center, center, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, center, center);
			
			fixCurrentDataEndLine();
			
			
			
			
			currentDataNode = createAndAppendLayerReturnDataNode("input_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
		
			//CornerOUT
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsCornerOut,index,0),0 , 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(format(idsCornerOut,index,0),format(idsCornerOut,index,90), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,0), format(idsCornerOut,index,90));
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index,180));
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			
			
			//CornerIn
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,0), 0);
			appendCurrentDataNodeLine(format(idsCornerOut,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,90), 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index, 90));
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index, 180));
			appendCurrentDataNodeLine(0, format(idsCornerOut,index, 180), 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsCornerOut,index, 270), 0, 0);
			appendCurrentDataNodeLine(0, format(idsCornerOut,index, 270), 0);
			appendEmptyLine();
			
			//Side
			appendCurrentDataNodeLine(0, 0, format(idsSide,index,0));
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, format(idsSide,index,90));
			
			fixCurrentDataEndLine();
		}
		
		void createAndAppendNotInputRegion(int index) {
			currentDataNode = createAndAppendLayerReturnDataNode("inputnot_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			//CornerOUT
			appendCurrentDataNodeLine(0, format(idsCenter,index,0), 0);
			appendCurrentDataNodeLine(format(idsCenter,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsCenter,index,0), 0);
			appendCurrentDataNodeLine(0, 0, format(idsCenter,index,0));
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCenter,index,0));
			appendCurrentDataNodeLine(0, format(idsCenter,index,0), 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsCenter,index,0), 0, 0);
			appendCurrentDataNodeLine(0, format(idsCenter,index,0), 0);
			appendEmptyLine();
			
			//CornerIn
			appendEmptyLines(16);
			//Side
			appendCurrentDataNodeLine(format(idsCenter,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCenter,index,0));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, format(idsCenter,index,0));
			appendEmptyLine();
			appendCurrentDataNodeLine(format(idsCenter,index,0), 0, 0);
			
			fixCurrentDataEndLine();
			
			
			
			currentDataNode = createAndAppendLayerReturnDataNode("inputnot_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			//CornerOUT
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,0), 0);
			appendCurrentDataNodeLine(format(idsCornerOut,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsCornerOut,index,90), 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsCornerOut,index,270), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			
			//CornerIn
			appendEmptyLines(16);
			//Side
			appendCurrentDataNodeLine(format(idsCornerOut,index,90), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index,180));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, format(idsSide,index,90));
			appendEmptyLine();
			appendCurrentDataNodeLine(format(idsSide,index,270), 0, 0);
			
			fixCurrentDataEndLine();
			
			
			
			
			currentDataNode = createAndAppendLayerReturnDataNode("inputnot_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			//CornerOUT 
			appendEmptyLines(16);
			//Corner IN
			appendCurrentDataNodeLine(0, format(idsSide,index,270), 0);
			appendCurrentDataNodeLine(format(idsSide,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, format(idsSide,index,90), 0);
			appendCurrentDataNodeLine(0, 0, format(idsSide,index,0));
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsSide,index,180));
			appendCurrentDataNodeLine(0, format(idsSide,index,0), 90);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(format(idsSide,index,180), 0, 0);
			appendCurrentDataNodeLine(0, format(idsSide,index,270), 0);
			appendEmptyLine();
			
			//Side
			appendCurrentDataNodeLine(format(idsSide,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index,270));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index,180));
			appendEmptyLine();
			appendCurrentDataNodeLine(format(idsSide,index,0), 0, 0);
			
			fixCurrentDataEndLine();
			
			
			
			
			
			currentDataNode = createAndAppendLayerReturnDataNode("inputnot_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			//CornerOUT 
			appendEmptyLines(16);
			//Corner IN
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerOut,index,180));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			
			//Side
			appendCurrentDataNodeLine(format(idsCornerOut,index,0), 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsSide,index,180));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			
			fixCurrentDataEndLine();
			
			
			
			
			currentDataNode = createAndAppendLayerReturnDataNode("inputnot_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			//CornerOUT AND IN
			appendEmptyLines(32);
			
			//Side
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerIn,index,180));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, format(idsCornerIn,index,180));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			
			fixCurrentDataEndLine();
			
			
			currentDataNode = createAndAppendLayerReturnDataNode("inputnot_Tiles", "3", "43", "0");
			
			appendEmptyLines(4);
			
			//CornerOUT AND IN
			appendEmptyLines(32);
			
			//Side
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, 0);
			appendCurrentDataNodeLine(0, 0, format(idsCornerIn,index,270));
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			appendEmptyLine();
			appendCurrentDataNodeLine(0, 0, 0);
			
			fixCurrentDataEndLine();
		}	
		
		
		void appendEmptyLines(int nb){
			for (int i = 0; i < nb; i++) {
				appendEmptyLine();
			}
		}
		
		void appendEmptyLine(){
			appendCurrentDataNodeLine(0,0, 0);
		}
		
		void appendCurrentDataNodeLine(params ulong[] tiles) {
			foreach (var tile in tiles) {
				currentDataNode.InnerText += tile + ",";
			}
			currentDataNode.InnerText += "\n";
		}
		
		public ulong format(List<int> ids, int index, int rotation = 0){
			if(index >= ids.Count){
				return 0;
			}else{
				int id = ids[index];
				switch(rotation){
					case 90: return (ulong)(id + 1) + TiledMapLoader.ROTATION_90_FLAG;
					case 180: return (ulong)(id + 1) + TiledMapLoader.ROTATION_180_FLAG;
					case 270: return (ulong)(id + 1) + TiledMapLoader.ROTATION_270_FLAG;				
					default : return (ulong)(id + 1);
				}
			}
		}
		
		public ulong format(int id, int rotation = 0){
			switch(rotation){
				case 90: return (ulong)(id + 1) + TiledMapLoader.ROTATION_90_FLAG;
				case 180: return (ulong)(id + 1) + TiledMapLoader.ROTATION_180_FLAG;
				case 270: return (ulong)(id + 1) + TiledMapLoader.ROTATION_270_FLAG;				
				default : return (ulong)(id + 1);
			}
		}

		void fixCurrentDataEndLine() {
			currentDataNode.InnerText = currentDataNode.InnerText.Substring(0, currentDataNode.InnerText.Length - 2) + "\n";
		}		
		
		
		XmlNode createAndAppendLayerReturnDataNode(string name, string width, string height, string visible) {
			XmlNode layer = doc.CreateElement("layer");
			map.AppendChild(layer);
			layer.Attributes.Append(createAttribut(doc,"name",name));
			layer.Attributes.Append(createAttribut(doc,"width",width));
			layer.Attributes.Append(createAttribut(doc,"height",height));
			layer.Attributes.Append(createAttribut(doc,"visible",visible));
			
			XmlNode data = doc.CreateElement("data");
			layer.AppendChild(data);
			data.Attributes.Append(createAttribut(doc,"encoding","csv"));
			data.InnerText = "\n";
			
			return data;
		}	
		
		protected XmlAttribute createAttribut(XmlDocument doc, string name, string value){
			XmlAttribute attribute = doc.CreateAttribute(name);
			attribute.Value = value;
			return attribute;
		}
	}
	
}
#endif

