using UnityEngine;
using System.Collections;
public class CameraAdjust : MonoBehaviour
{
    float width = 1920; 
    float height = 1200;

    void Awake()
    {
        //屏幕适配
        float orthographicSize = this.GetComponent<Camera>().orthographicSize;
        print("aa:" + orthographicSize);
        float scale = (Screen.height / (float)Screen.width) / (height / width);
        orthographicSize *= scale;
        print("center:" + scale + "shei" + Screen.height + Screen.width);
        print("bb:" + orthographicSize);
        this.GetComponent<Camera>().orthographicSize = orthographicSize;
    }
}