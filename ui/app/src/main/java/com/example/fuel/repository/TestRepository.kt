package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.Test
import retrofit2.Response

class TestRepository {
    suspend fun getTest(): Response<Test> {
        return RetrofitInstance.testApi.getTest()
    }
}