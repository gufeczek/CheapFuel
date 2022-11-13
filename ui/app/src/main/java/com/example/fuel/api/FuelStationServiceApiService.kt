package com.example.fuel.api

import com.example.fuel.model.FuelStationService
import com.example.fuel.model.page.Page
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.QueryMap

interface FuelStationServiceApiService {

    @GET("api/v1/services")
    suspend fun getFuelStationServices(
        @QueryMap pageRequest: Map<String, String>
    ): Response<Page<FuelStationService>>
}