package com.example.fuel.api

import com.example.fuel.utils.Constants.Companion.BASE_URL
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


object RetrofitInstance {

    private val retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }


    val accountApi: AccountApiService by lazy {
        retrofit.create(AccountApiService::class.java)
    }

    val testApi: TestApiService by lazy {
        retrofit.create(TestApiService::class.java)
    }
}