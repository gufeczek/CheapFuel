package com.example.fuel.api

import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.SimpleMapFuelStation
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Path

interface FuelStationApiService {

    @POST("api/v1/fuel-stations")
    suspend fun getSimpleMapFuelStations(
        @Body filter: FuelStationsFilter,
        @Header("Authorization") authHeader: String
    ): Response<Array<SimpleMapFuelStation>>

    @GET("api/v1/fuel-stations/{id}")
    suspend fun getFuelStationDetails(
        @Path("id") fuelStationId: Long,
        @Header("Authorization") authHeader: String
    ): Response<FuelStationDetails>
}