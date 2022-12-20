package com.example.fuel.api

import com.example.fuel.model.account.UserLogin
import com.example.fuel.model.account.UserRegistration
import retrofit2.Response
import retrofit2.http.*

interface AccountApiService {

    @FormUrlEncoded
    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/login")
    suspend fun postLogin(
        @Body user: UserLogin
    ): Response<UserLogin>

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/register")
    suspend fun postRegister(
        @Body user: UserRegistration
    ): Response<UserRegistration>


}