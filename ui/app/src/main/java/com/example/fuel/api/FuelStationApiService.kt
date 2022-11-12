package com.example.fuel.api

import com.example.fuel.model.SimpleMapFuelStation
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Query

interface FuelStationApiService {

    @GET("api/v1/fuel-stations")
    suspend fun getSimpleMapFuelStations(
        @Query("fuelTypeId") furlTypeId: Long
    ): Response<Array<SimpleMapFuelStation>>
}