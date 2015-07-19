using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class FileManager : MonoBehaviour{
	
	StreamWriter sw;
	string folderPath,dataPath;
	FpsCounter fpsCounter;
	int dataNum = 0;

	void Start()
	{
		fpsCounter = gameObject.GetComponent<FpsCounter>();

		DateTime dtNow = DateTime.Now;
		if (SystemInfo.deviceType != DeviceType.Handheld)
			dataPath = Application.dataPath + "/OutputData/";
		else
			dataPath = Application.persistentDataPath + "/";
		
		folderPath = dataPath + dtNow.Year.ToString () + "_" + 
			dtNow.Month.ToString ("00") + dtNow.Day.ToString ("00") + "_" + 
				dtNow.Hour.ToString ("00") + dtNow.Minute.ToString ("00");

		if (!Directory.Exists("Assets/output_images"))
			Directory.CreateDirectory(folderPath);

		Debug.Log (folderPath);
	}

	public void dataOutput(Vector3 pos, Vector3 accel, string direction, float radian, string mode){
		
		FileInfo fi;
		fi = new FileInfo(folderPath + "/data" + dataNum + ".csv");
		sw = fi.AppendText();
		sw.Write (pos.x);
		sw.Write (',');
		sw.Write (pos.y);
		sw.Write (',');
		sw.Write (accel.x);
		sw.Write (',');
		sw.Write (accel.y);
		sw.Write (',');
		sw.Write (direction);
		sw.Write (',');
		sw.Write (radian);
		sw.Write (',');
		sw.Write(mode);
		sw.Write (',');
		sw.WriteLine(fpsCounter.getFps());

		sw.Flush();
		sw.Close();
	}

	public void stringOutput(string str)
	{
		FileInfo fi;
		fi = new FileInfo(folderPath + "/data" + dataNum + ".csv");
		sw = fi.AppendText();

		sw.WriteLine (str);

		sw.Flush();
		sw.Close();
	}

	public void captureScreen()
	{
		Application.CaptureScreenshot (folderPath + "/pic" + dataNum + ".png");
		dataNum++;
	}
}