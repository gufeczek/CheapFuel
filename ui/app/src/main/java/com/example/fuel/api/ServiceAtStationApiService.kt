package com.example.fuel.api

import com.example.fuel.model.serviceAtStation.AddServiceToStation
import com.example.fuel.model.serviceAtStation.ServiceAtStation
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Path

interface ServiceAtStationApiService {

    @POST("api/v1/service-at-station")
    suspend fun createServiceAtStation(
        @Body serviceAtStation: AddServiceToStation,
        @Header("Authorization") authHeader: String
    ): Response<ServiceAtStation>

    @DELETE("api/v1/service-at-station/{fuelStationId}/{serviceId}")
    suspend fun deleteServiceAtStation(
        @Path("fuelStationId") fuelStationId: Long,
        @Path("serviceId") serviceId: Long,
        @Header("Authorization") authHeader: String
    ): Response<Void>
}