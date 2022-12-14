package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.price.NewFuelPriceResponse
import com.example.fuel.model.price.NewPricesAtFuelStation
import retrofit2.Response

class FuelPriceRepository {

    suspend fun createNewFuelPrices(fuelPrices: NewPricesAtFuelStation): Response<Array<NewFuelPriceResponse>> {
        return RetrofitInstance.fuelPriceApiService.createFuelPrices(fuelPrices, Auth.token);
    }
}