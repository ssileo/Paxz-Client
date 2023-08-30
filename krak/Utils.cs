using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BasicTypes;
using Kernys.Bson;
using System.Net;
using System.Web;
using System.Net.Http;
using System.IO;
namespace krak
{
	internal class utilities
	{
		public static Vector2i ConvertWorldPointToMapPoint(Vector2 worldPoint)
		{
			// yes
			Vector2i mapPoint;
			mapPoint.x = 0; mapPoint.y = 0;
			// please do big sum
			mapPoint.x = (int)Mathf.Round(worldPoint.x / Globals.tileSize.x);
			mapPoint.y = (int)((worldPoint.y + (Globals.tileSize.y / 2f)) / Globals.tileSize.y);
			return mapPoint;
		}

		public static Vector2 ConvertMapPointToWorldPoint(Vector2i mapPoint)
		{
			Vector2 worldPoint;
			worldPoint.x = (float)mapPoint.x * Globals.tileSize.x;
			worldPoint.y = (float)mapPoint.y* Globals.tileSize.y;
			return worldPoint;
		}


		public static Vector2 ScreenToWorld(BasicTypes.Vector2i screenPoint)
		{
			return Camera.main.ScreenToWorldPoint(
				new Vector3(screenPoint.x, screenPoint.y)
			);
		}

		public static Vector2 GetLocalPos()
		{
			Vector3 transform = krak.KrakMonoBehaviour.localPlayer.myTransform.position;
			return new Vector2(transform.x, transform.y);
		}

		public static bool InBounds(BasicTypes.Vector2i point, BasicTypes.Vector2i bounds)
		{
			return !(point.x > bounds.x - 1 || point.x < 0 || point.y > bounds.y - 1 || point.y < 0);
		}

		public static Vector2 toFull(Vector2 pos)
		{
			int x = (int)Math.Round(pos.x / 0.32);
			int y = (int)Math.Round((pos.y + 0.16) / 0.32);

			return new Vector2(x, y);
		}

		public static Vector2 toFloat(Vector2 pos)
		{
			return new Vector2(pos.x = (float)(pos.x * 0.32), pos.y = (float)(pos.y * 0.32));
		}

		public static bool CanMove()
		{
			PlayerNamesManager.StatusIconType icon = KrakMonoBehaviour.namesManager.GetPlayerStatusIconType(KrakMonoBehaviour.playerData.playerId);
			return icon != PlayerNamesManager.StatusIconType.InMenus && icon != PlayerNamesManager.StatusIconType.Typing;
		}


		// country codes	
		public static short[] countrycodes =
		{

			4,
			248,
			8,
			12,
			16,
			20,
			24,
			660,
			10,
			28,
			32,
			51,
			533,
			36,
			40,
			31,
			44,
			48,
			50,
			52,
			112,
			56,
			84,
			204,
			60,
			64,
			68,
			535,
			70,
			72,
			74,
			76,
			86,
			96,
			100,
			854,
			108,
			116,
			120,
			124,
			132,
			136,
			140,
			148,
			152,
			156,
			162,
			166,
			170,
			174,
			178,
			180,
			184,
			188,
			384,
			191,
			192,
			531,
			196,
			203,
			208,
			262,
			212,
			214,
			218,
			818,
			222,
			226,
			232,
			233,
			231,
			238,
			234,
			242,
			246,
			250,
			254,
			258,
			260,
			266,
			270,
			268,
			276,
			288,
			292,
			300,
			304,
			308,
			312,
			316,
			320,
			831,
			324,
			624,
			328,
			332,
			334,
			336,
			340,
			344,
			348,
			352,
			356,
			360,
			364,
			368,
			372,
			833,
			376,
			380,
			388,
			392,
			832,
			400,
			398,
			404,
			296,
			408,
			410,
			414,
			417,
			418,
			428,
			422,
			426,
			430,
			434,
			438,
			440,
			442,
			446,
			807,
			450,
			454,
			458,
			462,
			466,
			470,
			584,
			474,
			478,
			480,
			175,
			484,
			583,
			498,
			492,
			496,
			499,
			500,
			504,
			508,
			104,
			516,
			520,
			524,
			999,
			528,
			540,
			554,
			558,
			562,
			566,
			570,
			574,
			580,
			578,
			512,
			586,
			585,
			275,
			591,
			598,
			600,
			604,
			608,
			612,
			616,
			620,
			630,
			634,
			638,
			642,
			643,
			646,
			652,
			654,
			659,
			662,
			663,
			666,
			670,
			882,
			674,
			678,
			682,
			686,
			688,
			690,
			694,
			702,
			534,
			703,
			705,
			90,
			706,
			710,
			239,
			728,
			724,
			144,
			729,
			740,
			744,
			748,
			752,
			756,
			760,
			158,
			762,
			834,
			764,
			626,
			768,
			772,
			776,
			780,
			788,
			792,
			795,
			796,
			798,
			800,
			804,
			784,
			826,
			840,
			581,
			858,
			860,
			548,
			862,
			704,
			92,
			850,
			876,
			732,
			887,
			894,
			716

		};

		public static void Movement(string dir)
        {
			Vector2 newTrans;
			if (KrakMonoBehaviour.localPlayer != null)
            {
				switch (dir)
				{
					case "left":
						newTrans = utilities.ConvertMapPointToWorldPoint(KrakMonoBehaviour.localPlayer.currentPlayerLeftMapPoint);
						KrakMonoBehaviour.localPlayer.myTransform.position = new Vector3(newTrans.x, KrakMonoBehaviour.localPlayer.myTransform.position.y, KrakMonoBehaviour.localPlayer.myTransform.position.z);
						break;
					case "right":
						newTrans = utilities.ConvertMapPointToWorldPoint(KrakMonoBehaviour.localPlayer.currentPlayerRightMapPoint);
						KrakMonoBehaviour.localPlayer.myTransform.position = new Vector3(newTrans.x, KrakMonoBehaviour.localPlayer.myTransform.position.y, KrakMonoBehaviour.localPlayer.myTransform.position.z);
						break;

				}
			}
			
        }
	}
}