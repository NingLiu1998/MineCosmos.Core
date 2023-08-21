using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpNBT;

namespace pack包加密研究
{
    public class nbtHelper
    {

        static string NBTID => "id";
        static string NBTCOUNT => "Count";
        static string NBTLVL => "lvl";
        static string NBTDAMAGE => "Damage";
        static string NBTTAG => "tag";
        static string NBTEnchantments => "Enchantments";

        public static Dictionary<string, object> TagToDic(CompoundTag tags, Dictionary<string, object>? dic)
        {
            if (dic is null)
                dic = new();
            //   dic.TryAdd(tagItem.Name, TagHandle(tagItem, dic));
            foreach (Tag tagItem in tags)
            {

                //dic.TryAdd(tagItem.Name, TagHandle(tagItem, dic));

                switch (tagItem)
                {
                    case StringTag stringTag when stringTag.Name.Equals("id"):
                        dic.TryAdd(stringTag.Name, stringTag.Value);
                        break;

                    case ByteTag byteTag when byteTag.Name.Equals("Count"):
                        dic.TryAdd(byteTag.Name, (int)byteTag.Value);
                        break;

                    case CompoundTag tagTag when tagTag.Name.Equals("tag"):
                        dic.TryAdd(tagTag.Name, TagToDic(tagTag, null));
                        break;

                    case ListTag tagList when tagList.Name.Equals("Enchantments"):
                        var lst = new List<Dictionary<string, object>>();
                        foreach (Tag tag in tagList)
                        {
                            var tagListDic = new Dictionary<string, object>();
                            tagListDic = TagToDic((CompoundTag)tag, tagListDic);
                            lst.Add(tagListDic);
                        }
                        dic.TryAdd(tagItem.Name, lst);
                        //dic.TryAdd(tagItem.Name, ListTagHandle(tagList));
                        break;

                    case ShortTag shortTag when shortTag.Name.Equals("lvl"):
                        dic.TryAdd(shortTag.Name, (int)shortTag.Value);
                        break;
                    case IntTag intTag when intTag.Name.Equals("Damage"):
                        dic.TryAdd(intTag.Name, intTag.Value);
                        break;
                }
            }
            return dic;
        }










        public static CompoundTag DicToTag(Dictionary<string, object> dic, string name = "",TagBuilder? builder = null)
        {
            if(builder is null)
             builder = new TagBuilder(name);
            foreach (var dicItem in dic)
            {
                switch (dicItem.Value)
                {
                    //对应long 也就是mc中的byte  b
                    case Int64 intVal when dicItem.Key.Equals(NBTCOUNT):
                        builder.AddByte("Count", Convert.ToByte(dicItem.Value));
                        break;

                    case string intVal when dicItem.Key.Equals(NBTID):
                        builder.AddString(NBTID, dicItem.Key.ToString());
                        break;

                    case Int64 intVal when dicItem.Key.Equals(NBTLVL):
                        builder.AddShort(NBTLVL, Convert.ToInt16(dicItem.Value));
                        break;

                    case Int64 intVal when dicItem.Key.Equals(NBTDAMAGE):
                        builder.AddInt(NBTDAMAGE, Convert.ToInt32(dicItem.Value));
                        break;

                    // List<Dictionary<string, object>>
                    case Newtonsoft.Json.Linq.JObject dicJobject when dicItem.Key.Equals(NBTTAG):
                        //Newtonsoft.Json.Linq.JArray
                        //var tagBuilder = new TagBuilder(NBTTAG);
                        Dictionary<string, object> dictionary = dicJobject.ToObject<Dictionary<string, object>>();
                        var dicJobjectTag = DicToTag(dictionary, NBTTAG);
                        //tagBuilder.AddTag(dicJobjectTag);
                        builder.AddTag(dicJobjectTag);
                        //builder.AddTag( tagBuilder.Create());

                        break;

                    case Newtonsoft.Json.Linq.JArray dicJArry when dicItem.Key.Equals(NBTEnchantments):

                        List<Dictionary<string, object>> list = dicJArry.ToObject<List<Dictionary<string, object>>>();
                        using (builder.NewList(TagType.Compound, dicItem.Key))
                        {
                            foreach (var dicJArryItem in list)
                            {
                                CompoundTag? dicJarrTag = DicToTag(dicJArryItem,"123");
                               builder.AddTag(dicJarrTag);
                            }
                        }
                        break;
                }
            }
            return builder.Create();
        }


    }

}
