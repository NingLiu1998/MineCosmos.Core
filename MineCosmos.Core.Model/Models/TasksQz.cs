using MineCosmos.Core.Model.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 任务计划表
    /// </summary>
    public class TasksQz : RootEntityTkey<int>
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string ClassName { get; set; }
       
        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        /// </summary>
        public int TriggerType { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 循环执行次数
        /// </summary>
        public int CycleRunTimes { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 执行传参
        /// </summary>
        public string JobParams { get; set; }

        /// <summary>
        /// 任务内存中的状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TaskInfoDto> Triggers { get; set; }
    }
}
