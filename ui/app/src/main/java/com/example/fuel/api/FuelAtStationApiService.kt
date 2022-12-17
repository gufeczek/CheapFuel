package com.example.fuel.api

import com.example.fuel.model.fuelAtStation.AddFuelToStation
import com.example.fuel.model.fuelAtStation.FuelAtStation
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Path

interface FuelAtStationApiService {

    @POST("api/v1/fuel-at-station")
    suspend fun createFuelAtStation(
        @Body fuelAtStation: AddFuelToStation,
        @Header("Authorization") authHeader: String
    ): Response<FuelAtStation>

    @DELETE("api/v1/fuel-at-station/{fuelStationId}/{fuelTypeId}")
    suspend fun deleteFuelAtStation(
        @Path("fuelStationId") fuelStationId: Long,
        @Path("fuelTypeId") fuelTypeId: Long,
        @Header("Authorization") authHeader: String
    ): Response<Void>

}