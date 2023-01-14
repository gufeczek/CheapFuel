package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.utils.Auth
import com.example.fuel.model.price.NewFuelPriceResponse
import com.example.fuel.model.price.NewPriceAtFuelStationWithLocation
import com.example.fuel.model.price.NewPricesAtFuelStation
import retrofit2.Response

class FuelPriceRepository {

    suspend fun createNewFuelPricesByOwner(fuelPrices: NewPricesAtFuelStation): Response<Array<NewFuelPriceResponse>> {
        return RetrofitInstance.fuelPriceApiService.createFuelPricesByOwner(fuelPrices, Auth.token);
    }

    suspend fun createNewFuelPricesByUser(fuelPrices: NewPriceAtFuelStationWithLocation): Response<Array<NewFuelPriceResponse>> {
        return RetrofitInstance.fuelPriceApiService.createFuelPricesByUser(fuelPrices, Auth.token)
    }
}