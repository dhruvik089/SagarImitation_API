namespace SagarImitation.Data
{
    public class StoredProcedures
    {
        #region Login
        public const string LoginUser = "SP_LoginUser";
        public const string GetUserSaltByEmail = "SP_UserGetSaltByEmail";
        public const string UpdateLoginToken = "SP_UpdateLoginToken";
        public const string LogoutUser = "SP_Logout";
        public const string ForgetPassword = "SP_UserForgetPassword";
        public const string SaveOTP = "SP_EmailOTPAdd";
        public const string GetUserIdByEmail = "SP_GetUserIDByEmail";
        public const string VerifyOTP = "SP_EmailOTPVerify";
        public const string ResetPassword = "SP_ResetPassword";
        public const string ChangePassword = "SP_ChangePassword";
        public const string GetPasswordByUserId = "SP_GetPasswordByUserId";
        public const string UserStatusActiveInActive = "SP_UserActiveInActive";
        public const string ValidateToken = "SP_VerifyJWTToken";
        public const string GetUserProfile = "SP_GetUserProfileByID";
        public const string EditProfile = "SP_EditProfile";
        public const string ForgetPasswordWithLink = "SP_UserForgetPasswordWithLink";
        public const string ValidateResetPassword = "SP_ValidateResetPassword";
        public const string ResetPasswordWithLink = "SP_UpdatePasswordWithLink";
        #endregion

        #region User
        public const string SaveUser = "SP_SaveUser";
        public const string GetUserList = "SP_GetUserList";
        public const string GetUserById = "SP_GetUserById";
        public const string DeleteUser = "SP_DeleteUser";
        public const string ActiveInActiveUser = "SP_ActiveInActiveUser";
        #endregion

        #region Product
        public const string SaveProduct = "SP_SaveProduct";
        public const string GetProductDetailsById = "SP_GetProductDetailsById";
        public const string DeleteProduct = "SP_DeleteProduct";
        public const string ProductList = "SP_ProductList";
        public const string ActiveInactiveProduct = "SP_ActiveInactiveProduct";
        #endregion

        #region Category
        public const string CategotyListForDropDown = "SP_GetCategoryListForDropDown";
        public const string ActiveInActiveCategory = "SP_ActiveInactiveCategory";
        public const string DeleteCategory = "SP_DeleteCategory";
        public const string CategoryList = "SP_CategoryList";
        public const string CategoryListById = "SP_GetCategoryById";
        public const string SaveCategory = "SP_SaveCategory";
        #endregion
    }
}