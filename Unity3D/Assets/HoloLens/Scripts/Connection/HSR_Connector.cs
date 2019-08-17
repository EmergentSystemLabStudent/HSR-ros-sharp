/*
© Siemens AG, 2017
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
 * クラス名 : HSR_Connector
 * 説明 ; ROSとの通信を司るクラス.元々あるROS_Connectorクラスを改造して作った
 */

using RosSharp.RosBridgeClient;
using UnityEngine;

public class HSR_Connector : MonoBehaviour
{

    public RosSocket RosSocket { get; private set; }
    public string RosBridgeServerUrl = "192.168.0.1";
    private GameObject IP_info;

    public void Awake()
    {
        IP_info = GameObject.FindGameObjectsWithTag("IP")[0];
        RosBridgeServerUrl = "ws://" + IP_info.GetComponent<IP>().HSR_IP + ":9090";
    }

    public void Connect()
    {
        RosSocket = new RosSocket(RosBridgeServerUrl);
        Debug.Log("Connected to RosBridge: " + RosBridgeServerUrl);
    }

    public void Disconnect()
    {
        RosSocket.Close();
        Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
    }

    private void OnApplicationQuit()
    {
        RosSocket.Close();
        Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
    }
}
