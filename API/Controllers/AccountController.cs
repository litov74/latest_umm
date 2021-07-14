using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Common.Enums;
using API.Common.Interfaces;
using API.Common.Models;
using API.Common.Services;
using API.Configuration;
using API.Controllers.Models.Accounts.Request;
using API.Controllers.Models.Accounts.Response;
using API.Controllers.Models.PlanSubscribe.Request;
using API.Data;
using API.Database.Entities;
using API.Models.Entities;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : CoreBaseControler
    {
        private readonly ILogger<AccountController> _logger;
        private readonly APIContext _context;
        private readonly HashingService _hashingService;
        private readonly TokenService _tokenService;
        private readonly GenrateCodeSerivce _genCodeSerivce;
        private readonly SendGridEmailService _emailService;
        private readonly IUserClaimService _userClaimService;
        private readonly AppSettings.CoreLogicSettingModel _coreLogicSetting;
        private readonly NotificationService _notificationService;

        public AccountController(IConfiguration configuration,
            ILogger<AccountController> logger,
            APIContext context,
            HashingService hashingService,
            TokenService tokenService,
            GenrateCodeSerivce authenticationSerivce,
            NotificationService notificationService,
            SendGridEmailService emailService,
            IUserClaimService userClaimService
            )
        {
            _coreLogicSetting = configuration.Get<AppSettings>().CoreLogicSettings;
            _logger = logger;
            _context = context;
            _hashingService = hashingService;
            _tokenService = tokenService;
            _genCodeSerivce = authenticationSerivce;
            _notificationService = notificationService;
            _emailService = emailService;
            _userClaimService = userClaimService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<SignInResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public IActionResult SignIn([FromBody] SignInVm model)
        {
            try
            {
                var user = _context.UserEntity.FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower());
                if (user == null)
                {
                    return BadRequestResponse("Your email or password is not correct.");
                }
                var isPasswordValid = _hashingService.VerifyHashedPassword(user.Password, model.Password);
                if (!isPasswordValid)
                {
                    return BadRequestResponse("Your email or password is not correct.");
                }
                if (!user.IsEmailConfirm)
                {
                    return BadRequestResponse("Email is not confirmed.");
                }

                Dictionary<string, string> additionalClaims = new Dictionary<string, string>();
                if (user.Email == _coreLogicSetting.GlobalAdminEmail)
                {
                    additionalClaims.Add(ClaimTypes.Role, Roles.GlobalAdmin);
                }
                else
                {
                    additionalClaims.Add(ClaimTypes.Role, Roles.User);
                }

                (string accessToken, string tokenType, int expiresIn) = _tokenService.GenerateAccessToken(user, additionalClaims, model.IsRememberMe);
                var result = new SignInResponse
                {
                    AccessToken = accessToken,
                    TokenType = tokenType,
                    ExpiresIn = expiresIn,
                };
                return OKResponse(result, "Your Account Has Been Successfully Verified");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<IActionResult> SignUp([FromBody] SignUpVm model)
        {
            var company = _context.CompanyEntity.FirstOrDefault(c => c.CompanyName.ToLower() == model.CompanyName.ToLower());
            var nameSplit = model.Name.Split(" ");
            var user = _context.UserEntity.FirstOrDefault(x => x.Email == model.Email.ToLower());
            foreach (var itemType in await Task.FromResult(Enum.GetValues(typeof(CompanySubscriptionPlanEnum))))
            {
                if (model.Plan == Enum.GetName(typeof(CompanySubscriptionPlanEnum), model.Plan))
                {
                    break;
                }
                else
                {
                    return BadRequestResponse("Please Choose valid Plan");
                }
            }
            var subscriptionTierentity = _context.SubscriptionTierEntities.Where(x => x.TierName.ToString() == Enum.GetName(typeof(CompanySubscriptionPlanEnum), model.Plan)).Select(x => new
            {
                x.Id,
                x.TierName
            }).FirstOrDefault();
            if (subscriptionTierentity == null)
            {
                return BadRequestResponse("You Entered Plan Is Not Found!");
            }
            if (company != null)
            {
                return BadRequestResponse("Company was existed");
            }

            if (user != null && user.IsEmailConfirm)
            {
                var planEntity = _context.PlanSubscribeEntities.FirstOrDefault(x => x.UserId == user.Id);

                user.FirstName = nameSplit.First();
                user.LastName = nameSplit.Last();
                user.Email = model.Email.ToLower();
                user.Password = _hashingService.HashPassword(model.Password);
                user.SubscriptionTierId = subscriptionTierentity.Id;
                planEntity.Plan = (Enum.GetName(typeof(CompanySubscriptionPlanEnum), model.Plan));
                planEntity.SubscriptionTierId = subscriptionTierentity.Id;
                planEntity.Plan = (Enum.GetName(typeof(CompanySubscriptionPlanEnum), subscriptionTierentity.TierName));
                await _context.SaveChangesAsync();
                return OKResponse("", $"Your Are Successfully Upgraded to {model.Plan} plan");
            }

            if (user != null && !user.IsEmailConfirm)
            {
                return OKResponse("", "Please confirm your email and try login to the system");
            }

            var newCompany = new CompanyEntity
            {
                Id = FuncHelppers.GenerateGUID(),
                CompanyName = model.CompanyName
            };
            var newUser = new UserEntity
            {
                Id = FuncHelppers.GenerateGUID(),
                FirstName = nameSplit.First(),
                LastName = nameSplit.Length > 1 ? nameSplit.Last() : "",
                Email = model.Email.ToLower(),
                Password = _hashingService.HashPassword(model.Password),
                SubscriptionTierId = subscriptionTierentity.Id
            };
            var companyUserMapping = new CompanyUserMappingEntity
            {
                CompanyId = newCompany.Id,
                UserId = newUser.Id,
                AccessLevel = model.AccessLevels
            };

            await _context.AddAsync(newCompany);
            await _context.AddAsync(newUser);
            await _context.AddAsync(companyUserMapping);
            await _context.SaveChangesAsync();
            // if existed send verify
            var code = _genCodeSerivce.GenarateVerifyCode(VerifyTypeEnum.VerifyEmail);
            newUser.EmailVerifyCode = code;
            _context.Update(newUser);
            await _context.SaveChangesAsync();
            await _notificationService.SendVerifyEmail(newUser.Email, newUser.EmailVerifyCode, model.Plan);
            return OKResponse("", "Link to activate your account has been sent to your mailbox");
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<IActionResult> VerificationLinkSentAgain(string email, CompanySubscriptionPlanEnum subscribePlan)
        {
            var findEmail = _context.UserEntity.FirstOrDefault(x => x.Email == email);
            if (findEmail == null)
            {
                return BadRequestResponse("Email Address Not Registered!");
            }
            await _notificationService.SendVerifyEmail(findEmail.Email, findEmail.EmailVerifyCode, Enum.GetName(typeof(CompanySubscriptionPlanEnum), subscribePlan));
            return OKResponse("", "Email Verification Link Sent Again To Your Mail Address");
        }
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        //[Route("[action]")]
        //public async Task<IActionResult> PlanSubscribe([FromBody] Models.PlanSubscribe.Request.PlanSubscribeVm planSubscribeVm)
        //{
        //    var subscriptionPlan = "";
        //    var currentUserId = _userClaimService.GetCurrentUserId();
        //    var user = await _context.UserEntity
        //      .Include(x => x.CompanyMappings)
        //      .ThenInclude(x => x.Company)
        //      .FirstOrDefaultAsync(x => x.Id == currentUserId && !x.IsDeleted && x.Email == planSubscribeVm.Email);
        //    //var user = _context.UserEntity.FirstOrDefault(x => x.Email == planSubscribeVm.Email && x.Id == currentUserId && x.IsPaymentVerified);
        //    var subscriptionTier = _context.SubscriptionTierEntities.FirstOrDefault(x => x.TierName == planSubscribeVm.Plan);
        //    var company = _context.CompanyEntity.Where(x => x.CompanyName == planSubscribeVm.CompanyName.ToLower().Trim()).Select(x => new
        //    {
        //        x.Id
        //    }).FirstOrDefaultAsync();
        //    if (company == null)
        //    {
        //        return BadRequestResponse("Company Name Missmatch!");
        //    }
        //    if (subscriptionTier != null)
        //    {
        //        subscriptionPlan = Enum.GetName(typeof(CompanySubscriptionPlanEnum), subscriptionTier.TierName).ToString();
        //    }
        //    else
        //    {
        //        return BadRequestResponse("Subscription Plan Not Found!");
        //    }
        //    if (planSubscribeVm.Plan == CompanySubscriptionPlanEnum.Trial)
        //    {
        //        return OKResponse("Congratulations Your Free Trial Subscription Plan is Activated Successfully");
        //    }
        //    if (user != null)
        //    {
        //        var planSubscriber = new PlanSubscribeEntity
        //        {
        //            UserId = currentUserId,
        //            FullName = $"{user.FirstName} {user.LastName}",
        //            Email = user.Email,
        //            SubscriptionTierId = subscriptionTier.Id,
        //            Plan = subscriptionPlan,
        //            CompanyId = company.Id.ToString(),
        //            CountUser = planSubscribeVm.CountUser,
        //            CreatedAt = DateTime.UtcNow,
        //            CreatedBy = user.Email
        //        };
        //        await _context.AddAsync(planSubscriber);
        //        await _context.SaveChangesAsync();
        //        return OKResponse($"Your are Successfully Subscribed to {planSubscriber.Plan} Plan.");
        //    }
        //    return OKResponse("", "Email Address Or UserId Not Found");
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<IActionResult> Forgot([FromBody] ForgotAndResendVm model)
        {
            var user = _context.UserEntity.FirstOrDefault(x => x.Email.ToLower() == model.Email);
            if (user == null)
                return OKResponse("", "if email exist, please check your inbox");
            //Gen code
            string code = "";
            if (user.PasswordForgotCode != null && _genCodeSerivce.VerifyCodeDateAndType(user.PasswordForgotCode, VerifyTypeEnum.FogotPassword))
            {
                code = user.PasswordForgotCode;
            }
            else
            {
                code = _genCodeSerivce.GenarateVerifyCode(VerifyTypeEnum.FogotPassword);
                //save code to db
                user.PasswordForgotCode = code;
                _context.UserEntity.Update(user);
                await _context.SaveChangesAsync();
            }
            //Send mail at here
            await _notificationService.SendForgotPassword(user.Email, user.FullName, code, GetUserAgent(), GetUserPlatform());
            return OKResponse("", "if email exist, please check your inbox");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>abc</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("verify-forgot")]
        public async Task<IActionResult> VerifyForgotPasswordCode([FromBody] VerifyCodeVm model)
        {
            var user = await Task.FromResult(_context.UserEntity.FirstOrDefault(x => x.Email == model.Email.ToLower() && x.PasswordForgotCode == model.Code));
            if (user == null)
            {
                return BadRequestResponse("User or code is invalid");
            }
            var isValid = _genCodeSerivce.VerifyCodeDateAndType(model.Code, VerifyTypeEnum.FogotPassword);
            if (!isValid)
            {
                return BadRequestResponse("Code is exprired");
            }
            return OKResponse(model.Code);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<SignInResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVm model)
        {
            var user = _context.UserEntity.FirstOrDefault(x => x.Email == model.Email.ToLower() && x.PasswordForgotCode == model.Code);
            if (user == null)
            {
                return BadRequestResponse("User or code is invalid");
            }
            var isValid = _genCodeSerivce.VerifyCodeDateAndType(model.Code, VerifyTypeEnum.FogotPassword);
            if (!isValid)
            {
                return BadRequestResponse("Code is exprired");
            }

            var passwordHashed = _hashingService.HashPassword(model.Password);
            user.Password = passwordHashed;
            user.PasswordForgotCode = null;
            _context.Update(user);
            await _context.SaveChangesAsync();

            (string accessToken, string tokenType, int expiresIn) = _tokenService.GenerateAccessToken(user, null, false);
            var result = new SignInResponse
            {
                AccessToken = accessToken,
                TokenType = tokenType,
                ExpiresIn = expiresIn,
            };
            return OKResponse(result, "Reset password successful");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<SignInResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyCodeVm model)
        {
            var user = _context.UserEntity.FirstOrDefault(x => x.Email == model.Email.ToLower() && x.EmailVerifyCode == model.Code);

            if (user == null)
            {
                return BadRequestResponse("User or code invalid");
            }
            if (user.IsEmailConfirm)
            {
                return OKResponse(model.Email, "The email verified");
            }

            var isValid = _genCodeSerivce.VerifyCodeDateAndType(model.Code, VerifyTypeEnum.VerifyEmail);
            if (!isValid)
            {
                return BadRequestResponse("Code is exprired");
            }
            //Update confirm email
            user.IsEmailConfirm = true;
            //remove code
            user.EmailVerifyCode = null;
            _context.Update(user);
            await _context.SaveChangesAsync();
            (string accessToken, string tokenType, int expiresIn) = _tokenService.GenerateAccessToken(user, null, false);
            var result = new SignInResponse
            {
                AccessToken = accessToken,
                TokenType = tokenType,
                ExpiresIn = expiresIn,
            };
            return OKResponse(result, "The email verified");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("resend-verify-email")]
        public async Task<IActionResult> ResendVerifyEmail([FromBody] ForgotAndResendVm model)
        {
            var user = _context.UserEntity.FirstOrDefault(x => x.Email == model.Email.ToLower());

            if (user != null && user.IsEmailConfirm)
            {
                return OKResponse(user.Id, "The email verified");
            }

            if (user == null)
            {
                return OKResponse("", "if email exist, please check your inbox");
            }
            //Gen code
            string code = "";
            if (user.EmailVerifyCode != null && _genCodeSerivce.VerifyCodeDateAndType(user.EmailVerifyCode, VerifyTypeEnum.VerifyEmail))
            {
                code = user.EmailVerifyCode;
            }
            else
            {
                code = _genCodeSerivce.GenarateVerifyCode(VerifyTypeEnum.VerifyEmail);
                //save code to db
                user.EmailVerifyCode = code;
                _context.UserEntity.Update(user);
                await _context.SaveChangesAsync();
            }
            //Send mail at here
            await _notificationService.SendVerifyEmail(user.Email, user.EmailVerifyCode);
            return OKResponse("", "if email exist, please check your inbox");
        }
    }
}
