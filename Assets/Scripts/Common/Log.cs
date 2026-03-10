using UnityEngine;
using System.Runtime.CompilerServices;
using System;


#if UNITY_EDITOR
using UnityEditor;

namespace NetCommon
{
    public static class Log
    {
        public static void Trace(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNum = 0)
        {
            // 파일 경로에서 파일명만 추출
            string file_name = System.IO.Path.GetFileName(filePath);

            Debug.Log($"[{file_name} > {memberName} (Line: {lineNum})] {message}");
        }
    }
}

#endif