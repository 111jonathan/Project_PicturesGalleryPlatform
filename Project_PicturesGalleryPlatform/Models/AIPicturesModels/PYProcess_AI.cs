using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace Project_PicturesGalleryPlatform.Models.AIPicturesModels
{
    public class PYProcess_AI
    {
        // Python程式路徑
        private readonly string pythonExePath = @".\Models\AIPicturesModels\main_generate.exe";// 路徑要改
        // 要傳遞給Python的參數
        public string arguments;//ex: "可愛動物 1"
        // 創建一個進程啟動信息對象
        ProcessStartInfo startInfo;
        /// <summary>
        /// 建構式: 生成圖片關鍵字+一次生成數量
        /// </summary>
        /// <param name="keyword">生成圖片關鍵字</param>
        /// <param name="number">一次生成數量</param>
        public PYProcess_AI(string keyword, int number)
        {
            this.arguments = $"{keyword} {number}";
            startInfo = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = this.arguments,
                RedirectStandardOutput = true, // 重定向標準輸出
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        /// <summary>
        /// 執行python生成圖片，整理生成的圖片數據
        /// </summary>
        /// <returns></returns>
        public List<AIPicData> Generate1()
        {
            try
            {
                // 創建一個空的AIPicData列表，儲有生成的圖片數據
                List<AIPicData> PicDataList = new();
                // 啟動進程
                using (Process process = Process.Start(startInfo))
                {
                    // 讀取標準輸出
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        //Console.WriteLine("結果: {0}", result);
                        // 將JSON字符串轉換為JSON對象
                        var result_Json = JsonSerializer.Deserialize<JsonElement>(result);
                        foreach (var obj in result_Json.EnumerateArray())
                        {
                            AIPicData tempData = new ();
                            if (!obj.TryGetProperty("id", out JsonElement value1) ||
                                !obj.TryGetProperty("output_url", out JsonElement value2) ||
                                !obj.TryGetProperty("share_url", out JsonElement value3) ||
                                !obj.TryGetProperty("backend_request_id", out JsonElement value4)
                            )// .TryGetProperty 失敗
                            {
                                Console.WriteLine(".TryGetProperty 失敗");
                                return null;
                            }
                            else // .TryGetProperty 成功
                            {
                                tempData.id = value1.GetString();
                                tempData.output_url = value2.GetString();
                                tempData.share_url = value3.GetString();
                                tempData.backend_request_id = value4.GetString();
                                PicDataList.Add(tempData);
                            }
                        }
                    }
                    return PicDataList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"執行Python可執行文件時出錯: {e.Message}");
                return null;
            }
        }


        public JsonElement? Generate()
        {
            // test
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine("當前PYProcess_Generate()工作目錄: " + currentDirectory);
            try
            {
                // 啟動進程
                using (Process process = Process.Start(startInfo))
                {
                    // 讀取標準輸出
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        //Console.WriteLine("結果: {0}", result);
                        // 將JSON字符串轉換為JSON對象
                        var result_Json = JsonSerializer.Deserialize<JsonElement>(result);
                        return result_Json;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"執行Python可執行文件時出錯: {e.Message}");
                return null;
            }
        }
    }
}
