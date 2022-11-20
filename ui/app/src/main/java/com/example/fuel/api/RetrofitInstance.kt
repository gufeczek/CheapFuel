package com.example.fuel.api

import com.example.fuel.utils.Constants.Companion.BASE_URL
import com.google.gson.Gson
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit


object RetrofitInstance {

    private val gson by lazy {
        Gson().newBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
            .create()
    }

    private val client by lazy {
        OkHttpClient.Builder()
            .connectTimeout(60, TimeUnit.SECONDS)
            .readTimeout(60, TimeUnit.SECONDS)
            .build()
    }

    private val retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()
    }

    val accountApi: AccountApiService by lazy {
        retrofit.create(AccountApiService::class.java)
    }

    val fuelStationApi: FuelStationApiService by lazy {
        retrofit.create(FuelStationApiService::class.java)
    }

    val fuelTypeApiService: FuelTypeApiService by lazy {
        retrofit.create(FuelTypeApiService::class.java)
    }

    val fuelStationServiceApiService: FuelStationServiceApiService by lazy {
        retrofit.create(FuelStationServiceApiService::class.java)
    }

    val stationChainApiService: StationChainApiService by lazy {
        retrofit.create(StationChainApiService::class.java)
    }

    val reviewApiService: ReviewApiService by lazy {
        retrofit.create(ReviewApiService::class.java)
    }

    val testApi: TestApiService by lazy {
        retrofit.create(TestApiService::class.java)
    }
}