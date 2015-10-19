#if UNITY_EDITOR 
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

using Magicolo.EditorTools;
using RickEditor.Editor;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class AutoTileShowerHelper {
	
		const int EMPTY = 0;
		const int CENTER = 1;
		const int SIDE_NORTH = 2;
		const int SIDE_EST = 3;
		const int SIDE_SOUTH = 4;
		const int SIDE_WEST = 5;
		const int CORNER_INSIDE_NW = 6;
		const int CORNER_INSIDE_NE = 7;
		const int CORNER_INSIDE_SE = 8;
		const int CORNER_INSIDE_SW = 9;
		const int CORNER_OUTSIDE_NW = 10;
		const int CORNER_OUTSIDE_NE = 11;
		const int CORNER_OUTSIDE_SE = 12;
		const int CORNER_OUTSIDE_SW = 13;
		
		
		const int TILE_EMPTY = 0;
		const int TILE_CENTER = 4;
		const int TILE_STRAIGHT = 2;
		const int TILE_CORNER_INSIDE = 3;
		const int TILE_CORNER_OUTSIDE = 1;
		GUIStyle style;
		
		public AutotileData currentAutotile;
		
		Texture2D[] exempleTextures;
		Texture2D[] texture;
		
		Rect startRect;
		
		

		public void setCurrentautoTile(AutotileData currentAutotile) {
			this.currentAutotile = currentAutotile;
			loadTextures();
		}		
		public void loadTextures() {
			Sprite[] sprites = Resources.LoadAll<Sprite>("MapLoader/autotileExemple");
			
			exempleTextures = new Texture2D[14]; // METTRE LE MAX ET PAS UN CHFFRE MAGIQUE
			texture = new Texture2D[14]; // METTRE LE MAX ET PAS UN CHFFRE MAGIQUE
			
			addSingleTile(exempleTextures, EMPTY					, sprites[TILE_EMPTY]);
			addRotationsTile(exempleTextures, CORNER_INSIDE_NW	, sprites[TILE_CORNER_INSIDE]);
			addRotationsTile(exempleTextures, SIDE_NORTH			, sprites[TILE_STRAIGHT]);
			addRotationsTile(exempleTextures, CORNER_OUTSIDE_NW	, sprites[TILE_CORNER_OUTSIDE]);
			addSingleTile(exempleTextures, CENTER				, sprites[TILE_CENTER]);
			
			addSingleTile(texture, EMPTY					, sprites[TILE_EMPTY]);
			if(currentAutotile != null){
				tryAddSingleTile(currentAutotile.center, CENTER);
				tryAddRotationTiles(currentAutotile.cornerInside, CORNER_INSIDE_NW);
				tryAddRotationTiles(currentAutotile.side, SIDE_NORTH);
				tryAddRotationTiles(currentAutotile.cornerOutside, CORNER_OUTSIDE_NW);
			}
		}
		
		void tryAddSingleTile(List<Sprite> spriteList, int index) {
			if(spriteList.Count != 0){
				addSingleTile(texture, index, spriteList[0]);
			}
		}

		void tryAddRotationTiles(List<Sprite> spriteList, int index) {
			if(spriteList.Count != 0){
				addRotationsTile(texture, index, spriteList[0]);
			}
		}


		void addSingleTile(Texture2D[] array, int index, Sprite sprite) {
			array[index] = TextureUtils.textureFromSprite(sprite);
		}

		void makeSking() {
			style = GUI.skin.label;
			style.margin = new RectOffset(0,0,0,0);
			style.padding = new RectOffset(0,0,0,0);
			style.border = new RectOffset(0,0,0,0);
			style.stretchWidth = false;
			style.stretchHeight = false;
			style.fixedWidth = 32;
		}
		
		void addRotationsTile(Texture2D[] array, int startingIndex, Sprite sprite) {
			List<Texture2D> rotated = TextureUtils.texturesWithRotationsFromSprite(sprite);
			array[startingIndex] = rotated[0];
			array[startingIndex + 1] = rotated[1];
			array[startingIndex + 2] = rotated[2];
			array[startingIndex + 3] = rotated[3];
		}			

		
		
		public void show() {
			if(style == null){
				makeSking();
			}
			
			
			startRect =	EditorGUILayout.BeginHorizontal();
			showTiles(CORNER_OUTSIDE_NW, SIDE_NORTH, SIDE_NORTH, SIDE_NORTH, CORNER_OUTSIDE_NE);
			GUILayout.Space(15);
			showTiles(EMPTY, CORNER_OUTSIDE_NW, SIDE_NORTH, CORNER_OUTSIDE_NE, EMPTY);
			EditorGUILayout.EndHorizontal();
			
			startRect =	EditorGUILayout.BeginHorizontal();
			showTiles(SIDE_WEST, CORNER_INSIDE_SE, SIDE_SOUTH, CORNER_INSIDE_SW, SIDE_EST);
			GUILayout.Space(15);
			showTiles(CORNER_OUTSIDE_NW, CORNER_INSIDE_NW, CENTER, CORNER_INSIDE_NE, CORNER_OUTSIDE_NE);
			EditorGUILayout.EndHorizontal();
			
			startRect =	EditorGUILayout.BeginHorizontal();
			showTiles(SIDE_WEST, SIDE_EST, EMPTY, SIDE_WEST, SIDE_EST);
			GUILayout.Space(15);
			showTiles(SIDE_WEST, CENTER, CENTER, CENTER, SIDE_EST);
			EditorGUILayout.EndHorizontal();
			
			
			startRect =	EditorGUILayout.BeginHorizontal();
			showTiles(SIDE_WEST, CORNER_INSIDE_NE, SIDE_NORTH, CORNER_INSIDE_NW, SIDE_EST);
			GUILayout.Space(15);
			showTiles(CORNER_OUTSIDE_SW, CORNER_INSIDE_SW, CENTER, CORNER_INSIDE_SE, CORNER_OUTSIDE_SE);
			EditorGUILayout.EndHorizontal();
			
			startRect =	EditorGUILayout.BeginHorizontal();
			showTiles(CORNER_OUTSIDE_SW, SIDE_SOUTH, SIDE_SOUTH, SIDE_SOUTH, CORNER_OUTSIDE_SE);
			GUILayout.Space(15);
			showTiles(EMPTY, CORNER_OUTSIDE_SW, SIDE_SOUTH, CORNER_OUTSIDE_SE, EMPTY);
			EditorGUILayout.EndHorizontal();
		}
		
		void showTiles(params int[] tiles){
			int x = 0;
			foreach (var index in tiles) {
				Rect showRect = new Rect(startRect.left + x * 32 , startRect.top, 32,32);
				showTile(showRect, index);
				x++;
			}
		}

		void showTile(Rect showRect, int tileIndex) {
			GUIContent content = null;
			if(texture[tileIndex] != null){
				content = new GUIContent(texture[tileIndex]);
			}else{
				content = new GUIContent(exempleTextures[tileIndex]);
			}
			//GUI.DrawTexture(showRect,exempleTextures[tileIndex]);
			EditorGUIUtility.fieldWidth = 16;
			GUILayout.Label(content,style, GUILayout.Width(32));
		}
		
		
	}
}
#endif