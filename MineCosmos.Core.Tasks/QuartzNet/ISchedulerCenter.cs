﻿using MineCosmos.Core.Model;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MineCosmos.Core.Tasks
{
    /// <summary>
    /// 服务调度接口
    /// </summary>
    public interface ISchedulerCenter
    {

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StartScheduleAsync();
        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StopScheduleAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> AddScheduleJobAsync(TasksQz sysSchedule);
        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> StopScheduleJobAsync(TasksQz sysSchedule);
        /// <summary>
        /// 检测任务是否存在
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<bool> IsExistScheduleJobAsync(TasksQz sysSchedule);
        /// <summary>
        /// 暂停指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> PauseJob(TasksQz sysSchedule);
        /// <summary>
        /// 恢复一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> ResumeJob(TasksQz sysSchedule);

        /// <summary>
        /// 获取任务触发器状态
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<List<TaskInfoDto>> GetTaskStaus(TasksQz sysSchedule);
        /// <summary>
        /// 获取触发器标识
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTriggerState(string key);

        /// <summary>
        /// 立即执行 一个任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        Task<MessageModel<string>> ExecuteJobAsync(TasksQz tasksQz);

    }

}
