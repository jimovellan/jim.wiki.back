﻿namespace jim.wiki.core.Authentication.Models;

public class UserData : IUserData
{
    public long? Id { get ; set ; }
    public string? Name { get  ; set ; }
    public string? Email { get ; set ; }
    public string? IP { get ; set ; }
}