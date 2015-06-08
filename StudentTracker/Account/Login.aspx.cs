using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using StudentTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace StudentTracker.Account
{
    public partial class Login : Page
    {
        StudentTrackerDBContext db = new StudentTrackerDBContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            RegisterHyperLinkInstructor.NavigateUrl = "InstructorRegister";
            // Enable this once you have account confirmation enabled for password reset functionality
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }

            if (User.Identity.IsAuthenticated)
                Response.Redirect("~/AccessDeny");
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();


                // Require the user to have a confirmed email before they can log on.
                var user = manager.FindByName(Email.Text);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        FailureText.Text = "Invalid login attempt.<br>You must have a confirmed email address.<br>To resend new confirmation through email, then press \"Resend Confirmation\".";
                        ErrorMessage.Visible = true;
                        ResendConfirm.Visible = true;
                    }
                    else
                    {

                        // This doen't count login failures towards account lockout
                        // To enable password failures to trigger lockout, change to shouldLockout: true
                        var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                        switch (result)
                        {
                            case SignInStatus.Success:
                                UpdateLastLogin();
                                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                                //Response.Redirect("~/");
                                break;
                            case SignInStatus.LockedOut:
                                Response.Redirect("/Account/Lockout");
                                break;
                            case SignInStatus.RequiresVerification:
                                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                Request.QueryString["ReturnUrl"],
                                                                RememberMe.Checked),
                                                  true);
                                break;
                            case SignInStatus.Failure:
                            default:
                                FailureText.Text = "Invalid login attempt";
                                ErrorMessage.Visible = true;
                                break;
                        }
                     } // user.EmailConfirmed
                }     // user != null
                else
                {
                   FailureText.Text = "You need to register account using valid email address. Click create account link";
                   ErrorMessage.Visible = true;
                   ResendConfirm.Visible = true;
                }
            }
        }

        //update user last login date time
        protected void UpdateLastLogin()
        {
            var updateLogin = db.Users.SingleOrDefault(u => u.Email == Email.Text.Trim());
            if (updateLogin != null)
            {
                updateLogin.LastLogin = System.DateTime.Now;
                db.SaveChanges();
            }
        }

        protected void SendEmailConfirmationToken(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByName(Email.Text);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    FailureText.Text = "Confirmation email sent. Please view the email and confirm your account.";
                    ErrorMessage.Visible = true;
                    ResendConfirm.Visible = false;
                }
            }
        }

        protected void Email_TextChanged(object sender, EventArgs e)
        {

        }
    }
}