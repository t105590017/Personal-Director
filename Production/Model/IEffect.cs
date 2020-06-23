using System;
using System.Collections.Generic;
using System.Text;

namespace Production.Model
{
    public interface IEffect
    {
        /// <summary>
        /// 輸出路徑
        /// </summary>
        string OutputPath { get; }
        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="guid">暫存guid</param>
        /// <param name="path">資料來源</param>
        void SetDataSource(Guid guid, string path);

        /// <summary>
        /// 取得所有參數
        /// </summary>
        /// <returns></returns>
        List<object> GetParameters();
        /// <summary>
        /// 執行
        /// </summary>
        /// 
        void Excute();
    }
}
