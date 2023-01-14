package com.example.fuel.api

import com.example.fuel.model.block.BlockUser
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface BlockUserApiService {

    @POST("api/v1/blocked-users")
    suspend fun blockUser(
        @Body block: BlockUser,
        @Header("Authorization") authHeader: String
    ): Response<BlockUser>
}