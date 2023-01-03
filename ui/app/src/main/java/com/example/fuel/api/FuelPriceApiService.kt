package com.example.fuel.api

import com.example.fuel.model.price.NewFuelPriceResponse
import com.example.fuel.model.price.NewPriceAtFuelStationWithLocation
import com.example.fuel.model.price.NewPricesAtFuelStation
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface FuelPriceApiService {

    @POST("api/v1/fuel-prices/owner")
    suspend fun createFuelPricesByOwner(
        @Body fuelPrices: NewPricesAtFuelStation,
        @Header("Authorization") authHeader: String
    ): Response<Array<NewFuelPriceResponse>>

    @POST("api/v1/fuel-prices/user")
    suspend fun createFuelPricesByUser(
        @Body fuelPrices: NewPriceAtFuelStationWithLocation,
        @Header("Authorization") authHeader: String
    ): Response<Array<NewFuelPriceResponse>>
}