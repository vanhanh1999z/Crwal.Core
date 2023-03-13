using Crwal.Core.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crwal.Core.Model
{
    public abstract class BaseModel
    {
        /// <summary>
        /// Thời gian tạo mới bản ghi
        /// </summary>
        //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ss")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Thời gian cập nhật lần cuối
        /// </summary>
        //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ss")]
        public DateTime Modified { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class AccessAccount
    {
        public string LoginName { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FullIdName { get; set; }
        public bool IsSystemAdmin { get; set; }
     
        public AccessAccount()
        {
        }
        public AccessAccount(string name, string display)
        {
            LoginName = name;
            DisplayName = display;
        }
        public AccessAccount(string name)
        {
            LoginName = name;
            DisplayName = name;
        }

    }
    public class SysAdminAccount
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public List<string> Roles { get; set; }
        public string DomainName
        {
            get
            {
                return $"{Domain}|{Name}";
            }

        }
    }
   
    public class ServiceAccount
    {
        public string Name { get; set; }
        public string PasswordHashed { get; set; }
        [JsonIgnore]
        public string Password { get; protected set; }
        public ServiceAccount() { }
        public ServiceAccount(string name, string passwordHashed, string passParse)
        {
            Name = name;
            PasswordHashed = passwordHashed;
            DecryptPassword();
        }
        public ServiceAccount(string name, string passwordHashed)
        {
            Name = name;
            PasswordHashed = passwordHashed;
            DecryptPassword();
        }
       
        public void DecryptPassword()
        {
            if (string.IsNullOrEmpty(PasswordHashed))
                Password = "";
            else
                Password = StringCipher.Decrypt(PasswordHashed);
        }
        public void DecryptPassword(string key)
        {
            if (string.IsNullOrEmpty(PasswordHashed))
                Password = "";
            else
                Password = StringCipher.Decrypt(PasswordHashed, key);
        }
    }
    public class KeyValueData
    {
        public List<KeyValue> Data { get; set; } = new List<KeyValue>();
    }
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public KeyValue()
        {

        }
        public KeyValue(string key)
        {
            Key = key;
        }
        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    public class KeyValueInt
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public KeyValueInt()
        {

        }
        public KeyValueInt(string key)
        {
            Key = key;
        }
        public KeyValueInt(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
    public class HierarchyObject : KeyValue
    {
        public int Index { get; set; } = 0;
        public string Parent { get; set; }
        public byte Level { get; set; } = 1;
        public HierarchyObject() : base() { }
        public HierarchyObject(string key) : base(key) { }
        public HierarchyObject(string key, string value) : base(key, value)
        {
        }
    }
    public class FeatureObject : HierarchyObject
    {
        public bool AllowAssign { get; set; } = true;
        public string Descriptions { get; set; }
        public List<string> DependencyFeatures { get; set; }
        public FeatureObject() { }
        public FeatureObject(string key) : base(key) { }
        public FeatureObject(string key, string value) : base(key, value)
        {
        }
    }
    public class MenuObject : HierarchyObject
    {
        public string Icon { get; set; }
        public string Position { get; set; } = "main";
        public bool RequirePermission { get; set; } = true;
        //public string Title { get; set; }
        public string Endpoint { get; set; }
        public bool Expanded { get; set; } = true;
        public MenuObject()
        {

        }
        public MenuObject(string key) : base(key)
        {

        }
        public MenuObject(string key, string value) : base(key, value)
        {
        }
    }
    public class PermissionObject
    {
        public string Name { get; set; }
        public string Edp { get; set; }
        public PermissionObject(string name)
        {
            Name = name;
        }
        public PermissionObject(string name, string edp)
        {
            Name = name;
            Edp = edp;
        }
    }
}
