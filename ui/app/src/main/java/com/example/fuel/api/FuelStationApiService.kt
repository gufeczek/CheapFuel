package com.example.fuel.api

import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.SimpleMapFuelStation
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface FuelStationApiService {

    @POST("api/v1/fuel-stations")
    suspend fun getSimpleMapFuelStations(
        @Body filter: FuelStationsFilter
    ): Response<Array<SimpleMapFuelStation>>
}