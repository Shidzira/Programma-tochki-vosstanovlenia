using System;
using System.Runtime.InteropServices;

public class SystemRestorePointCreator
{
    [DllImport("SrClient.dll")]
    private static extern int SRSetRestorePointW(ref RestorePointInfo restorePointInfo, out uint restorePointId);

    [DllImport("Kernel32.dll")]
    private static extern int GetLastError();

    private const int STATEMENT_TYPE_ALL = 0x001F;
    private const int ERROR_SUCCESS = 0;

    private struct RestorePointInfo
    {
        public int dwEventType;
        public int dwRestorePtType;
        public int dwDescription;
        public long llSequenceNumber;
        public string szDescription;
    }

    public static bool CreateRestorePoint(string description)
    {
        RestorePointInfo restorePointInfo = new RestorePointInfo();
        restorePointInfo.dwEventType = STATEMENT_TYPE_ALL;
        restorePointInfo.dwRestorePtType = 0; // 0 means automatic restore point
        restorePointInfo.dwDescription = 0;
        restorePointInfo.llSequenceNumber = 0;
        restorePointInfo.szDescription = description;

        uint restorePointId = 0;
        int result = SRSetRestorePointW(ref restorePointInfo, out restorePointId);
        if (result == ERROR_SUCCESS)
        {
            Console.WriteLine("Точка восстановления успешно создана.");
            Console.WriteLine("Идентификатор точки восстановления: " + restorePointId);
            return true;
        }
        else
        {
            Console.WriteLine("Ошибка при создании точки восстановления: " + GetLastError());
            return false;
        }
    }

    static void Main()
    {
        string description = "Точка восстановления перед установкой обновлений или программ";
        bool success = CreateRestorePoint(description);

        if (success)
        {
            // Ваш код для установки обновлений или программ
            Console.WriteLine("Установка обновлений или программ...");
        }

    }
}