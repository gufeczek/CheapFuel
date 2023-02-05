package com.example.fuel.api

import com.example.fuel.model.Email
import com.example.fuel.model.Token
import com.example.fuel.model.account.ChangePassword
import com.example.fuel.model.account.UserLogin
import com.example.fuel.model.account.UserPasswordReset
import com.example.fuel.model.account.UserRegistration
import retrofit2.Response
import retrofit2.http.*

interface AccountApiService {

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/login")
    suspend fun postLogin(
        @Body user: UserLogin
    ): Response<Token>

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/register")
    suspend fun postRegister(
        @Body user: UserRegistration
    ): Response<UserRegistration>

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/generate-password-reset-token")
    suspend fun postPasswordResetToken(
        @Body email: Email,
    ): Response<Void>

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/reset-password")
    suspend fun postResetPassword(
        @Body userPasswordReset: UserPasswordReset
    ): Response<Void>

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/change-password")
    suspend fun postChangePassword(
        @Body changePassword: ChangePassword,
        @Header("Authorization") authHeader: String
    ): Response<Void>

    @DELETE("api/v1/accounts/{username}")
    suspend fun deactivateAccount(
        @Path("username") username: String,
        @Header("Authorization") authHeader: String
    ): Response<Void>
}