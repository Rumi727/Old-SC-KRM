using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SCKRM.Resources;
using UnityEngine;

namespace SCKRM.Json
{
    public class JsonManager
    {
        /// <summary>
        /// JSON을 불러옵니다 (이걸 사용할려면, JSON 파일의 모든 키와 값의 타입이 T랑 같아야합니다)
        /// </summary>
        /// <param name="key">키</param>
        /// <param name="path">JSON 파일 경로</param>
        /// <param name="value">값</param>
        /// <param name="resourcePackPath">리소스팩에서 파일 불러오기</param>
        /// <returns></returns>
        public static bool JsonRead<T>(string key, string path, out T value, bool resourcePackPath = true)
        {
            if (resourcePackPath)
            {
                NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(key);
                string selectedNameSpace = nameSpaceAndPath.NameSpace;
                key = nameSpaceAndPath.Path;
                
                string allPath;
                for (int i = 0; i < ResourcesManager.ResourcePacks.Count; i++)
                {
                    ResourcePack resourcePack = ResourcesManager.ResourcePacks[i];
                    for (int j = 0; j < resourcePack.NameSpace.Length; j++)
                    {
                        string nameSpace = resourcePack.NameSpace[j];
                        if (nameSpace != selectedNameSpace)
                            continue;

                        allPath = Path.Combine(resourcePack.Path, path).Replace("\\", "/").Replace("%NameSpace%", nameSpace) + ".json";
                        if (File.Exists(allPath))
                        {
                            string json = File.ReadAllText(allPath);

                            Dictionary<string, T> jsonDic = JsonConvert.DeserializeObject<Dictionary<string, T>>(json);
                            if (jsonDic == null)
                                break;
                            else if (!jsonDic.ContainsKey(key))
                                break;

                            value = jsonDic[key];
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    string json = sr.ReadToEnd();
                    sr.Close();

                    Dictionary<string, T> jsonDic = JsonConvert.DeserializeObject<Dictionary<string, T>>(json);
                    if (jsonDic == null)
                    {
                        value = default(T);
                        return false;
                    }
                    else if (!jsonDic.ContainsKey(key))
                    {
                        value = default(T);
                        return false;
                    }

                    value = jsonDic[key];
                    return true;
                }
            }

            value = default(T);
            return false;
        }
    }

    public struct JVector2
    {
        public float x;
        public float y;

        public JVector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        public JVector2(float f) => x = y = f;
        public JVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 JVector3ToVector3(JVector2 jVector2) => new Vector2() { x = jVector2.x, y = jVector2.y };
    }

    public struct JVector3
    {
        public float x;
        public float y;
        public float z;

        public JVector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public JVector3(float f) => x = y = z = f;
        public JVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 JVector3ToVector3(JVector3 jVector3) => new Vector3() { x = jVector3.x, y = jVector3.y, z = jVector3.z };
    }
    public struct JVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public JVector4(Vector4 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            w = v.w;
        }

        public JVector4(float f) => x = y = z = w = f;
        public JVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector4 JVector4ToVector4(JVector4 jVector4) => new Vector4() { x = jVector4.x, y = jVector4.y, z = jVector4.z, w = jVector4.w };
    }

    public struct JRect
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public JRect(Rect v)
        {
            x = v.x;
            y = v.y;
            width = v.width;
            height = v.height;
        }

        public JRect(float f) => x = y = width = height = f;
        public JRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static Rect JRectToRect(JRect jRect) => new Rect() { x = jRect.x, y = jRect.y, width = jRect.width, height = jRect.height };
    }
}