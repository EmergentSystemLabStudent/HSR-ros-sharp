using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System;
using RosSharp.RosBridgeClient;
using UnityEngine.XR.WSA.WebCam;

//Semantic Point Cloudを生成するスクリプト

#if !UNITY_EDITOR
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class Semantic_Point_Cloud_Generator : MonoBehaviour, IInputClickHandler
{
    private PhotoCapture photoCaptureObject = null;
    private bool isCapturing = false;
    private int Width;
    private int Height;
    private int CaptureCount;
    private int time = 0;
    private bool finished = true;
    private bool arrived = false;
    public int qualityLevel = 50;

    public AudioClip try_get_pcl;
    public AudioClip got_pcl;

    List<Vector3> PointAll = null;
    List<Color> ColorAll = null;

    private RosSocket rosSocket;
    private Subscribe_semantic_image semantic_result_topic;
    private string advertise_id;
    private List<byte> imageBufferList;
    private PhotoCaptureFrame memory;
    private AudioSource audioSource;
    private List<int> not_hitted_pixel;

    Texture2D segmented_texture;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        InputManager.Instance.AddGlobalListener(gameObject);
        PointAll = new List<Vector3>();
        ColorAll = new List<Color>();
        not_hitted_pixel = new List<int>();
        imageBufferList = new List<byte>();
        CaptureCount = 0;
        Invoke("Init", 1.0f);
    }

    private void Update()
    {
        if (arrived)
        {
            segmented_texture = new Texture2D(2048, 1152); //width,height
            segmented_texture.LoadImage(semantic_result_topic.segmented_image.data);


            List<Color> colorArray = new List<Color>();

            Color[] pixels = segmented_texture.GetPixels();
            Color temp = new Color();

            for (int y = 0; y < 1152; y++)
            {
                for (int x = 0; x < 1024; x++)
                {
                    temp = pixels[x + (y * 2048)];
                    pixels[x + (y * 2048)] = pixels[(2048 - x - 1) + (y * 2048)];
                    pixels[(2048 - x - 1) + (y * 2048)] = temp;
                }
            }

            colorArray = pixels.ToList();

            //メッシュデータを作成する
            Create_point_cloud(ref colorArray, memory);

            //保存する
            if (CaptureCount % 1 == 0)
            {
                SaveFile(PointAll, ColorAll);
            }

            photoCaptureObject.StopPhotoModeAsync(Stop_take_picture);


        }

    }

    public void Init()
    {
        GetComponent<Subscribe_semantic_image>().semantic_result += (sender, e) => semantic_result_arrived();
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        semantic_result_topic = GetComponent<Subscribe_semantic_image>();
        advertise_id = rosSocket.Advertise("/image_hololens", "sensor_msgs/CompressedImage");
        Debug.Log("ara:ROS Initation Finished");
    }

    //クリックされた時
    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        if (Time.frameCount - time < 15 & finished)
        {

            Debug.Log("ara:clicked");

            finished = false;

            if (!isCapturing)
            {
                audioSource.PlayOneShot(try_get_pcl);
                PointAll.Clear();
                ColorAll.Clear();
                not_hitted_pixel.Clear();

                if (photoCaptureObject != null)
                {
                    photoCaptureObject.Dispose();
                    photoCaptureObject = null;
                }
                PhotoCapture.CreateAsync(false, Start_take_picture);
                isCapturing = true;
            }
        }
        time = Time.frameCount;
    }

    void Start_take_picture(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        //ディスプレイの解像度を取得
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        //カメラパラメータを設定
        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        //画像の縦、横を取得
        Width = cameraResolution.width;
        Height = cameraResolution.height;

        //写真撮影
        photoCaptureObject.StartPhotoModeAsync(c, delegate (PhotoCapture.PhotoCaptureResult result) {
            photoCaptureObject.TakePhotoAsync(Publish_to_ROS);
        });
    }

    void Stop_take_picture(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        isCapturing = false;
    }

    void Publish_to_ROS(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {

            memory = photoCaptureFrame;

            //画像をコピーする
            photoCaptureFrame.CopyRawImageDataIntoBuffer(imageBufferList);

            byte[] ROS_data = new byte[imageBufferList.Count];

            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);

            ROS_data = targetTexture.EncodeToJPG(qualityLevel);

            GameObject.Destroy(targetTexture);

            Publish_to_ROS(ROS_data);
        }
    }

    void Publish_to_ROS(byte[] image_data)
    {
        SensorCompressedImage publish_image = new SensorCompressedImage();

        publish_image.format = "jpeg";
        publish_image.data = image_data;

        Debug.Log("ara:Publishing...");

        // ROSに画像を送信
        rosSocket.Publish(advertise_id, publish_image);

    }

    void semantic_result_arrived()
    {

        Debug.Log("ara:Event arrived!!!");

        arrived = true;

    }

    void Create_point_cloud(ref List<Color> colorArray, PhotoCaptureFrame photoCaptureFrame)
    {
        int texWidth = Width / 8;   
        int texHeight = Height / 8;
        List<Vector3> points = new List<Vector3>(); //ピックアップしたピクセル
        List<int> indecies = new List<int>(); //インデックス行列
        List<Color> colors = new List<Color>(); //そのピクセルのRGBA情報

        for (int y = 0; y < texHeight; ++y)
        {
            for (int x = 0; x < texWidth; ++x)
            {
                int i = y * texWidth + x;
                int i2 = y * 8 * Width + x * 8;

                points.Add(new Vector3(((x - 1) - (texWidth / 2)) / 200.0F,
                                        (y - (texHeight / 2)) / 200.0F, 0.0F));
                indecies.Add(i);
                colors.Add(new Color(colorArray[i2].r, colorArray[i2].g, colorArray[i2].b, colorArray[i2].a));
            }
        }

        //カメラ座標からワールド座標
        Matrix4x4 cameraToWorldMatrix;
        photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);

        //World座標からカメラ座標
        Matrix4x4 worldToCameraMatrix = cameraToWorldMatrix.inverse;

        //カメラの座標
        Matrix4x4 projectionMatrix;
        photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

        // UnityとHoloLensの座標系が違うためここで場合分け
#if UNITY_EDITOR
        Vector3 position = cameraToWorldMatrix.GetColumn(3) + cameraToWorldMatrix.GetColumn(2);
#else
        Vector3 position = cameraToWorldMatrix.GetColumn(3) - cameraToWorldMatrix.GetColumn(2);
#endif

        //ユーザー側にキャンバスを向かせる
        Quaternion rotation = Quaternion.LookRotation(cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));


        //ピクセルからベクトルの生成

        for (int y = texHeight - 1; y >= 0; --y)
        {
            for (int x = texWidth - 1; x >= 0; --x)
            {
                int i = y * texWidth + x;
#if false
                Vector3 point = points[i];
#else
                Vector3 translation = position;
                Vector3 scale = new Vector3(1.0F, 1.0F, 1.0F);
                Matrix4x4 m = Matrix4x4.identity;

                m.SetTRS(translation, rotation, scale);
                Vector3 point = m.MultiplyPoint3x4(points[i]);
#endif
                //カメラの位置とピクセル間でのベクトルの生成
                Vector3 front = new Vector3(point.x - cameraToWorldMatrix.GetColumn(3).x,
                                            point.y - cameraToWorldMatrix.GetColumn(3).y,
                                            point.z - cameraToWorldMatrix.GetColumn(3).z);

                RaycastHit hit;

                if (Physics.Raycast(cameraToWorldMatrix.GetColumn(3), front, out hit, 50.0F))
                {
                    points[i] = hit.point; //さっきまでDepthは偽の情報を入れていたが、それを正しい値に更新
                }
                else
                {
                    //Hitする対象がなかった場合,そのピクセルは無視する
                    points.RemoveAt(i);
                    indecies.RemoveAt(i);
                    colors.RemoveAt(i);

                    //Hitしなかったピクセルを保存しておく
                    not_hitted_pixel.Add(i);
                }
            }
        }

        // Indexを付けなおす
        for (int i = 0; i < indecies.Count; ++i)
        {
            indecies[i] = i;
        }

        //ROSの座標系に変換する
        for(int i = 0; i < indecies.Count; ++i)
        {
            points[i] = Unity2Ros(points[i]);
        }

        // ファイル保存のためにコピーを作成する
        for (int i = 0; i < indecies.Count; ++i)
        {
            PointAll.Add(points[i]);
            ColorAll.Add(colors[i]);
        }
        CaptureCount++;
    }

    public static Vector3 Unity2Ros(Vector3 vector3)
    {
        return new Vector3(vector3.z, -vector3.x, vector3.y);
    }

    void SaveFile(List<Vector3> PointAll, List<Color> ColorAll)
    {
        string fileName = String.Format("pcd{0}.txt", CaptureCount);
        string fileName2 = String.Format("missed{0}.txt", CaptureCount);


#if !UNITY_EDITOR
        Task task = new Task(
                        async () =>
                        {
                            // Create sample file; replace if exists.
                            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                            StorageFile offFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);

                            List<string> PointColorList = new List<string>();
                            PointColorList.Add("COFF");
                            PointColorList.Add(String.Format("{0} {1} {2}\n", PointAll.Count, 0, 0));
                            for (int i = 0; i < PointAll.Count; ++i)
                            {
                                PointColorList.Add(String.Format(" {0} {1} {2} {3} {4} {5} {6}\n",
                                                                  PointAll[i].x, PointAll[i].y, PointAll[i].z,
                                                                  ColorAll[i].r, ColorAll[i].g, ColorAll[i].b, ColorAll[i].a));
                            }
                            IEnumerable<string> IPointColorList = PointColorList;
                            await FileIO.WriteLinesAsync(offFile, IPointColorList);
                        });

        Task task2 = new Task(
                         async () =>
                         {
                            StorageFolder storageFolder2 = ApplicationData.Current.LocalFolder;
                            StorageFile offFile2 = await storageFolder2.CreateFileAsync(fileName2, CreationCollisionOption.GenerateUniqueName);

                            List<string> miss_List = new List<string>();
                            foreach(int i in not_hitted_pixel)
                            {
                                miss_List.Add(String.Format(" {0} \n",i));
                            }
                            IEnumerable<string> Imiss_List = miss_List;
                            await FileIO.WriteLinesAsync(offFile2, Imiss_List);        
                        });

        task.Start();
        task.Wait();
        task2.Start();
        task2.Wait();

#endif
        audioSource.PlayOneShot(got_pcl);
        finished = true;
        arrived = false;
    }
}
