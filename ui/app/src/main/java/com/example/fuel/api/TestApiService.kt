package com.example.fuel.api

import com.example.fuel.model.Test
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Headers

interface TestApiService {
    @GET("api/v1/test/anonymous")
    suspend fun getTest(): Response<Test>
}