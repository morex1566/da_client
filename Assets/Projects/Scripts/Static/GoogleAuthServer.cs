using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems; // UIBehaviour를 위해 필요합니다.
using UnityEngine.LightTransport;
using UnityEngine.Networking;

// UIBehaviour를 상속받고, IAuthService를 구현합니다.
public class GoogleAuthServicer : UIBehaviour
{
    // --- [설정 및 기존 로직은 동일] ---
    private string clientId = "1037531147924-tomb4bmakelqcuht2igfb9eomegacsso.apps.googleusercontent.com";
    private string clientSecret = "YOUR_CLIENT_SECRET_HERE";
    private int authConsolePort = 5000;

    private string redirectUri => $"http://localhost:{authConsolePort}/";



    public void AsyncLogin()
    {
        HttpListener listener = new HttpListener();
        {
            listener.Prefixes.Add(redirectUri);
            listener.Start();
        }

        string encodedRedirectUri = UnityWebRequest.EscapeURL(redirectUri);
        string authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                         $"client_id={clientId}&" +
                         $"response_type=code&" +
                         $"scope=openid%20email%20profile&" +
                         $"redirect_uri={encodedRedirectUri}";

        Application.OpenURL(authUrl);

        listener.GetContextAsync().ContinueWith(task => 
        {
            var context = task.Result;
            string code = context.Request.QueryString.Get("code");

            // context(브라우저)에 전달할 내용 및 설정
            byte[] contentBuffer = HTTP.GetCloseTabResponseBuffer();
            context.Response.ContentLength64 = contentBuffer.Length;
            context.Response.ContentType = HTTP.ContentType.HTML;
            context.Response.OutputStream.Write(contentBuffer, 0, contentBuffer.Length);
            context.Response.OutputStream.Close();

            listener.Stop();

            Debug.Log("<color=green>로그인 성공! 인증 코드: </color>" + code);
        });
    }
}
