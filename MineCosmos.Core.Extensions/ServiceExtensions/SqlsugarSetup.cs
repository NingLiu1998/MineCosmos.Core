using MineCosmos.Core.Common;
using MineCosmos.Core.Common.DB;
using MineCosmos.Core.Common.Helper;
using MineCosmos.Core.Common.LogHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MineCosmos.Core.Model;

namespace MineCosmos.Core.Extensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlsugarSetup
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 默认添加主数据库连接
            MainDb.CurrentDbConnId = AppSettings.app(new string[] { "MainDB" });

            // SqlSugarScope是线程安全，可使用单例注入
            // 参考：https://www.donet5.com/Home/Doc?typeId=1181
            services.AddSingleton<ISqlSugarClient>(o =>
            {
                var memoryCache = o.GetRequiredService<IMemoryCache>();

                // 连接字符串
                var listConfig = new List<ConnectionConfig>();
                // 从库
                var listConfig_Slave = new List<SlaveConnectionConfig>();
                BaseDBConfig.MutiConnectionString.slaveDbs.ForEach(s =>
                {
                    listConfig_Slave.Add(new SlaveConnectionConfig()
                    {
                        HitRate = s.HitRate,
                        ConnectionString = s.Connection
                    });
                });

                BaseDBConfig.MutiConnectionString.allDbs.ForEach(m =>
                {
                    listConfig.Add(new ConnectionConfig()
                    {
                        ConfigId = m.ConnId.ObjToString().ToLower(),                        
                        ConnectionString = m.Connection,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,
                        //IsShardSameThread = false,
                        AopEvents = new AopEvents
                        {
                            OnLogExecuting = (sql, p) =>
                            {
                                if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                                {
                                    if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToFile", "Enabled" }).ObjToBool())
                                    {
                                        Parallel.For(0, 1, e =>
                                        {
                                            MiniProfiler.Current.CustomTiming("SQL：", GetParas(p) + "【SQL语句】：" + sql);
                                            //LogLock.OutSql2Log("SqlLog", new string[] { GetParas(p), "【SQL语句】：" + sql });
                                            LogLock.OutLogAOP("SqlLog", "", new string[] { sql.GetType().ToString(), GetParas(p), "【SQL语句】：" + sql });

                                        });
                                    }
                                    if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToConsole", "Enabled" }).ObjToBool())
                                    {
                                        ConsoleHelper.WriteColorLine(string.Join("\r\n", new string[] { "--------", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                    }
                                }
                            },
                            OnError = (ex) =>
                            {
                                //打印日志
                                    if (ex.Parametres == null) return;
                                    Console.ForegroundColor = ConsoleColor.Red;

                                //UtilMethods.GetNativeSql(ex.Sql, ex.Parametres);
                                    //var pars = m.db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                                    //WriteSqlLog($"{m.ConnId}库操作异常");
                                    //Console.WriteLine(UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                
                            },
                            //插入和更新过滤器
                            DataExecuting = (oldValue, entityInfo) =>
                            {
                                // 新增操作
                                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                                {
                                    
                                    if (entityInfo.PropertyName == nameof(RootEntityTkey<int>.CreateTime))
                                        entityInfo.SetValue(DateTime.Now);

                                    if (entityInfo.PropertyName == nameof(RootEntityTkey<int>.IsDeleted))
                                    {
                                       var val = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                                        if (val is null)
                                        {
                                            entityInfo.SetValue(false);
                                        }                                       
                                        
                                    }
                                     

                                    //手机号和密码自动加密
                                    //if (entityInfo.EntityName == nameof(SysUser) && (entityInfo.PropertyName == nameof(SysUser.Password) || entityInfo.PropertyName == nameof(SysUser.Phone)))
                                    //    entityInfo.SetValue(CryptogramUtil.Sm4Encrypt(oldValue?.ToString()));

                                    //获取当前登录token

                                    //if (App.User != null)
                                    //{
                                    //    //创建人和创建机构ID
                                    //    if (entityInfo.PropertyName == nameof(BaseEntity.CreateUserId))
                                    //        entityInfo.SetValue(App.User.FindFirst(ClaimConst.UserId)?.Value);
                                    //    if (entityInfo.PropertyName == nameof(BaseEntity.CreateUser))
                                    //        entityInfo.SetValue(App.User?.FindFirst(ClaimConst.Account)?.Value);
                                    //    if (entityInfo.PropertyName == nameof(DataEntityBase.CreateOrgId))
                                    //        entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgId)?.Value);
                                    //}
                                }
                                // 更新操作
                                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                                {
                                    
                                    //更新时间
                                    if (entityInfo.PropertyName == nameof(RootEntityTkey<int>.ModifyTime))
                                        entityInfo.SetValue(DateTime.Now);
                                    //获取登录jwt token 信息

                                    ////更新人
                                    //if (App.User != null)
                                    //{
                                    //    if (entityInfo.PropertyName == nameof(BaseEntity.UpdateUserId))
                                    //        entityInfo.SetValue(App.User?.FindFirst(ClaimConst.UserId)?.Value);
                                    //    if (entityInfo.PropertyName == nameof(BaseEntity.UpdateUser))
                                    //        entityInfo.SetValue(App.User?.FindFirst(ClaimConst.Account)?.Value);
                                    //}

                                }
                            }

                },
                        MoreSettings = new ConnMoreSettings()
                        {
                            //IsWithNoLockQuery = true,
                            IsAutoRemoveDataCache = true
                        },
                        // 从库
                        SlaveConnectionConfigs = listConfig_Slave,
                        // 自定义特性
                        ConfigureExternalServices = new ConfigureExternalServices()
                        {

                            DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache),                            
                            EntityService = (property, column) =>
                            {
                                if (column.IsPrimarykey && property.PropertyType == typeof(int))
                                {
                                    column.IsIdentity = true;
                                }
                            }
                        },
                        InitKeyType = InitKeyType.Attribute,
                        
                    }
                   );
                });
                return new SqlSugarScope(listConfig);
            });
        }

        private static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql.Replace(param.ParameterName, param.Value.ObjToString());
            }

            return sql;
        }

        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }
    }
}