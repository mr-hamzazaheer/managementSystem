using System.ComponentModel.DataAnnotations;


namespace Shared.Enums;

public enum ActivityAction
{
    // General User Actions
    [Display(Name = "Login")]
    Login = 1,

    [Display(Name = "Logout")]
    Logout = 2,

    [Display(Name = "Change Password")]
    ChangePassword = 3,

    [Display(Name = "Reset Password")]
    ResetPassword = 4,

    // User Management
    [Display(Name = "Create User")]
    CreateUser = 10,

    [Display(Name = "Update User")]
    UpdateUser = 11,

    [Display(Name = "Delete User")]
    DeleteUser = 12,

    [Display(Name = "Assign Role")]
    AssignRole = 13,

    // Entity Actions
    [Display(Name = "Create")]
    Create = 20,

    [Display(Name = "Update")]
    Update = 21,

    [Display(Name = "Delete")]
    Delete = 22,

    [Display(Name = "View")]
    View = 23,

    [Display(Name = "Export")]
    Export = 24,

    [Display(Name = "Import")]
    Import = 25,

    // Workflow Actions
    [Display(Name = "Approve")]
    Approve = 30,

    [Display(Name = "Reject")]
    Reject = 31,

    [Display(Name = "Submit")]
    Submit = 32,

    [Display(Name = "Cancel")]
    Cancel = 33,

    [Display(Name = "Archive")]
    Archive = 34,

    [Display(Name = "Restore")]
    Restore = 35,

    [Display(Name = "Download")]
    Download = 36,

    // System Actions
    [Display(Name = "System Job Started")]
    SystemJobStarted = 40,

    [Display(Name = "System Job Completed")]
    SystemJobCompleted = 41,

    [Display(Name = "System Job Failed")]
    SystemJobFailed = 42,

    // Security Events
    [Display(Name = "Access Denied")]
    AccessDenied = 50,

    [Display(Name = "Token Refreshed")]
    TokenRefreshed = 51,

    [Display(Name = "Register")]
    Register = 51
}

