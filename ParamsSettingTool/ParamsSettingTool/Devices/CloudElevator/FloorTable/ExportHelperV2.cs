using Aspose.Cells;
using ITL.ParamsSettingTool.SettingCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool.ParamsSettingTool.Devices.CloudElevator
{
    public class ExportHelperV2
    {
        /// <summary>
        /// 导出数据到本地
        /// </summary>
        /// <param name="dt">要导出的数据</param>
        /// <param name="tableName">表格标题</param>
        /// <param name="path">保存路径</param>
        public static void OutFileToDisk(List<FloorRelationUIObject> floorRelationUIObjectList, string path)
        {
            //License l = new License();
            //l.SetLicense("Aid/License.lic");
            Workbook workbook = new Workbook(); //工作簿
            Worksheet sheet = workbook.Worksheets[0]; //工作表
            sheet.Name = "楼层对应表";
            Cells cells = sheet.Cells;//单元格
            int Colnum = 9;//表格列数
            int Rownum = floorRelationUIObjectList.Count;//表格行数
            cells.SetRowHeight(0, 25);
            cells[0, 0].PutValue("设备ID");
            cells[0, 1].PutValue("权限标识");
            cells[0, 2].PutValue("按键名称");
            cells[0, 3].PutValue("实际楼层");
            cells[0, 4].PutValue("检测楼层");
            cells[0, 5].PutValue("端子号");
            cells[0, 6].PutValue("第一副操纵盘");
            cells[0, 7].PutValue("第二副操纵盘");
            cells[0, 8].PutValue("对讲端子号");

            int i = 1;
            foreach(var item in floorRelationUIObjectList)
            {
                cells.SetColumnWidth(i, 100);
                cells[i, 0].PutValue(item.DeviceId);
                cells[i, 1].PutValue(item.FloorNo);
                cells[i, 2].PutValue(item.FloorName.Contains("-") ? "" : item.FloorName);
                cells[i, 3].PutValue(item.FloorReal.Contains("-") ? "" : item.FloorReal);
                cells[i, 4].PutValue(item.CheckFloor.Contains("-") ? "" : item.CheckFloor);
                cells[i, 5].PutValue(item.FloorTerminalNo.Contains("-") ? "" : item.FloorTerminalNo);
                cells[i, 6].PutValue(item.TerminalNumSlave1.Contains("-") ? "" : item.TerminalNumSlave1);
                cells[i, 7].PutValue(item.TerminalNumSlave2.Contains("-") ? "" : item.TerminalNumSlave2);
                cells[i, 8].PutValue(item.TerminalNumIntercom.Contains("-") ? "" : item.TerminalNumIntercom);
                i++;
            }
            workbook.Save(path);
        }
    }
}
