﻿using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MongoUtility.Core
{
    public class MongoConfig
    {
        /// <summary>
        ///     配置文件名称
        /// </summary>
        public static string MongoConfigFilename = "MongoConfig.xml";

        /// <summary>
        ///     AppPath
        /// </summary>
        public static string AppPath = string.Empty;

        /// <summary>
        ///     连接配置列表(管理用）
        /// </summary>
        [XmlIgnore] public Dictionary<string, MongoConnectionConfig> ConnectionList =
            new Dictionary<string, MongoConnectionConfig>();

        /// <summary>
        ///     ReadPreference
        /// </summary>
        public string ReadPreference;

        /// <summary>
        ///     连接配置列表(保存用）
        /// </summary>
        public List<MongoConnectionConfig> SerializableConnectionList = new List<MongoConnectionConfig>();

        /// <summary>
        ///     WaitQueueSize;
        /// </summary>
        /// <remarks></remarks>
        public int WaitQueueSize;

        /// <summary>
        ///     WriteConcern
        /// </summary>
        public string WriteConcern;

        /// <summary>
        ///     wtimeoutMS
        /// </summary>
        /// <remarks>The driver adds { wtimeout : ms } to the getlasterror command. Implies safe=true.</remarks>
        public double WtimeoutMs;

        /// <summary>
        ///     添加链接
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static void AddConnection(MongoConnectionConfig con)
        {
            MongoConnectionConfig.MongoConfig.ConnectionList.Add(con.ConnectionName, con);
        }

        /// <summary>
        ///     写入配置
        /// </summary>
        public void SaveMongoConfig()
        {
            MongoConnectionConfig.MongoConfig.SerializableConnectionList.Clear();
            foreach (var item in MongoConnectionConfig.MongoConfig.ConnectionList.Values)
            {
                MongoConnectionConfig.MongoConfig.SerializableConnectionList.Add(item);
            }
            //Utility.SaveObjAsXml(AppPath + MongoConfigFilename, this);
        }

        /// <summary>
        /// </summary>
        public static void LoadFromConfigFile()
        {
            MongoConnectionConfig.MongoConfig = LoadObjFromXml<MongoConfig>(AppPath + MongoConfigFilename);
            MongoConnectionConfig.MongoConfig.ConnectionList.Clear();
            foreach (var item in MongoConnectionConfig.MongoConfig.SerializableConnectionList)
            {
                MongoConnectionConfig.MongoConfig.ConnectionList.Add(item.ConnectionName, item);
            }
        }

        /// <summary>
        ///     读取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T LoadObjFromXml<T>(string filename)
        {
            var setting = new XmlReaderSettings();
            var xml = new XmlSerializer(typeof(T));
            var reader = XmlReader.Create(filename, setting);
            var obj = (T)xml.Deserialize(reader);
            return obj;
        }
    }
}