package com.example.fuel.api

import com.example.fuel.model.UserDetails
import com.example.fuel.model.UserFilter
import com.example.fuel.model.page.Page
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.QueryMap

interface UserApiService {

    @POST("api/v1/users")
    suspend fun getAllUsers(
        @Body filter: UserFilter,
        @QueryMap pageRequest: Map<String, String>,
        @Header("Authorization") authHeader: String
    ): Response<Page<UserDetails>>

    @GET("api/v1/users/admin/{username}")
    suspend fun getUserDetails(
        @Path("username") username: String,
        @Header("Authorization") authHeader: String
    ): Response<UserDetails>
}