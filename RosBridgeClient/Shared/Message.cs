﻿/*
c Siemens AG, 2017-2018
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

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient
{
    public class Message
    {
        [JsonIgnore]
        public string RosMessageType
        {
            get { return MessageTypes.RosMessageType(GetType()); }
        }
    }

    public class GeometryTwist : Message
    {
        public GeometryVector3 linear;
        public GeometryVector3 angular;
        public GeometryTwist()
        {
            linear = new GeometryVector3();
            angular = new GeometryVector3();
        }
    }
    public class StandardString : Message
    {
        public string data;
        public StandardString()
        {
            data = "";
        }
    }

    public class GeometryAccel : Message
    {
        public GeometryVector3 linear;
        public GeometryVector3 angular;
        public GeometryAccel()
        {
            linear = new GeometryVector3();
            angular = new GeometryVector3();
        }
    }

    public class SensorJointStates : Message
    {
        public StandardHeader header;
        public string[] name;
        public float[] position;
        public float[] velocity;
        public float[] effort;
        public SensorJointStates()
        {
            header = new StandardHeader();
            name = new string[0];
            position = new float[0];
            velocity = new float[0];
            effort = new float[0];
        }
    }
    public class GeometryVector3 : Message
    {
        public float x;
        public float y;
        public float z;
        public GeometryVector3()
        {
            x = 0f;
            y = 0f;
            z = 0f;
        }
    }
    public class SensorJoy : Message
    {
        public StandardHeader header;
        public float[] axes;
        public int[] buttons;

        public SensorJoy()
        {
            header = new StandardHeader();
            axes = new float[0];
            buttons = new int[0];
        }
    }

    public class MoveBaseGoal
    {
        public GeometryPoseStamped target_pose;

        public MoveBaseGoal()
        {
            target_pose = new GeometryPoseStamped();
        }
    }

    public class MoveBaseActionGoal : Message
    {
        public StandardHeader header;
        // goal_id
        public MoveBaseGoal goal;

        public MoveBaseActionGoal()
        {
            header = new StandardHeader();
            // goal_id
            goal = new MoveBaseGoal();
        }

    }

    public class NavigationOdometry : Message
    {
        public StandardHeader header;
        public string child_frame_id;
        public GeometryPoseWithCovariance pose;
        public GeometryTwistWithCovariance twist;
        public NavigationOdometry()
        {
            header = new StandardHeader();
            child_frame_id = "";
            pose = new GeometryPoseWithCovariance();
            twist = new GeometryTwistWithCovariance();
        }
    }
    public class StandardHeader : Message
    {
        public int seq;
        public StandardTime stamp;
        public string frame_id;
        public StandardHeader()
        {
            seq = 0;
            stamp = new StandardTime();
            frame_id = "";
        }
    }

    public class GeometryPoseWithCovariance : Message
    {
        public GeometryPose pose;
        public float[] covariance;
        public GeometryPoseWithCovariance()
        {
            pose = new GeometryPose();
            covariance = new float[36];
        }
    }
    public class GeometryTwistWithCovariance : Message
    {
        public GeometryTwist twist;
        public float[] covariance;
        public GeometryTwistWithCovariance()
        {
            twist = new GeometryTwist();
            covariance = new float[36];
        }
    }

    public class GeometryPose : Message
    {
        public GeometryPoint position;
        public GeometryQuaternion orientation;
        public GeometryPose()
        {
            position = new GeometryPoint();
            orientation = new GeometryQuaternion();
        }
    }

    public class GeometryPoseStamped : Message
    {
        public StandardHeader header;
        public GeometryPose pose;
        public GeometryPoseStamped()
        {
            header = new StandardHeader();
            pose = new GeometryPose();
        }
    }

    public class GeometryPoint : Message
    {
        public float x;
        public float y;
        public float z;
        public GeometryPoint()
        {
            x = 0;
            y = 0;
            z = 0;
        }
    }
    public class GeometryQuaternion : Message
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public GeometryQuaternion()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 0;
        }
    }
    public class SensorPointCloud2 : Message
    {
        public StandardHeader header;
        public int height;
        public int width;
        public SensorPointField[] fields;
        public bool is_bigendian;
        public int point_step;
        public int row_step;

        public byte[] data;
        public bool is_dense;
        public SensorPointCloud2()
        {
            header = new StandardHeader();
            height = 0;
            width = 0;
            fields = new SensorPointField[0];
            is_bigendian = false;
            point_step = 0;
            row_step = 0;
            is_dense = false;
            data = new byte[0];
        }
    }
    public class SensorPointField : Message
    {
        public int datatype;
        public string name;
        public int offset;
        public int count;
        public SensorPointField()
        {
            datatype = 0;
            name = "";
            offset = 0;
            count = 0;
        }
    }
    public class SensorImage : Message
    {
        public StandardHeader header;
        public int height;
        public int width;
        public string encoding;
        public byte is_bigendian;
        public int step;
        public byte[] data;
        public SensorImage()
        {
            header = new StandardHeader();
            height = 0;
            width = 0;
            encoding = "undefined";
            is_bigendian = 0;
            step = 0;
            data = new byte[0];
        }
    }
    public class SensorCompressedImage : Message
    {
        public StandardHeader header;
        public string format;
        public byte[] data;
        public SensorCompressedImage()
        {
            header = new StandardHeader();
            format = "";
            data = new byte[0];
        }
    }

    public class StandardTime : Message
    {
        public int secs;
        public int nsecs;
        public StandardTime()
        {
            secs = 0;
            nsecs = 0;
        }
    }

    public class NavigationMapMetaData : Message
    {
        public StandardTime map_load_time;
        public float resolution;
        public uint width;
        public uint height;
        public GeometryPose origin;

        public NavigationMapMetaData()
        {
            map_load_time = null;
            resolution = 0;
            width = 0;
            height = 0;
            origin = new GeometryPose();
        }
    }

    public class NavigationOccupancyGrid : Message
    {
        public StandardHeader header;
        public NavigationMapMetaData info;
        public sbyte[] data;
        public NavigationOccupancyGrid()
        {
            header = new StandardHeader();
            info = new NavigationMapMetaData();
            data = null;
        }
    }

    public class GeometryTransform : Message
    {
        public GeometryVector3 translation;
        public GeometryQuaternion rotation;
        public GeometryTransform()
        {
            translation = new GeometryVector3();
            rotation = new GeometryQuaternion();
        }
    }

    public class GeometryTransformStamped : Message
    {
        public StandardHeader header;
        public string child_frame_id;
        public GeometryTransform transform;
        public GeometryTransformStamped()
        {
            header = new StandardHeader();
            child_frame_id = "";
            transform = new GeometryTransform();
        }
    }

    public class TF2TFMessage : Message
    {
        public GeometryTransformStamped[] transforms;

        public TF2TFMessage()
        {
            transforms = null;
        }
    }

    public class ColorRGBA : Message
    {
        //public const string RosMessageName = "std_msgs/ColorRGBA";
        public float r;
        public float g;
        public float b;
        public float a;
        public ColorRGBA()
        {
            r = 0;
            g = 0;
            b = 0;
            a = 0;
        }
    }

    public class Marker : Message
    {
        public StandardHeader header;
        public string ns;
        public int id;
        public int type;
        public int action;
        public GeometryPose pose;
        public GeometryVector3 scale;
        public ColorRGBA color;
        //duration lifetime ???
        public bool frame_locked;
        public GeometryPoint[] points;
        public ColorRGBA[] colors;
        public string text;
        public string mesh_resource;
        public bool mesh_use_embedded_materials;

        public Marker()
        {
            header = new StandardHeader();
            ns = "";
            id = 0;
            type = 0;
            action = 0;
            pose = new GeometryPose();
            scale = new GeometryVector3();
            color = new ColorRGBA();
            //duration lifetime ???
            frame_locked = false;
            points = new GeometryPoint[0];
            colors = new ColorRGBA[0];
            text = "";
            mesh_resource = "";
            mesh_use_embedded_materials = false;
        }
    }

    public class MarkerArray : Message
    {
        public Marker[] markers;

        public MarkerArray()
        {
            markers = new Marker[0];
        }

    }

    public class PathPlan : Message
    {
        public StandardHeader header;
        public GeometryPoseStamped[] poses;

        public PathPlan()
        {
            header = new StandardHeader();
            poses = new GeometryPoseStamped[0];
        }

    }

    public class ParamName : Message
    {
        public string name;
        public ParamName(string _name)
        {
            name = _name;
        }
    }

    public class SetParam : Message
    {
        public string name;
        public string value;
        public SetParam(string _name, string _value)
        {
            name = _name;
            value = _value;
        }
    }

    public class ParamValueString : Message
    {
        public string value;
    }

    public class ParamValueByte : Message
    {
        public byte[] value;
    }

    public class BoundingBoxes : Message
    {

        public StandardHeader header;
        public StandardHeader image_header;
        public BoundingBox[] bounding_boxes;

        public BoundingBoxes()
        {
            header = new StandardHeader();
            image_header = new StandardHeader();
            bounding_boxes = new BoundingBox[0];

        }

    }

    public class BoundingBox : Message
    {
        public string Class;
        public float probability;
        public int xmin;
        public int ymin;
        public int xmax;
        public int ymax;
        public BoundingBox()
        {
            Class = "";
            probability = 0f;
            xmin = 0;
            ymin = 0;
            xmax = 0;
            ymax = 0;
        }

    }
    public class int32 : Message
    {
        public int data;
        public int32()
        {
            data = 0;

        }
    }
}


