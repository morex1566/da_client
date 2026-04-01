using System;
using UnityEngine;

public enum AuthType 
{ 
    Google, 
    Guest 
}

[System.Serializable]
public class AuthResult
{
    public bool IsSuccess;
    public string UserId;      
    public string DisplayName; 
    public string IdToken;    
    public string ErrorMessage;
}