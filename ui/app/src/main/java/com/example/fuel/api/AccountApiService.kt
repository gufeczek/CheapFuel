package com.example.fuel.api

import com.example.fuel.model.User
import retrofit2.Response
import retrofit2.http.*

interface AccountApiService {

    @FormUrlEncoded
    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/login")
    suspend fun postLogin(
        @Field("username") username: String,
        @Field("password") password: String
    ): Response<User>

    @Headers("Content-Type:application/json")
    @POST("api/v1/accounts/register")
    suspend fun postRegister(
        @Body user: User
    ): Response<User>


}