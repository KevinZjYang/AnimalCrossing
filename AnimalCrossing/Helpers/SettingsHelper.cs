using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Helpers
{
    public static class SettingsHelper
    {
        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private static Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        /// <summary>
        /// 获取本地设置-存在返回值-不存在创建值，设置为null
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetLocalSetting(string key)
        {
            if (localSettings.Values.ContainsKey(key))
            {
                return localSettings.Values[key].ToString();
            }
            else
            {
                localSettings.Values[key] = null;
                return null;
            }
        }

        /// <summary>
        /// 获取本地设置-存在返回值-不存在创建并保存默认值
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetLocalSetting(string key, string defaultValue)
        {
            if (localSettings.Values.ContainsKey(key))
            {
                return localSettings.Values[key].ToString();
            }
            else
            {
                localSettings.Values[key] = defaultValue;
                return defaultValue;
            }
        }

        /// <summary>
        /// 保存本地设置
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SaveLocalSetting(string key, object value)
        {
            localSettings.Values[key] = value;
        }

        /// <summary>
        /// 同时保存本地和漫游设置
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SaveLocalAndRoamingSetting(string key, object value)
        {
            localSettings.Values[key] = value;
            roamingSettings.Values[key] = value;
        }

        /// <summary>
        /// 保存漫游设置
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SaveRoamingSetting(string key, object value)
        {
            roamingSettings.Values[key] = value;
        }

        /// <summary>
        /// 获取漫游设置-存在返回值-不存在创建并保存默认值
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRoamingSetting(string key, string defaultValue)
        {
            if (roamingSettings.Values.ContainsKey(key))
            {
                return roamingSettings.Values[key].ToString();
            }
            else
            {
                roamingSettings.Values[key] = defaultValue;
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取漫游设置-存在返回值-不存在创建值，设置值为null
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRoamingSetting(string key)
        {
            if (roamingSettings.Values.ContainsKey(key))
            {
                return roamingSettings.Values[key].ToString();
            }
            else
            {
                roamingSettings.Values[key] = null;
                return null;
            }
        }

        /// <summary>
        /// 1、先获取本地值，不存在的情况下获取漫游值 2、漫游值存在，获取并保存到本地 3、漫游值不存在，设置默认值，并保存到本地
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        public static string GetSettingFromLocalOrRoaming(string key, string defaultValue)
        {
            var str = GetLocalSetting(key);
            if (str == null)
            {
                str = GetRoamingSetting(key, defaultValue);
                SaveLocalSetting(key, str);
                return str;
            }
            else
            {
                return str;
            }
        }
    }
}
