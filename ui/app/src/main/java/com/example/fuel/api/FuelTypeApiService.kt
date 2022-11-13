package com.example.fuel.api

import com.example.fuel.model.FuelType
import com.example.fuel.model.page.Page
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.QueryMap

interface FuelTypeApiService {

    @GET("api/v1/fuel-types")
    suspend fun getFuelTypes(
        @QueryMap pageRequest: Map<String, String>
    ): Response<Page<FuelType>>
}