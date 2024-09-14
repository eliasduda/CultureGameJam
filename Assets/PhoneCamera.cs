using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    bool camAvailiable, isFrontCam;
    int currentCam;
    WebCamTexture tex;
    Texture2D squareTexture;
    public RawImage display,finishedPicture;

    private int picture_count;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (camAvailiable)
        {
            int squareSize = Mathf.Min(tex.width, tex.height);  // Find the size for the square
            int xOffset = (tex.width - squareSize) / 2;  // Horizontal offset
            int yOffset = (tex.height - squareSize) / 2;  // Vertical offset

            // Create a new square Texture2D
            Texture2D squareTexture = new Texture2D(squareSize, squareSize);

            // Get the central pixels from the WebCamTexture
            Color[] pixels = tex.GetPixels(xOffset, yOffset, squareSize, squareSize);
            squareTexture.SetPixels(pixels);
            squareTexture.Apply();
            display.texture = squareTexture;

            int scaleY = tex.videoVerticallyMirrored ? -1 :  1;
            display.rectTransform.localScale = new Vector3(isFrontCam? -1 : 1, scaleY, 1);

            int orient = -tex.videoRotationAngle;
            display.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        }
    }

    public void SwitchWebCam()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            currentCam++;
            currentCam %= WebCamTexture.devices.Length;
            StartCamera();
        }
    }

    public void StartCameraCallback(string callbackText)
    {
        StartCamera();
    }

    public void StartCamera()
    {
        if (HasPermission(Permission.Camera))
        {
            Debug.Log("Start cam");
            WebCamDevice device = WebCamTexture.devices[currentCam];
            tex = new WebCamTexture(device.name);
            display.texture = squareTexture;
            tex.Play();
            camAvailiable = true;
            isFrontCam = device.isFrontFacing;
        }
        else AskPermissionToCamera();
    
    }

    public void StopCamera()
    {
        if (tex != null)
        {
            display.texture = null;
            tex.Stop();
            tex = null;
            camAvailiable = false;
        }
    }

    bool HasPermission(string permission)
    {
        if (Application.isEditor) return true;
#if UNITY_IOS
        return (Permission.HasUserAuthorizedPermission(permission));
#endif
#if UNITY_ANDROID
        return Permission.HasUserAuthorizedPermission(permission);
#endif
    }

    void AskPermissionToCamera()
    {
#if UNITY_IOS
        
#endif
#if UNITY_ANDROID
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += StartCameraCallback;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
#endif
    }


    void AskPermissionToStorage()
    {
#if UNITY_IOS
        
#endif
#if UNITY_ANDROID
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += SavePictureCallback;
        Permission.RequestUserPermission(Permission.ExternalStorageWrite, callbacks);
#endif
    }

    void SavePictureCallback(string callbackText = "")
    {
        SavePicture();
    }

    public void SavePicture()
    {
        if (!camAvailiable)
        {
            Debug.LogError("Cant capture no camera availiable");
            return;
        }
        if (HasPermission(Permission.ExternalStorageWrite))
        {
            string path = GetCurrentModelPicturePath(picture_count);
            Texture2D texture = display.texture as Texture2D;
            byte[] bytes = texture.EncodeToJPG();
            System.IO.File.WriteAllBytes(path, bytes);
            picture_count++;
            Debug.Log("Image captures and saved at " + path + name + ".jpg");
            DisplayPicture();
        }
        else
        {
            AskPermissionToStorage();
        }
    }

    string GetCurrentModelPicturePath(int number)
    {
        return Path.Combine(Application.persistentDataPath, MasterManager.Instance.modelController.currentModel.modelName + "_" + number + ".jpg");
    }

    public void DisplayPicture()
    {
        string path = GetCurrentModelPicturePath(0);
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2); 
            texture.LoadImage(fileData); 

            finishedPicture.texture = texture;
        }
        else
        {
            Debug.LogError("Image file not found at: " + path);
        }
    }
}
